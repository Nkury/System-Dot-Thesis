/************************************************************************************************* 
 * Programmer: Nizar Kury
 * Date:       7/14/16
 * Updated:    8/28/17
 * Purpose:    An object that parses code and returns actions game objects can perform in System Dot.  
 *             Code has to conform to the following grammar:
 *             
 *   [1]     program            ->        stmt_list |  Є
 *   [2]     stmt_list          ->        stmt  stmt_list  |  stmt
 *   [3]     stmt               ->        assign_stmt  |  while_stmt  |  if_stmt  |  for_stmt  |  sys_stmt
 *           stmt               ->        comment_stmt
 *   [4]     type_name          ->        INT  |  DOUBLE  |  BOOLEAN  |  STRING
 *   [5]     assign_stmt        ->        type_name  ID  EQUAL  (numExpr  |  strExpr  |  boolExpr)  SEMICOLON
 *           assign_stmt        ->        ID EQUAL (numExpr  |  strExpr  |  boolExpr) SEMICOLON
 *   [6]     while_stmt         ->        WHILE  condition  body
 *   [7]     if_stmt            ->        IF  condition  body  elseif_stmt  else_stmt
 *   [8]     elseif_stmt        ->        ELSEIF  condition  body elseif_stmt  |   Є
 *   [9]     else_stmt          ->        ELSE  body  |  Є
 *   [10]    for_stmt           ->        FOR for_condition_stmt  body
 *   [11]    numExpr            ->        numTerm  PLUS  numExpr  |  numTerm  MINUS  numExpr  |  numTerm
 *   [12]    numTerm            ->        numFactor  (MULT  |  DIV  |  MOD)  numTerm  |  numFactor
 *   [13]    numFactor          ->        LPAREN  numExpr  RPAREN  |  NUM  |  REALNUM  |  ID
 *   [14]    strExpr            ->        (QUOTE  ID  QUOTE  |  DOUBLEQUOTE)  |  strExpr PLUS strExpr
 *   [15]    boolExpr           ->        TRUE  |  FALSE  | primary relop primary | LPAREN  boolExpr  RPAREN
 *           boolExpr           ->        boolExpr  AND  boolExpr  |  boolExpr  OR  boolExpr
 *   [16]    condition          ->        LPAREN (ID  |  primary  relop  primary) RPAREN
 *                                           <when evaluating condition, primary has to be the same type>
 *   [17]    primary            ->        ID  |  NUM  |  REALNUM  |  TRUE  |  FALSE
 *   [18]    relop              ->        GREATER  |  GTEQ  |  LESS  |  LTEQ  |  NOTEQUAL  |  EQUALEQUAL
 *   [19]    body               ->        LBRACE  stmt_list  RBRACE
 *   [20]    for_condition_stmt ->        LPAREN  assign_stmt  SEMICOLON  primary  relop  primary  
 *                                               SEMICOLON  assign_stmt  RPAREN
 *                                    <first assign_stmt needs to be a fresh assignment i.e. int x = 0
 *                                          if there is a variable already instantiated, then x = 0 is fine>
 *                                    <primary relop primary should deal with variable assigned in
 *                                          first assign_stmt>
 *                                    <second assign_stmt should increment/decrement variable defined in
 *                                          first assign_stmt>
 *   [21]    sys_stmt           ->        SYSTEM  DOT  (jump_stmt  |  body_stmt  |  open_stmt  |
 *                                                      close_stmt  |  move_stmt  |  check_stmt  |
 *                                                      output_stmt  | wait_stmt)  SEMICOLON
 *   [22]    jump_stmt          ->        JUMP  LPAREN  RPAREN
 *   [23]    body_stmt          ->        BODY  LPAREN  COLOR  DOT  (BLUE  |  GREEN  |  BLUE  |  BLACK)
 *                                              RPAREN
 *   [24]    open_stmt          ->        OPEN  LPAREN  RPAREN
 *   [25]    close_stmt         ->        CLOSE  LPAREN  RPAREN
 *   [26]    move_stmt          ->        MOVE  LPAREN  DIRECTION  DOT  (RIGHT  |  LEFT)  RPAREN
 *   [27]    check_stmt         ->        CHECK  LPAREN  ID  RPAREN
 *              <returns a boolean>
 *   [28]    output_stmt        ->        OUTPUT  LPAREN  ID  RPAREN
 *   [29]    wait_stmt          ->        WAIT  LPAREN  (NUM  |  REALNUM)  RPAREN
 *   [30]    comment_stmt       ->        DOUBLESLASH   <characters>    NEWLINE (\n)
 *                                       <characters represent anything> 
 *   [31]    smash_stmt         ->        SMASH LPAREN RPAREN           
 *   [32]    gravity_stmt       ->        GRAVITY LPAREN (TRUE | FALSE | ID) RPAREN 
 *   [33]    activate_stmt      ->        ACTIVATE LPAREN (NUM | REALNUM | ID) RPAREN       
 *   [34]    rotate_stmt        ->        ROTATE LPAREN ( NUM | REALNUM | ID) RPAREN
 *   [35]    delete_stmt        ->        DELETE LPAREN ( ID | strExpr) RPAREN 
 *   [36]    sys_distance_stmt  ->        SYSTEM DOT DISTANCE LPAREN ID RPAREN (SEMICOLON)* 
 *               Unlike other functions, System.Distance() will be evaluated on the EnemyTerminal side.
 *               The parser will return a list of strings of the complete System.distance command for replacement
 *                  with the actual number.             
 *   (-=-=-=-=-=-=- The following grammar are for methods and can be substituted into the above grammar
 *                  based on their return type -=-=-=-=-=-)
 *   [37]    substr_method      ->        ID  DOT  SUBSTRING  LPAREN  (NUM  |  NUM  COMMA  NUM  )  RPAREN  SEMICOLON
 *             <STRING>               <first ID needs to be an existing string variable>
 *                                    <NUMs needs to be in range of the string>
 *                                    <for NUM comma NUM, the first NUM < second NUM>
 *   [38]    indexOf_method     ->        ID  DOT  INDEXOF  LPAREN  ID  RPAREN  SEMICOLON
 *             <NUM>                  <first ID needs to be an existing string variable>
 *   [39]    length_method      ->        ID  DOT  LENGTH  LPAREN  RPAREN  SEMICOLON                                                    
 *             <NUM>                  <first ID needs to be an existing string variable>
 *             
 *             Whenever the code does not conform to the above grammar, an ERROR action will be returned.
 *             When an ERROR action is returned and the user has messed up more than two times, then
 *             the syntaxMessage variable will store an appropriate message detailing the syntax error.
 *             
 *             Infinite loops are possible. If a loop loops more than 100 times, then we will conclude
 *             that it is an infinite loop and add the INFINTELOOP action.
 *             
 *             Some common programming syntax that has NOT been included in the above grammar include:
 *              - short-hand incrementation/decrementation (++/--)
 *                     
*************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ParserAlgo
{

    public enum keyActions
    {
        // color state changing through System.body(Color.BLUE/RED/GREEN);
        // color turns black if there is no System.body function
        TURNBLUE = 1, TURNRED, TURNGREEN, TURNBLACK,

        // letter state changing through System.body(ID / strExpr);
        TURNLETTER,

        // System.move(Direction.LEFT/RIGHT)
        MOVELEFT, MOVERIGHT, NOMOVE,

        // System.jump()
        JUMP,

        // System.open/close()
        OPEN, CLOSE,

        // System.output(variable_name) will allow user to see value of a variable
        OUTPUT,

        // System.wait(num/realnum)
        WAIT,

        // System.smash()
        SMASH,

        // System.gravity(true/false)
        GRAVITYON, GRAVITYOFF,

        // System.activate(num/realnum/id)
        // activation power outputted in outputValue in the format: ["Activate: #"]
        ACTIVATE,

        // System.rotate(num/realnum/id)
        // amount to rotate outputted in outputValue in format ["Rotate: #"]
        ROTATE,

        // System.delete(id/strExpr)
        // delete certain strings from a word
        DELETE,

        // System.distance(id)
        // calculate distance of object
        DISTANCE,

        // if there is an infinite loop or syntax error with message
        INFINITELOOP, ERRORMSG, ERROR

    }   

    public class Parser
    {
        string[] reserved = {"",
                            "while", "for", "if", "else",
                            "int", "double", "string", "boolean", "true", "false",
                            "substring", "length", "indexOf",
                            "System", "output", "check", "move", "body", "jump", "open", "close", "wait",
                            "smash", "gravity", "activate", "rotate", "delete", "distance",
                            "Direction", "LEFT", "RIGHT", "UP", "DOWN", "Color", "BLACK", "RED", "BLUE", "GREEN",
                            "+", "-", "/", "//", "*","%", "=",
                            ":", ",", ";",
                            "[", "]", "(", ")", "{", "}",
                            "<>", ">", "<", "<=", ">=", ".",
                            "&&", "||", "!", "==", "!=",
                            "ID", "NUM", "REALNUM",
                            "ERROR"
                        };

        public struct variable
        {
            public string name;
            public string value;
            public TokenTypes type;
        }

        public struct numberOrString
        {
            public TokenTypes tag; // tag the numberOrString as either REAL, INT, STRING, or BOOLEAN
            public string value;
        }

        // token type values need to be aligned with reserved values in the order in which they appear
        public enum TokenTypes
        {
            WHILE = 1, FOR, IF, ELSE,
            INT, REAL, STRING, BOOLEAN, TRUE, FALSE,
            SUBSTRING, LENGTH, INDEXOF,
            SYSTEM, OUTPUT, CHECK, MOVE, BODY, JUMP, OPEN, CLOSE, WAIT,
            SMASH, GRAVITY, ACTIVATE, ROTATE, DELETE, DISTANCE,
            DIRECTION, LEFT, RIGHT, UP, DOWN, COLOR, BLACK, RED, BLUE, GREEN,
            PLUS, MINUS, DIV, DOUBLESLASH, MULT, MOD, EQUAL,
            COLON, COMMA, APOSTROPHE, QUOTE, DOUBLEQUOTE, SEMICOLON, 
            LBRAC, RBRAC, LPAREN, RPAREN, LBRACE, RBRACE,
            GREATER, LESS, LTEQ, GTEQ, DOT,
            AND, OR, NOT, EQUALEQUAL, NOTEQUAL,
            ID, NUM, REALNUM,
            EOS,
            ERROR
        };

        // StatementTypes are used to remember what statement we are executing when parsing. For instance,
        //
        // HASIF tells the parser that there is an if so any subsequent else-ifs or elses are valid; 
        // EXECUTEIF tells the parser that the current if, else-if, or else body is the one that will be
        //           executed so do not evaluate any subsequent else-if or else statements if valid;
        // ISEXECUTING tells the parser that we are currently executing the conditional body so any nested
        //             else-if or else statements need to have a corresponding HASIF to execute;
        // ISWHILE tells the parser that we are in a while loop and will loop as necessary;
        // ISFOR tells the parser that we are in a for loop and reconstructs the code accordingly
        public enum StatementTypes
        {
            HASIF = 1, EXECUTEIF, ISEXECUTING, ISWHILE, ISFOR
        };

        public string syntaxMessage; // for assisting the player with the error in their code
        public List<string> outputValue; // to allow the user to see the value they request.
        public List<float> waitTimes; // when WAIT is an action, tells the object how long to wait

        int errorCount; // keeps track of how many errors have been passed to signal when the
                        // syntax message will be sent to the player

        bool keepParsing = true; // if we keepParsing, then parse. Otherwise,
                                 // we clear everything after the condition and body.
        bool quoteSwitch = false; // if a quote has started, read spaces in scan_keyword and
                                  // scan_number    
                                      
        int activeToken = 0; // used for ungetToken()
        TokenTypes ttype; // token type
        int line_no = 1; // to denote line_no the parser is parsing
        int loopLineNo = 0; // for loops, we need to store the beginning of the loop in a line number
        string code; // code that is being passed in as input
        string token; // the string that is being read when calling getToken()


        // Dictionary format: <value of variable, variable>
        Dictionary<string, variable> symbolTable = new Dictionary<string, variable>();

        // a dummy symbol that will be used to addd variables into the dicitonary
        variable symbol;

        // List of actions that will be performed by the game object who calls Parse()
        List<keyActions> actions = new List<keyActions>();

        List<string> loopCode = new List<string>(); // for loops
        List<int> newAssignments = new List<int>(); // for local and global scopes;

        // can be called by game objects in order to add prexisting symbols for instances
        // where there are predefined values defined in the parameters of those game objects
        public void AddSymbol(string symbolName, string symbolType, string symbolValue)
        {
            variable var = default(variable);
            var.name = symbolName;
            if (symbolType == "STRING")
                var.type = TokenTypes.STRING;
            else if (symbolType == "INT")
                var.type = TokenTypes.INT;
            else if (symbolType == "REAL")
                var.type = TokenTypes.REAL;
            else if (symbolType == "BOOLEAN")
                var.type = TokenTypes.BOOLEAN;
            var.value = symbolValue;
            symbolTable.Add(symbolName, var);
        }

        // parses for System.body(Color.RED/BLUE/GREEN/BLACK);
        private void ParseBody()
        {
            ttype = getToken();
            if (ttype == TokenTypes.BODY)
            {
                ttype = getToken();
                if (ttype == TokenTypes.LPAREN)
                {
                    // check for colors
                    ttype = getToken();
                    if (ttype == TokenTypes.COLOR)
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.DOT)
                        {
                            ttype = getToken();
                            keyActions colorSet = keyActions.TURNBLACK;

                            if (ttype == TokenTypes.RED)
                            {
                                colorSet = keyActions.TURNRED;
                            }
                            else if (ttype == TokenTypes.BLUE)
                            {
                                colorSet = keyActions.TURNBLUE;
                            }
                            else if (ttype == TokenTypes.GREEN)
                            {
                                colorSet = keyActions.TURNGREEN;
                            }

                            ttype = getToken();
                            if (ttype == TokenTypes.RPAREN)
                            {
                                ttype = getToken();
                                if (ttype == TokenTypes.SEMICOLON)
                                {
                                    actions.Add(colorSet);
                                    return;
                                }
                            }
                        }
                    } // check for letters
                    else if (ttype == TokenTypes.ID || ttype == TokenTypes.DOUBLEQUOTE || ttype == TokenTypes.QUOTE)
                    {
                        int outputValLength = outputValue.Count;

                        if (ttype == TokenTypes.DOUBLEQUOTE || ttype == TokenTypes.QUOTE || ttype == TokenTypes.ID)
                        {
                            ungetToken();
                            outputValue.Add("Body: " + ParseStringExpression());
                        }                       

                        ttype = getToken();
                        if (ttype == TokenTypes.RPAREN)
                        {
                            ttype = getToken();
                            if (ttype == TokenTypes.SEMICOLON)
                            {
                                if (outputValue.Count > outputValLength && !outputValue[outputValue.Count - 1].Contains("error404notfoundit'snotworkingbzzzt"))
                                {
                                    actions.Add(keyActions.TURNLETTER);
                                    return;
                                }
                            }
                        }
                    }                   
                }
            }

            syntaxMessage = "Error in line " + (line_no) +
                ": There is an error when you try to change the body of an object";
            actions.Add(keyActions.ERROR);
            return;
        }

        // parses for System.move(Direction.LEFT/RIGHT);
        private void ParseMove()
        {
            ttype = getToken();
            if (ttype == TokenTypes.MOVE)
            {
                ttype = getToken();
                if (ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.DIRECTION)
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.DOT)
                        {
                            ttype = getToken();
                            keyActions moveSet = keyActions.NOMOVE;

                            if (ttype == TokenTypes.LEFT)
                            {
                                moveSet = keyActions.MOVELEFT;
                            }
                            else if (ttype == TokenTypes.RIGHT)
                            {
                                moveSet = keyActions.MOVERIGHT;
                            }

                            ttype = getToken();
                            if (ttype == TokenTypes.RPAREN)
                            {
                                ttype = getToken();
                                if (ttype == TokenTypes.SEMICOLON)
                                {
                                    actions.Add(moveSet);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no) +
                ": There is an error when you try to change the movement of an object";
            actions.Add(keyActions.ERROR);
            return;
        }

        // parses for System.jump();
        private void ParseJump()
        {
            ttype = getToken();
            if (ttype == TokenTypes.JUMP)
            {
                ttype = getToken();
                if (ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.RPAREN)
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.SEMICOLON)
                        {
                            actions.Add(keyActions.JUMP);
                            return;
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when you try to make an object jump";
            actions.Add(keyActions.ERROR);
            return;
        }

        // parses for System.open();
        private void ParseOpen()
        {
            ttype = getToken();
            if (ttype == TokenTypes.OPEN)
            {
                ttype = getToken();
                if (ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.RPAREN)
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.SEMICOLON)
                        {
                            actions.Add(keyActions.OPEN);
                            return;
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when you try to open an object";
            actions.Add(keyActions.ERROR);
            return;
        }

        // parses for System.close();
        private void ParseClose()
        {
            ttype = getToken();
            if (ttype == TokenTypes.CLOSE)
            {
                ttype = getToken();
                if (ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.RPAREN)
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.SEMICOLON)
                        {
                            actions.Add(keyActions.CLOSE);
                            return;
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when you try to close an object";
            actions.Add(keyActions.ERROR);
            return;
        }

        // parses for System.check(ID);
        private numberOrString ParseCheck()
        {
            numberOrString returnVal = default(numberOrString);
            ttype = getToken();
            if(ttype == TokenTypes.CHECK)
            {
                ttype = getToken();
                if(ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if(ttype == TokenTypes.COLOR || ttype == TokenTypes.DIRECTION)
                    {
                        if(ttype == TokenTypes.COLOR)
                        {
                            ttype = getToken();
                            if (ttype == TokenTypes.DOT)
                            {
                                ttype = getToken();
                                if (ttype == TokenTypes.BLUE)
                                {
                                    if (actions.Contains(keyActions.TURNBLUE))
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "true";
                                    }
                                    else
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "false";
                                    }
                                } else if (ttype == TokenTypes.RED)
                                {
                                    if (actions.Contains(keyActions.TURNRED))
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "true";
                                    }
                                    else
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "false";
                                    }
                                } else if (ttype == TokenTypes.GREEN)
                                {
                                    if (actions.Contains(keyActions.TURNGREEN))
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "true";
                                    }
                                    else
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "false";
                                    }
                                } else if(ttype == TokenTypes.BLACK)
                                {
                                    if (actions.Contains(keyActions.TURNBLACK))
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "true";
                                    }
                                    else
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "false";
                                    }
                                }

                                ttype = getToken();
                                if(ttype == TokenTypes.RPAREN)
                                {                             
                                    return returnVal;
                                } 
                            }
                        } else if(ttype == TokenTypes.DIRECTION)
                        {
                            ttype = getToken();
                            if(ttype == TokenTypes.DOT)
                            {
                                ttype = getToken();
                                if (ttype == TokenTypes.RIGHT)
                                {
                                    if (actions.Contains(keyActions.MOVERIGHT))
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "true";
                                    }
                                    else
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "false";
                                    }
                                } else if (ttype == TokenTypes.LEFT)
                                {
                                    if (actions.Contains(keyActions.MOVELEFT))
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "true";
                                    }
                                    else
                                    {
                                        returnVal.tag = TokenTypes.BOOLEAN;
                                        returnVal.value = "false";
                                    }
                                }

                                ttype = getToken();
                                if(ttype == TokenTypes.RPAREN)
                                {
                                    return returnVal;
                                }
                            }
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when you try to check the value of a system field";
            actions.Add(keyActions.ERROR);
            return default(numberOrString);
        }

        // parses for System.output(ID);
        private void ParseOutput()
        {
            ttype = getToken();
            if(ttype == TokenTypes.OUTPUT)
            {
                ttype = getToken();
                if(ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if(ttype == TokenTypes.ID)
                    {
                        variable var;
                        if(symbolTable.TryGetValue(token, out var))
                        {
                            outputValue.Add("Output: " + var.name + "\n" + var.type + "\n" + var.value);
                            ttype = getToken();
                            if(ttype == TokenTypes.RPAREN)
                            {
                                ttype = getToken();
                                if(ttype == TokenTypes.SEMICOLON)
                                {
                                    actions.Add(keyActions.OUTPUT);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when you try to output the value of a system field";
            actions.Add(keyActions.ERROR);
            return;
        }

        // parses for System.wait(NUM/REALNUM);
        private void ParseWait()
        {
            float waitTime = -1;

            ttype = getToken();
            if(ttype == TokenTypes.WAIT)
            {
                ttype = getToken();
                if (ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.NUM || ttype == TokenTypes.REALNUM ||
                        (ttype ==TokenTypes.ID))
                    {
                        if(ttype == TokenTypes.NUM)
                        {
                            waitTime = Int32.Parse(token);
                        }
                        else if(ttype == TokenTypes.REALNUM)
                        {
                            waitTime = float.Parse(token);
                        } else if(ttype == TokenTypes.ID)
                        {
                            variable var;
                            if(symbolTable.TryGetValue(token, out var))
                            {
                                if(var.type == TokenTypes.INT || var.type == TokenTypes.NUM)
                                {
                                    waitTime = Int32.Parse(var.value);
                                } else if(var.type == TokenTypes.REAL || var.type == TokenTypes.REALNUM)
                                {
                                    waitTime = float.Parse(var.value);
                                }                                
                            }
                        }

                        ttype = getToken();
                        if(ttype == TokenTypes.RPAREN)
                        {
                            ttype = getToken();
                            if(ttype == TokenTypes.SEMICOLON)
                            {
                                if (waitTime >= 0)
                                {
                                    actions.Add(keyActions.WAIT);
                                    waitTimes.Add(waitTime);
                                    return;
                                }
                            }
                            else
                            {
                                syntaxMessage = "Error in line " + (line_no - 1) +
                                    ": Wait times cannot be negative";
                            }
                        }
                    }
                }
            }

            if(syntaxMessage == "")
                syntaxMessage = "Error in line " + (line_no - 1) +
                    ": There is an error when you try to stall the system";
            actions.Add(keyActions.ERROR);
            return;
        }

        // parses for System.smash();
        private void ParseSmash()
        {
            ttype = getToken();
            if (ttype == TokenTypes.SMASH)
            {
                ttype = getToken();
                if (ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.RPAREN)
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.SEMICOLON)
                        {
                            actions.Add(keyActions.SMASH);
                            return;
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when you try to smash an object";
            actions.Add(keyActions.ERROR);
            return;
        }

        // parses for System.gravity(true/false);
        private void ParseGravity()
        {
            ttype = getToken();
            if (ttype == TokenTypes.GRAVITY)
            {
                ttype = getToken();
                if (ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.TRUE || ttype == TokenTypes.FALSE ||
                        ttype == TokenTypes.ID)
                    {
                        if(ttype == TokenTypes.TRUE)
                        {
                            actions.Add(keyActions.GRAVITYON);
                        } else if(ttype == TokenTypes.FALSE)
                        {
                            actions.Add(keyActions.GRAVITYOFF);
                        } else if(ttype == TokenTypes.ID)
                        {
                            variable var;
                            if (symbolTable.TryGetValue(token, out var))
                            {
                                if (var.type == TokenTypes.BOOLEAN && var.value == "true")
                                {
                                    actions.Add(keyActions.GRAVITYON);
                                }
                                else if (var.type == TokenTypes.BOOLEAN && var.value == "false")
                                {
                                    actions.Add(keyActions.GRAVITYOFF);
                                }
                                else
                                {
                                    syntaxMessage = "Error in line " + (line_no - 1) +
                                    ": There is an error when you try to alter gravity for an object";
                                    actions.Add(keyActions.ERROR);
                                    return;
                                }
                            }
                        }

                        ttype = getToken();
                        if (ttype == TokenTypes.RPAREN)
                        {
                            ttype = getToken();
                            if (ttype == TokenTypes.SEMICOLON)
                            {
                                return;
                            }
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when you try to alter gravity for an object";
            actions.Add(keyActions.ERROR);
            return;
        }

        // parses for System.activate(num/realnum/id);
        private void ParseActivate()
        {
            int outputValLength = outputValue.Count;

            ttype = getToken();
            if (ttype == TokenTypes.ACTIVATE)
            {
                ttype = getToken();
                if (ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.NUM || ttype == TokenTypes.REALNUM ||
                         (ttype == TokenTypes.ID))
                    {
                        if (ttype == TokenTypes.NUM || ttype == TokenTypes.REALNUM)
                        {
                            outputValue.Add("Activate: " + token);
                        }
                        else if (ttype == TokenTypes.ID)
                        {
                            variable var;
                            if (symbolTable.TryGetValue(token, out var))
                            {
                                if (var.type == TokenTypes.INT || var.type == TokenTypes.NUM || var.type == TokenTypes.REAL || var.type == TokenTypes.REALNUM)
                                {
                                    outputValue.Add("Activate: " + var.value);
                                }                          
                            }
                        }

                        ttype = getToken();
                        if (ttype == TokenTypes.RPAREN)
                        {
                            ttype = getToken();
                            if (ttype == TokenTypes.SEMICOLON)
                            {
                                if (outputValue.Count > outputValLength)
                                {
                                    actions.Add(keyActions.ACTIVATE);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when you try to activate an object";
            actions.Add(keyActions.ERROR);
            return;
        } // ParseActivate()

        // parses for System.activate(num/realnum/id);
        private void ParseRotate()
        {
            int outputValLength = outputValue.Count;

            ttype = getToken();
            if (ttype == TokenTypes.ROTATE)
            {
                ttype = getToken();
                if (ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.NUM || ttype == TokenTypes.REALNUM ||
                         (ttype == TokenTypes.ID) || ttype == TokenTypes.DIRECTION)
                    {
                        if (ttype == TokenTypes.NUM || ttype == TokenTypes.REALNUM)
                        {
                            outputValue.Add("Rotate: " + token);
                        } else if(ttype == TokenTypes.DIRECTION)
                        {
                            ttype = getToken();
                            if(ttype == TokenTypes.DOT)
                            {
                                ttype = getToken();
                                if(ttype == TokenTypes.LEFT)
                                {
                                    outputValue.Add("Rotate: LEFT");
                                } else if(ttype == TokenTypes.RIGHT)
                                {
                                    outputValue.Add("Rotate: RIGHT");
                                } else if(ttype == TokenTypes.UP)
                                {
                                    outputValue.Add("Rotate: UP");
                                } else if(ttype == TokenTypes.DOWN)
                                {
                                    outputValue.Add("Rotate: DOWN");
                                }
                            }
                        }
                        else if (ttype == TokenTypes.ID)
                        {
                            variable var;
                            if (symbolTable.TryGetValue(token, out var))
                            {
                                if (var.type == TokenTypes.INT || var.type == TokenTypes.NUM || var.type == TokenTypes.REAL || var.type == TokenTypes.REALNUM)
                                {
                                    outputValue.Add("Rotate: " + var.value);
                                }
                            }
                        }

                        ttype = getToken();
                        if (ttype == TokenTypes.RPAREN)
                        {
                            ttype = getToken();
                            if (ttype == TokenTypes.SEMICOLON)
                            {
                                if (outputValue.Count > outputValLength)
                                {
                                    actions.Add(keyActions.ROTATE);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when you try to rotate an object";
            actions.Add(keyActions.ERROR);
            return;
        } // ParseRotate()

        // parses for System.delete(id/strExpr);
        private void ParseDelete()
        {
            int outputValLength = outputValue.Count;

            ttype = getToken();
            if (ttype == TokenTypes.DELETE)
            {
                ttype = getToken();
                if (ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.DOUBLEQUOTE || ttype == TokenTypes.QUOTE || ttype == TokenTypes.ID)
                    {
                        ungetToken();
                        outputValue.Add("Delete: " + ParseStringExpression());
                    }                   

                    ttype = getToken();
                    if (ttype == TokenTypes.RPAREN)
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.SEMICOLON)
                        {
                            if (outputValue.Count > outputValLength && !outputValue[outputValue.Count - 1].Contains("error404notfoundit'snotworkingbzzzt"))
                            {
                                actions.Add(keyActions.DELETE);
                                return;
                            }
                        }
                    }                    
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when you try to delete an object";
            actions.Add(keyActions.ERROR);
            return;
        } // ParseDelete()

    

        // parses for System. and then evaluates which of the above System commands it should parse for
        private numberOrString ParseSystem()
        {
            ttype = getToken();
            if (ttype == TokenTypes.SYSTEM)
            {
                ttype = getToken();
                if (ttype == TokenTypes.DOT)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.BODY)
                    {
                        ungetToken();
                        ParseBody();
                    }
                    else if (ttype == TokenTypes.MOVE)
                    {
                        ungetToken();
                        ParseMove();
                    }
                    else if (ttype == TokenTypes.JUMP)
                    {
                        ungetToken();
                        ParseJump();
                    }
                    else if (ttype == TokenTypes.OPEN)
                    {
                        ungetToken();
                        ParseOpen();
                    }
                    else if (ttype == TokenTypes.CLOSE)
                    {
                        ungetToken();
                        ParseClose();
                    } else if(ttype == TokenTypes.CHECK)
                    {
                        ungetToken();
                        return ParseCheck();
                    } else if(ttype == TokenTypes.OUTPUT)
                    {
                        ungetToken();
                        ParseOutput();
                    } else if(ttype == TokenTypes.WAIT)
                    {
                        ungetToken();
                        ParseWait();                      
                    } else if (ttype == TokenTypes.SMASH)
                    {
                        ungetToken();
                        ParseSmash();                        
                    } else if(ttype == TokenTypes.GRAVITY)
                    {
                        ungetToken();
                        ParseGravity();
                    } else if(ttype == TokenTypes.ACTIVATE)
                    {
                        ungetToken();
                        ParseActivate();
                    } else if(ttype == TokenTypes.ROTATE)
                    {
                        ungetToken();
                        ParseRotate();
                    } else if(ttype == TokenTypes.DELETE)
                    {
                        ungetToken();
                        ParseDelete();
                    } 
                    return default(numberOrString);
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when you perform a system command";
            actions.Add(keyActions.ERROR);
            return default(numberOrString);
        } // ParseSystem()

        // returns true if it is an int otherwise returns false if it is a decimal.
        private bool checkTag(numberOrString num)
        {
            return num.tag == TokenTypes.INT;
        }

        // performs math on numberOrString types by evaluating their type and then doing the appropriate
        // math for them
        private numberOrString numMath(numberOrString left_op, numberOrString right_op, TokenTypes op)
        {
            numberOrString num = default(numberOrString);

            switch (op)
            {
                case TokenTypes.PLUS:
                    try
                    {
                        if (checkTag(left_op))
                        {
                            if (checkTag(right_op))
                            {
                                num.value = (Int32.Parse(left_op.value) + Int32.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.INT;
                            }
                            else
                            {
                                num.value = (Int32.Parse(left_op.value) + float.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                        }
                        else
                        {
                            if (checkTag(right_op))
                            {
                                num.value = (float.Parse(left_op.value) + Int32.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                            else
                            {
                                num.value = (float.Parse(left_op.value) + float.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                        }
                    }
                    catch
                    {
                        syntaxMessage = "Error in line " + line_no +
                        ": One of the values is not numeric.";
                        return default(numberOrString);
                    }
                    break;
                case TokenTypes.MINUS:
                    try
                    {
                        if (checkTag(left_op))
                        {
                            if (checkTag(right_op))
                            {
                                num.value = (Int32.Parse(left_op.value) - Int32.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.INT;
                            }
                            else
                            {
                                num.value = (Int32.Parse(left_op.value) - float.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                        }
                        else
                        {
                            if (checkTag(right_op))
                            {
                                num.value = (float.Parse(left_op.value) - Int32.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                            else
                            {
                                num.value = (float.Parse(left_op.value) - float.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                        }
                    }
                    catch
                    {
                        syntaxMessage = "Error in line " + line_no +
                        ": One of the values is not numeric.";
                        return default(numberOrString);
                    }
                    break;
                case TokenTypes.MULT:
                    try
                    {
                        if (checkTag(left_op))
                        {
                            if (checkTag(right_op))
                            {
                                num.value = (Int32.Parse(left_op.value) * Int32.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.INT;
                            }
                            else
                            {
                                num.value = (Int32.Parse(left_op.value) * float.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                        }
                        else
                        {
                            if (checkTag(right_op))
                            {
                                num.value = (float.Parse(left_op.value) * Int32.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                            else
                            {
                                num.value = (float.Parse(left_op.value) * float.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                        }
                    }
                    catch
                    {
                        syntaxMessage = "Error in line " + line_no +
                        ": One of the values is not numeric.";
                        return default(numberOrString);
                    }
                    break;
                case TokenTypes.DIV:
                    try
                    {
                        if (checkTag(left_op))
                        {
                            if (checkTag(right_op))
                            {
                                num.value = (Int32.Parse(left_op.value) / Int32.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.INT;
                            }
                            else
                            {
                                num.value = (Int32.Parse(left_op.value) / float.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                        }
                        else
                        {
                            if (checkTag(right_op))
                            {
                                num.value = (float.Parse(left_op.value) / Int32.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                            else
                            {
                                num.value = (float.Parse(left_op.value) / float.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                        }
                        break;
                    }
                    catch
                    {
                        syntaxMessage = "Error in line " + line_no +
                        ": Cannot divide by 0";
                        return default(numberOrString);
                    }
                case TokenTypes.MOD:
                    try
                    {
                        if (checkTag(left_op))
                        {
                            if (checkTag(right_op))
                            {
                                num.value = (Int32.Parse(left_op.value) % Int32.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.INT;
                            }
                            else
                            {
                                num.value = (Int32.Parse(left_op.value) % float.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                        }
                        else
                        {
                            if (checkTag(right_op))
                            {
                                num.value = (float.Parse(left_op.value) % Int32.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                            else
                            {
                                num.value = (float.Parse(left_op.value) % float.Parse(right_op.value)).ToString();
                                num.tag = TokenTypes.REAL;
                            }
                        }
                        break;
                    }
                    catch
                    {
                        syntaxMessage = "Error in line " + line_no +
                        ": Cannot divide by 0";
                        return default(numberOrString);
                    }
            }

            return num;
        }

        private numberOrString parseNumericFactor()
        {
            numberOrString valueOfNum;

            ttype = getToken();
            if (ttype == TokenTypes.LPAREN)
            {
                valueOfNum = parseNumericExpression();
                ttype = getToken();
                if (ttype == TokenTypes.RPAREN)
                    return valueOfNum;
            }
            else if (ttype == TokenTypes.NUM)
            {
                valueOfNum.tag = TokenTypes.INT;
                valueOfNum.value = token;
                return valueOfNum;
            }
            else if (ttype == TokenTypes.REALNUM)
            {
                valueOfNum.tag = TokenTypes.REAL;
                valueOfNum.value = token;
                return valueOfNum;
            }
            else if (ttype == TokenTypes.ID)
            {
                variable var;

                if (symbolTable.TryGetValue(token, out var))
                {
                    if (var.type == TokenTypes.INT)
                    {
                        valueOfNum.tag = TokenTypes.INT;
                        valueOfNum.value = var.value;
                        return valueOfNum;
                    }
                    else if (var.type == TokenTypes.REAL && symbol.type == TokenTypes.REAL)
                    {
                        valueOfNum.tag = TokenTypes.REAL;
                        valueOfNum.value = var.value;
                        return valueOfNum;
                    } else if(var.type == TokenTypes.STRING)
                    {
                        ttype = getToken();
                        if(ttype == TokenTypes.DOT)
                        {
                            ttype = getToken();
                            if (ttype == TokenTypes.LENGTH)
                            {
                                ungetToken();
                                return ParseLength(var.value);
                            } else if(ttype == TokenTypes.INDEXOF)
                            {
                                ungetToken();
                                return ParseIndexOf(var.value);
                            }
                        }
                    }
                }
            }

            return default(numberOrString);
        }

        private numberOrString parseNumericTerm()
        {
            numberOrString valueOfNum;

            numberOrString left_op;
            TokenTypes op;
            numberOrString right_op;

            ttype = getToken();
            if (ttype == TokenTypes.ID || ttype == TokenTypes.LPAREN || ttype == TokenTypes.NUM || ttype == TokenTypes.REALNUM)
            {
                ungetToken();
                left_op = parseNumericFactor();
                ttype = getToken();
                if (ttype == TokenTypes.MULT || ttype == TokenTypes.DIV || ttype == TokenTypes.MOD)
                {
                    op = (TokenTypes)ttype;
                    right_op = parseNumericTerm();
                    if (object.Equals(right_op, default(numberOrString)))
                    {
                        syntaxMessage = "Error in line " + (line_no - 1) +
                        ": There is an error when you try to set an int or double to a value";
                        actions.Add(keyActions.ERROR);
                        return default(numberOrString);
                    }
                    valueOfNum = numMath(left_op, right_op, op);
                    return valueOfNum;
                }
                else if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.PLUS ||
                  ttype == TokenTypes.MINUS || ttype == TokenTypes.RPAREN)
                {
                    ungetToken();
                    return left_op;
                }
            }

            return default(numberOrString);
        }

        private numberOrString parseNumericExpression()
        {
            numberOrString valueOfNum;

            numberOrString left_op;
            TokenTypes op;
            numberOrString right_op;

            ttype = getToken();
            if (ttype == TokenTypes.ID || ttype == TokenTypes.LPAREN || ttype == TokenTypes.NUM || ttype == TokenTypes.REALNUM)
            {
                ungetToken();
                left_op = parseNumericTerm();
                ttype = getToken();
                if (ttype == TokenTypes.PLUS || ttype == TokenTypes.MINUS)
                {
                    op = (TokenTypes)ttype;
                    right_op = parseNumericExpression();
                    if (object.Equals(right_op, default(numberOrString)))
                    {
                        syntaxMessage = "Error in line " + (line_no - 1) +
                        ": There is an error when you try to set an int or double to a value";
                        actions.Add(keyActions.ERROR);
                        return default(numberOrString);
                    }
                    valueOfNum = numMath(left_op, right_op, op);
                    return valueOfNum;
                }
                else if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.MULT || ttype == TokenTypes.MOD || 
                  ttype == TokenTypes.DIV || ttype == TokenTypes.RPAREN)
                {
                    ungetToken();
                    return left_op;
                }
            } else if (ttype == TokenTypes.MINUS)
            {
                valueOfNum = parseNumericExpression();
                numberOrString prev = valueOfNum;
                valueOfNum = numMath(valueOfNum, valueOfNum, TokenTypes.MINUS);
                valueOfNum = numMath(valueOfNum, prev, TokenTypes.MINUS);
                return valueOfNum;
            }

            return default(numberOrString);
        }

        private string ParseSubstring(string valOfString)
        {
            int firstIndex;
            int secondIndex;

            try
            {
                ttype = getToken();
                if (ttype == TokenTypes.SUBSTRING)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.LPAREN)
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.ID)
                        {
                            variable var;
                            if (symbolTable.TryGetValue(token, out var))
                            {
                                if (var.type == TokenTypes.NUM || var.type == TokenTypes.INT)
                                {
                                    firstIndex = Int32.Parse(var.value);
                                    ttype = getToken();
                                    if (ttype == TokenTypes.COMMA)
                                    {
                                        ttype = getToken();
                                        if (ttype == TokenTypes.ID)
                                        {
                                            if (symbolTable.TryGetValue(token, out var))
                                            {
                                                if (var.type == TokenTypes.NUM || var.type == TokenTypes.INT)
                                                {
                                                    secondIndex = Int32.Parse(var.value) - firstIndex;
                                                    ttype = getToken();
                                                    if (ttype == TokenTypes.RPAREN)
                                                    {
                                                        ttype = getToken();
                                                        if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.RPAREN)
                                                        {
                                                            ungetToken();
                                                            return valOfString.Substring(firstIndex, secondIndex);
                                                        }
                                                        else if (ttype == TokenTypes.PLUS)
                                                        {
                                                            return valOfString.Substring(firstIndex, secondIndex) + ParseStringExpression();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (ttype == TokenTypes.NUM)
                                        {
                                            secondIndex = Int32.Parse(token) - firstIndex;
                                            ttype = getToken();
                                            if (ttype == TokenTypes.RPAREN)
                                            {
                                                ttype = getToken();
                                                if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.RPAREN)
                                                {
                                                    ungetToken();
                                                    return valOfString.Substring(firstIndex, secondIndex);
                                                }
                                                else if (ttype == TokenTypes.PLUS)
                                                {
                                                    return valOfString.Substring(firstIndex, secondIndex) + ParseStringExpression();
                                                }
                                            }
                                        }
                                    }
                                    else if (ttype == TokenTypes.RPAREN)
                                    {
                                        ttype = getToken();
                                        if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.RPAREN)
                                        {
                                            ungetToken();
                                            return valOfString.Substring(firstIndex);
                                        }
                                        else if (ttype == TokenTypes.PLUS)
                                        {
                                            return valOfString.Substring(firstIndex) + ParseStringExpression();
                                        }
                                    }
                                }
                            }
                        }
                        else if (ttype == TokenTypes.NUM)
                        {
                            firstIndex = Int32.Parse(token);
                            ttype = getToken();
                            if (ttype == TokenTypes.COMMA)
                            {
                                ttype = getToken();
                                if (ttype == TokenTypes.ID)
                                {
                                    variable var;
                                    if (symbolTable.TryGetValue(token, out var))
                                    {
                                        if (var.type == TokenTypes.NUM || var.type == TokenTypes.INT)
                                        {
                                            secondIndex = Int32.Parse(var.value) - firstIndex;
                                            ttype = getToken();
                                            if (ttype == TokenTypes.RPAREN)
                                            {
                                                ttype = getToken();
                                                if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.RPAREN)
                                                {
                                                    ungetToken();
                                                    return valOfString.Substring(firstIndex, secondIndex);
                                                }
                                                else if (ttype == TokenTypes.PLUS || ttype == TokenTypes.RPAREN)
                                                {
                                                    return valOfString.Substring(firstIndex, secondIndex) + ParseStringExpression();
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (ttype == TokenTypes.NUM)
                                {
                                    secondIndex = Int32.Parse(token) - firstIndex;
                                    ttype = getToken();
                                    if (ttype == TokenTypes.RPAREN)
                                    {
                                        ttype = getToken();
                                        if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.RPAREN)
                                        {
                                            ungetToken();
                                            return valOfString.Substring(firstIndex, secondIndex);
                                        }
                                        else if (ttype == TokenTypes.PLUS)
                                        {
                                            return valOfString.Substring(firstIndex, secondIndex) + ParseStringExpression();
                                        }
                                    }
                                }
                            }
                            else if (ttype == TokenTypes.RPAREN)
                            {
                                ttype = getToken();
                                if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.RPAREN)
                                {
                                    ungetToken();
                                    return valOfString.Substring(firstIndex);
                                }
                                else if (ttype == TokenTypes.PLUS)
                                {
                                    return valOfString.Substring(firstIndex) + ParseStringExpression();
                                }
                            }
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                syntaxMessage = "Error in line " + (line_no - 1) +
                ": Values placed in substring out of range for string";
                return "error404notfoundit'snotworkingbzzzt";
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when trying to use substring";
            return "error404notfoundit'snotworkingbzzzt";
        } // parseNumericExpression()

        private numberOrString ParseLength(string valOfString)
        {
            numberOrString val;

            ttype = getToken();
            if(ttype == TokenTypes.LENGTH)
            {
                ttype = getToken();
                if(ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if(ttype == TokenTypes.RPAREN)
                    {
                        val.tag = TokenTypes.INT;
                        val.value = valOfString.Length.ToString();
                        return val;                       
                    }
                }
            }

            return default(numberOrString);
        }

        private numberOrString ParseIndexOf(string valOfString)
        {
            numberOrString val;
            string tVal;

            ttype = getToken();
            if(ttype == TokenTypes.INDEXOF)
            {
                ttype = getToken();
                if(ttype == TokenTypes.LPAREN)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.ID ||
                        ttype == TokenTypes.QUOTE ||
                        ttype == TokenTypes.DOUBLEQUOTE)
                    {
                        variable var;
                        if(ttype == TokenTypes.ID)
                        {
                            if(!symbolTable.TryGetValue(token, out var))
                            {
                                return default(numberOrString);
                            }
                        }

                        ungetToken();
                        tVal = ParseStringExpression();
                        ttype = getToken();
                        if(ttype == TokenTypes.RPAREN)
                        {
                            val.tag = TokenTypes.INT;
                            val.value = valOfString.IndexOf(tVal).ToString();
                            return val;                            
                        }
                    }
                                        
                }
            }

            return default(numberOrString);
        }

        private string ParseStringExpression()
        {
            string valueOfStr;

            ttype = getToken();
            if (ttype == TokenTypes.QUOTE)
            {
                quoteSwitch = true;
                ttype = getToken();
                if (ttype == TokenTypes.ID)
                {
                    valueOfStr = token;

                    ttype = getToken();

                    if (ttype == TokenTypes.QUOTE)
                    {

                        ttype = getToken();
                        if (ttype == TokenTypes.PLUS)
                        {
                            valueOfStr += ParseStringExpression();
                            return valueOfStr;
                        }
                        else if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.RPAREN ||
                            ttype == TokenTypes.GREATER || ttype == TokenTypes.GTEQ ||
                            ttype == TokenTypes.LESS || ttype == TokenTypes.LTEQ ||
                            ttype == TokenTypes.NOTEQUAL || ttype == TokenTypes.EQUALEQUAL)
                        {
                            ungetToken();
                            return valueOfStr;
                        }
                    }
                }
            }
            else if (ttype == TokenTypes.DOUBLEQUOTE)
            {
                quoteSwitch = false;
                valueOfStr = "";
                ttype = getToken();
                if (ttype == TokenTypes.PLUS)
                {
                    valueOfStr += ParseStringExpression();
                    return valueOfStr;
                }
                else if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.RPAREN)
                {
                    ungetToken();
                    return valueOfStr;
                }
            }
            else if (ttype == TokenTypes.ID)
            {
                variable var;
                if (symbolTable.TryGetValue(token, out var))
                {
                    if (var.type == TokenTypes.STRING)
                    {
                        valueOfStr = var.value;

                        ttype = getToken();
                        if (ttype == TokenTypes.PLUS)
                        {
                            valueOfStr += ParseStringExpression();
                            return valueOfStr;
                        } else if(ttype == TokenTypes.DOT)
                        {
                            ttype = getToken();
                            if(ttype == TokenTypes.SUBSTRING)
                            {
                                ungetToken();
                                valueOfStr = ParseSubstring(valueOfStr);
                                return valueOfStr;
                            }
                        }
                        else if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.RPAREN ||
                                ttype == TokenTypes.GREATER || ttype == TokenTypes.GTEQ ||
                                ttype == TokenTypes.LESS || ttype == TokenTypes.LTEQ ||
                                ttype == TokenTypes.NOTEQUAL || ttype == TokenTypes.EQUALEQUAL)
                        {
                            ungetToken();
                            return valueOfStr;
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error trying to set a string to a value";
            return "error404notfoundit'snotworkingbzzzt";
        }

        private int ParseBooleanExpression()
        {
            int valueOfBool;
            numberOrString left_op;
            TokenTypes op;
            numberOrString right_op;

            try
            {
                ttype = getToken();
                if (ttype == TokenTypes.ID || ttype == TokenTypes.NUM ||
                    ttype == TokenTypes.REALNUM || ttype == TokenTypes.TRUE ||
                    ttype == TokenTypes.FALSE || ttype == TokenTypes.QUOTE ||
                    ttype == TokenTypes.SYSTEM || ttype == TokenTypes.NOT)
                {
                    ungetToken();
                    left_op = ParsePrimary();
                    ttype = getToken();
                    if (ttype == TokenTypes.GREATER || ttype == TokenTypes.GTEQ ||
                        ttype == TokenTypes.LESS || ttype == TokenTypes.LTEQ ||
                        ttype == TokenTypes.NOTEQUAL || ttype == TokenTypes.EQUALEQUAL ||
                        ttype == TokenTypes.AND || ttype == TokenTypes.OR)
                    {
                        op = ttype;
                        ttype = getToken();
                        if (ttype == TokenTypes.ID || ttype == TokenTypes.NUM ||
                            ttype == TokenTypes.REALNUM || ttype == TokenTypes.TRUE
                            || ttype == TokenTypes.FALSE || ttype == TokenTypes.QUOTE || ttype == TokenTypes.NOT)
                        {
                            ungetToken();
                            right_op = ParsePrimary();
                            if (left_op.tag == right_op.tag ||
                                ((left_op.tag == TokenTypes.INT || left_op.tag == TokenTypes.REAL) &&
                                right_op.tag == TokenTypes.INT || right_op.tag == TokenTypes.REAL))
                            {
                                valueOfBool = Convert.ToInt32(evaluateOperands(left_op, right_op, op));
                                ttype = getToken();
                                if (ttype == TokenTypes.RPAREN || ttype == TokenTypes.SEMICOLON)
                                {
                                    ungetToken();
                                    return valueOfBool;
                                }
                                else if (ttype == TokenTypes.AND || ttype == TokenTypes.OR)
                                {
                                    if (ttype == TokenTypes.AND)
                                    {
                                        valueOfBool *= ParseBooleanExpression();
                                    }
                                    else
                                    {
                                        valueOfBool += ParseBooleanExpression();
                                    }

                                    if (valueOfBool < 2)
                                    {
                                        return valueOfBool;
                                    }
                                }
                            }
                        }
                        else if (ttype == TokenTypes.LPAREN)
                        {
                            ungetToken();
                            right_op.tag = TokenTypes.BOOLEAN;
                            right_op.value = bool.Parse(ParseBooleanExpression().ToString()).ToString();
                            if (left_op.tag == right_op.tag ||
                               ((left_op.tag == TokenTypes.INT || left_op.tag == TokenTypes.REAL) &&
                               right_op.tag == TokenTypes.INT || right_op.tag == TokenTypes.REAL))
                            {
                                valueOfBool = Convert.ToInt32(evaluateOperands(left_op, right_op, op));
                                ttype = getToken();
                                if (ttype == TokenTypes.RPAREN || ttype == TokenTypes.SEMICOLON)
                                {
                                    ungetToken();
                                    return valueOfBool;
                                }
                            }
                        }
                    }
                    else if (ttype == TokenTypes.RPAREN)
                    {
                        if (left_op.tag == TokenTypes.BOOLEAN)
                        {
                            ungetToken();
                            return Convert.ToInt32(bool.Parse(left_op.value));
                        }
                    }
                    else if (ttype == TokenTypes.SEMICOLON)
                    {
                        ungetToken();
                        return Convert.ToInt32(bool.Parse(left_op.value));
                    }
                }
                else if (ttype == TokenTypes.LPAREN)
                {
                    valueOfBool = ParseBooleanExpression();
                    ttype = getToken();
                    if (ttype == TokenTypes.RPAREN)
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.SEMICOLON || ttype == TokenTypes.RPAREN)
                        {
                            ungetToken();
                            return valueOfBool;
                        }
                        else if (ttype == TokenTypes.AND || ttype == TokenTypes.OR)
                        {
                            if (ttype == TokenTypes.AND)
                            {
                                valueOfBool *= ParseBooleanExpression();
                            }
                            else
                            {
                                valueOfBool += ParseBooleanExpression();
                            }

                            if (valueOfBool < 2)
                            {
                                return valueOfBool;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // return 2 below
            }

            return 2; //DUMMY DATA
        }

        private void ParseAssignment()
        {
            numberOrString valueOfNum; // if we are assigning ints or floats, we use this variable
            string valueOfStr;
            int valueOfBool; // 0 = false, 1 = true, anything else is error
            int errorType = 3; // 0 = numeric error, 1 = string error, 2 = boolean error, 3 = general assignment

            ttype = getToken();
            if (ttype == TokenTypes.ID)
            {
                if (!symbolTable.ContainsKey(token))
                {
                    symbol.name = token;
                    ttype = getToken();
                    if (ttype == TokenTypes.EQUAL)
                    {
                        // check to see if we need to be parsing for an integer
                        if (symbol.type == TokenTypes.INT || symbol.type == TokenTypes.REAL)
                        {
                            valueOfNum = parseNumericExpression();

                            if (object.Equals(valueOfNum, default(numberOrString)))
                            {
                                if(syntaxMessage == "")
                                    syntaxMessage = "Error in line " + (line_no) +
                                     ": There is an error when trying to set an int or double to a value";
                                actions.Add(keyActions.ERROR);
                                return;
                            }

                            ttype = getToken();
                            if (ttype == TokenTypes.SEMICOLON)
                            {
                                if (symbol.type == valueOfNum.tag || 
                                    (symbol.type == TokenTypes.REAL && valueOfNum.tag == TokenTypes.INT))
                                {
                                    symbol.value = valueOfNum.value;
                                    symbolTable.Add(symbol.name, symbol);
                                    return;
                                }else
                                {
                                    syntaxMessage = "Error in line " + (line_no - 1) +
                                    ": Type mismatch: You cannot set ints to doubles";
                                    actions.Add(keyActions.ERROR);
                                    return;
                                }

                            }
                            else
                            {
                                errorType = 0;
                            }
                        }
                        // check to see if we need to be parsing for a boolean
                        else if (symbol.type == TokenTypes.BOOLEAN)
                        {
                            valueOfBool = ParseBooleanExpression();

                            if (valueOfBool == 0 || valueOfBool == 1)
                            {
                                ttype = getToken();
                                if (ttype == TokenTypes.SEMICOLON)
                                {
                                    symbol.value = Convert.ToBoolean(valueOfBool).ToString();
                                    symbolTable.Add(symbol.name, symbol);
                                    return;
                                }
                                else
                                {
                                    errorType = 2;
                                }
                            }
                            else
                            {
                                errorType = 2;
                            }
                        }
                        // check to see if we need to be parsing for a string
                        else if (symbol.type == TokenTypes.STRING)
                        {
                            valueOfStr = ParseStringExpression();
                            if (valueOfStr != "error404notfoundit'snotworkingbzzzt")
                            {
                                ttype = getToken();
                                if (ttype == TokenTypes.SEMICOLON)
                                {
                                    symbol.value = valueOfStr;
                                    symbolTable.Add(symbol.name, symbol);
                                    return;
                                }
                                else
                                {
                                    errorType = 1;
                                }
                            }
                            else
                            {
                                errorType = 3;
                            }
                        }
                    }
                    else if (ttype == TokenTypes.SEMICOLON)
                    {
                        // default values assigned to variables if they are instantiated
                        // without a value
                        if (symbol.type == TokenTypes.INT || symbol.type == TokenTypes.REAL)
                            symbol.value = "0";
                        else if (symbol.type == TokenTypes.STRING)
                            symbol.value = "";
                        else if (symbol.type == TokenTypes.BOOLEAN)
                            symbol.value = "false";

                        symbolTable.Add(symbol.name, symbol);
                        return;
                    }
                    else
                    {
                        errorType = 3;
                        syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is something wrong with assigning a value to a variable";
                    }
                }
                else
                {
                    errorType = 3;
                    syntaxMessage = "Error in line " + (line_no - 1) +
                ": Cannot declare a variable twice in the same code";
                }
            }
            else if (ttype == TokenTypes.EQUAL)
            {
                variable var;
                if (symbol.name != null)
                {
                    if (symbolTable.TryGetValue(symbol.name, out var))
                    {
                        if (var.type == TokenTypes.INT || var.type == TokenTypes.REAL)
                        {
                            symbol.type = var.type;
                            valueOfNum = parseNumericExpression();

                            if (object.Equals(valueOfNum, default(numberOrString)))
                            {
                                syntaxMessage = "Error in line " + (line_no - 1) +
                                ": There is an error when trying to set an existing int or double to a value";
                                actions.Add(keyActions.ERROR);
                                return;
                            }

                            ttype = getToken();
                            if (ttype == TokenTypes.SEMICOLON)
                            {
                                symbol.value = valueOfNum.value;
                                symbolTable[symbol.name] = symbol;
                                return;
                            }
                            else
                            {
                                errorType = 0;
                            }
                        }
                        else if (var.type == TokenTypes.STRING)
                        {
                            symbol.type = var.type;
                            valueOfStr = ParseStringExpression();

                            if (valueOfStr != "error404notfoundit'snotworkingbzzzt")
                            {
                                ttype = getToken();
                                if (ttype == TokenTypes.SEMICOLON)
                                {
                                    symbol.value = valueOfStr;
                                    symbolTable[symbol.name] = symbol;
                                    return;
                                }
                                else
                                {
                                    errorType = 1;
                                }
                            }
                            else
                            {
                                errorType = 1;
                            }
                        }
                        else if (var.type == TokenTypes.BOOLEAN)
                        {
                            symbol.type = var.type;
                            valueOfBool = ParseBooleanExpression();

                            if (valueOfBool == 0 || valueOfBool == 1)
                            {
                                ttype = getToken();
                                if (ttype == TokenTypes.SEMICOLON)
                                {
                                    symbol.value = Convert.ToBoolean(valueOfBool).ToString();
                                    symbolTable[symbol.name] = symbol;
                                    return;
                                }
                                else
                                {
                                    errorType = 2;
                                }
                            }
                            else
                            {
                                errorType = 2;
                            }
                        }
                        else
                        {
                            errorType = 3;
                            syntaxMessage = "Error in line " + (line_no - 1) +
                            ": There is soething wrong with assigning a value to a variable";
                        }
                    }
                    else
                    {
                        errorType = 3;
                        syntaxMessage = "Error in line " + (line_no - 1) +
                        ": A variable you are trying to assign does not exist";
                    }
                }
                else
                {
                    errorType = 3;
                    syntaxMessage = "Error in line " + (line_no - 1) +
                                ": A variable you are trying to assign does not exist";
                }
            }
            else
            {
                errorType = 3;
                syntaxMessage = "Error in line " + (line_no - 1) +
                                ": There is something wrong with variable assignment";
            }

            if (errorType == 0)
                syntaxMessage = "Error in line " + (line_no - 1) +
                                ": There is an error when trying to set an int or double to a value";
            else if (errorType == 1)
                syntaxMessage = "Error in line " + (line_no - 1) +
                             ": There is an error when trying to set a string to a value";
            else if (errorType == 2)
                syntaxMessage = "Error in line " + (line_no - 1) +
                            ": There is an error when trying to set a boolean to a value";

            actions.Add(keyActions.ERROR);
            return;
        } // parseAssignment()

        private numberOrString ParsePrimary()
        {
            numberOrString returnVal;
            ttype = getToken();
            if (ttype == TokenTypes.ID)
            {
                variable var;
                if (symbolTable.TryGetValue(token, out var))
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.DOT && var.type == TokenTypes.STRING)
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.LENGTH)
                        {
                            ungetToken();
                            return ParseLength(var.value);
                        }
                        else if (ttype == TokenTypes.SUBSTRING)
                        {
                            ungetToken();
                            returnVal.tag = TokenTypes.STRING;
                            returnVal.value = ParseSubstring(var.value);
                            if (returnVal.value != "error404notfoundit'snotworkingbzzzt")
                            {
                                return returnVal;
                            }
                            else
                            {
                                return default(numberOrString);
                            }
                        }
                        else if (ttype == TokenTypes.INDEXOF)
                        {
                            ungetToken();
                            return ParseIndexOf(var.value);
                        }
                    }
                    else
                    {
                        ungetToken();
                        returnVal.tag = var.type;
                        returnVal.value = var.value;
                        return returnVal;
                    }
                }
            }
            else if (ttype == TokenTypes.NUM)
            {
                returnVal.tag = TokenTypes.INT;
                returnVal.value = token;
                return returnVal;
            }
            else if (ttype == TokenTypes.REALNUM)
            {
                returnVal.tag = TokenTypes.REAL;
                returnVal.value = token;
                return returnVal;
            }
            else if (ttype == TokenTypes.QUOTE)
            {
                ungetToken();
                returnVal.tag = TokenTypes.STRING;
                returnVal.value = ParseStringExpression();
                return returnVal;
            }
            else if (ttype == TokenTypes.TRUE || ttype == TokenTypes.FALSE)
            {
                returnVal.tag = TokenTypes.BOOLEAN;
                returnVal.value = token;
                return returnVal;
            }
            else if (ttype == TokenTypes.NOT)
            {
                returnVal.tag = TokenTypes.BOOLEAN;
                returnVal.value = (!Convert.ToBoolean(ParseBooleanExpression())).ToString();
                return returnVal;
            }
            else if (ttype == TokenTypes.SYSTEM)
            {
                ungetToken();
                return ParseSystem();
            }

            return default(numberOrString);
        } // parsePrimary()

        private bool evaluateOperands(numberOrString left_op, numberOrString right_op, TokenTypes op)
        {          
            switch (op)
            {
                case TokenTypes.GREATER:
                    if (left_op.tag == TokenTypes.NUM || left_op.tag == TokenTypes.INT ||
                        left_op.tag == TokenTypes.REALNUM || left_op.tag == TokenTypes.REAL)
                        return float.Parse(left_op.value) > float.Parse(right_op.value);                    
                    else if (left_op.tag == TokenTypes.STRING)
                        return left_op.value.CompareTo(right_op.value) > 0;
                    else if (left_op.tag == TokenTypes.BOOLEAN)
                    {
                        syntaxMessage = "Error in line " + (line_no - 1) +
                                ": There is an error when trying to evaluate a condition";
                        actions.Add(keyActions.ERROR);
                        return false;
                    }
                    break;
                case TokenTypes.GTEQ:
                    if (left_op.tag == TokenTypes.NUM || left_op.tag == TokenTypes.INT ||
                        left_op.tag == TokenTypes.REALNUM || left_op.tag == TokenTypes.REAL)
                        return float.Parse(left_op.value) >= float.Parse(right_op.value);
                    else if (left_op.tag == TokenTypes.STRING)
                        return left_op.value.CompareTo(right_op.value) >= 0;
                    else if (left_op.tag == TokenTypes.BOOLEAN)
                    {
                        syntaxMessage = "Error in line " + (line_no - 1) +
                                    ": There is an error when trying to evaluate a condition";
                        actions.Add(keyActions.ERROR);
                        return false;
                    }
                    break;
                case TokenTypes.LESS:
                    if (left_op.tag == TokenTypes.NUM || left_op.tag == TokenTypes.INT ||
                        left_op.tag == TokenTypes.REALNUM || left_op.tag == TokenTypes.REAL)
                        return float.Parse(left_op.value) < float.Parse(right_op.value);
                    else if (left_op.tag == TokenTypes.STRING)
                        return left_op.value.CompareTo(right_op.value) < 0;
                    else if (left_op.tag == TokenTypes.BOOLEAN)
                    {
                        syntaxMessage = "Error in line " + (line_no - 1) +
                                ": There is an error when trying to evaluate a condition";
                        actions.Add(keyActions.ERROR);
                        return false;
                    }
                    break;
                case TokenTypes.LTEQ:
                    if (left_op.tag == TokenTypes.NUM || left_op.tag == TokenTypes.INT ||
                        left_op.tag == TokenTypes.REALNUM || left_op.tag == TokenTypes.REAL)
                        return float.Parse(left_op.value) <= float.Parse(right_op.value);
                    else if (left_op.tag == TokenTypes.STRING)
                        return left_op.value.CompareTo(right_op.value) <= 0;
                    else if (left_op.tag == TokenTypes.BOOLEAN)
                    {
                        syntaxMessage = "Error in line " + (line_no - 1) +
                                ": There is an error when trying to evaluate a condition";
                        actions.Add(keyActions.ERROR);
                        return false;
                    }
                    break;
                case TokenTypes.EQUALEQUAL:
                    if (left_op.tag == TokenTypes.NUM || left_op.tag == TokenTypes.INT ||
                        left_op.tag == TokenTypes.REALNUM || left_op.tag == TokenTypes.REAL)
                        return float.Parse(left_op.value) == float.Parse(right_op.value);
                    else if (left_op.tag == TokenTypes.STRING)
                        return left_op.value.CompareTo(right_op.value) == 0;
                    else if (left_op.tag == TokenTypes.BOOLEAN)
                    {
                        return bool.Parse(left_op.value) == bool.Parse(right_op.value);
                    }
                    break;
                case TokenTypes.NOTEQUAL:
                    if (left_op.tag == TokenTypes.NUM || left_op.tag == TokenTypes.INT ||
                        left_op.tag == TokenTypes.REALNUM || left_op.tag == TokenTypes.REAL)
                        return float.Parse(left_op.value) != float.Parse(right_op.value);
                    else if (left_op.tag == TokenTypes.STRING)
                        return left_op.value.CompareTo(right_op.value) != 0;
                    else if (left_op.tag == TokenTypes.BOOLEAN)
                    {
                        return bool.Parse(left_op.value) != bool.Parse(right_op.value);
                    }
                    break;
                case TokenTypes.AND:
                    if(left_op.tag == TokenTypes.BOOLEAN && 
                        Convert.ToInt32(bool.Parse(left_op.value)) < 2 &&
                        Convert.ToInt32(bool.Parse(right_op.value)) < 2)
                    {
                        return (Convert.ToInt32(bool.Parse(left_op.value)) * 
                            Convert.ToInt32(bool.Parse(right_op.value))) != 0;
                    }
                    break;
                case TokenTypes.OR:
                    if(left_op.tag == TokenTypes.BOOLEAN &&
                        Convert.ToInt32(bool.Parse(left_op.value)) < 2 &&
                        Convert.ToInt32(bool.Parse(right_op.value)) < 2)
                    {
                        return (Convert.ToInt32(bool.Parse(left_op.value)) + 
                            Convert.ToInt32(bool.Parse(right_op.value))) != 0;
                    }
                    break;
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                     ": There is an error when trying to evaluate a condition";
            actions.Add(keyActions.ERROR);
            return false;
        } //evaluateOperands()

        private bool ParseCondition()
        {
            numberOrString left_op;
            TokenTypes op;
            numberOrString right_op;

            bool returnVal;

            ttype = getToken();
            if (ttype == TokenTypes.LPAREN)
            {
                ttype = getToken();
                if (ttype == TokenTypes.ID || ttype == TokenTypes.NUM || 
                    ttype == TokenTypes.REALNUM || ttype == TokenTypes.TRUE ||
                    ttype == TokenTypes.FALSE || ttype == TokenTypes.QUOTE ||
                    ttype == TokenTypes.SYSTEM)
                {
                    ungetToken();
                    left_op = ParsePrimary();
                    ttype = getToken();
                    if (ttype == TokenTypes.GREATER || ttype == TokenTypes.GTEQ ||
                        ttype == TokenTypes.LESS || ttype == TokenTypes.LTEQ ||
                        ttype == TokenTypes.NOTEQUAL || ttype == TokenTypes.EQUALEQUAL)
                    {
                        op = ttype;
                        ttype = getToken();
                        if (ttype == TokenTypes.ID || ttype == TokenTypes.NUM ||
                            ttype == TokenTypes.REALNUM || ttype == TokenTypes.TRUE
                            || ttype == TokenTypes.FALSE || ttype == TokenTypes.QUOTE)
                        {
                            ungetToken();
                            right_op = ParsePrimary();
                            if (left_op.tag == right_op.tag || 
                                ((left_op.tag == TokenTypes.INT || left_op.tag == TokenTypes.REAL) &&
                                right_op.tag == TokenTypes.INT || right_op.tag == TokenTypes.REAL))
                            {
                                returnVal = evaluateOperands(left_op, right_op, op);
                                ttype = getToken();
                                if (ttype == TokenTypes.RPAREN)
                                {
                                    ttype = getToken();
                                    if (ttype == TokenTypes.LBRACE)
                                    {
                                        ungetToken();
                                        return returnVal;
                                    }
                                    else
                                        ungetToken();
                                }
                            }
                        }
                    }
                    else if (ttype == TokenTypes.RPAREN)
                    {
                        if (left_op.tag == TokenTypes.BOOLEAN)
                        {
                            ttype = getToken();
                            if (ttype == TokenTypes.LBRACE)
                            {
                                ungetToken();
                                return bool.Parse(left_op.value);
                            }
                            else
                                ungetToken();
                        }
                    }
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                    ": There is an error with the condition. Remember to compare equivalent types and start the body with a \"{\"";
            actions.Add(keyActions.ERROR);
            return false;
        } // ParseCondition()

        private void ClearBody()
        {
            int numOfLBrace = 0;
            ttype = getToken();
            if (ttype != TokenTypes.LBRACE)
            {
                syntaxMessage = "Error in line " + (line_no - 1) +
                     ": There is an error when trying to read code after condition";
                actions.Add(keyActions.ERROR);
                return;
            }
            else
            {
                numOfLBrace++;
                while (ttype != TokenTypes.EOS && numOfLBrace != 0)
                {
                    ttype = getToken();

                    if (ttype == TokenTypes.LBRACE)
                        numOfLBrace++;
                    else if (ttype == TokenTypes.RBRACE)
                        numOfLBrace--;
                }
            }

            if (ttype == TokenTypes.EOS || numOfLBrace != 0)
            {
                syntaxMessage = "Error in line " + (line_no - 1) +
                         ": There is an error when trying to read code after condition";
                actions.Add(keyActions.ERROR);
            }
        } // ClearBody()

        private void ReadBody(string concat)
        {
            int numOfLBrace = 0; // increment for amount of LBRACE, decrement for RBRACE
            int prevLineNo = line_no;
            int numQuotes = 0;

            ttype = getToken();
            loopCode[loopCode.Count - 1] += token + " ";
            // expecting a left brace; otherwise, there is no body
            if (ttype != TokenTypes.LBRACE)
            {
                syntaxMessage = "Error in line " + (line_no - 1) +
                            ": There is an error when trying to read code after condition";
                actions.Add(keyActions.ERROR);
                return;
            }
            else
            {
                numOfLBrace++;
                while (ttype != TokenTypes.EOS && numOfLBrace != 0 && ttype != TokenTypes.ERROR)
                {
                    ttype = getToken();

                    // this means we entered a new line number and therefore need to add a new line to the loop code
                    if(prevLineNo != line_no)
                    {
                        prevLineNo = line_no;
                        loopCode[loopCode.Count - 1] += "\n";
                    }
                    
                    // since reading in a quote leads to a "quote switching" protocol in getToken(),
                    // we take care of it in this section of the code
                    if(ttype == TokenTypes.QUOTE && numQuotes < 1)
                    {
                        quoteSwitch = true;
                        numQuotes++;
                    } else if(ttype == TokenTypes.QUOTE && numQuotes >= 1)
                    {
                        numQuotes = 0;
                    }

                    if (quoteSwitch || numQuotes != 0)
                    {
                        loopCode[loopCode.Count - 1] += token;
                    }
                    else
                    {
                        loopCode[loopCode.Count - 1] += token + " ";
                    }

                    if (ttype == TokenTypes.LBRACE)
                        numOfLBrace++;
                    else if (ttype == TokenTypes.RBRACE)
                        numOfLBrace--;
                }

                if (concat != "")
                {
                    loopCode[loopCode.Count - 1] = loopCode[loopCode.Count - 1].TrimEnd(' ');
                    loopCode[loopCode.Count - 1] = loopCode[loopCode.Count - 1].TrimEnd('}');
                    loopCode[loopCode.Count - 1] += concat + " ";
                    loopCode[loopCode.Count - 1] += "}\n";
                }
            }

            if (ttype == TokenTypes.EOS || ttype == TokenTypes.ERROR || numOfLBrace != 0)
            {
                syntaxMessage = "Error in line " + (line_no - 1) +
                    ": There is an error when trying to read code after condition";
                actions.Add(keyActions.ERROR);
            }
        } //ReadBody()

        private void ClearCondition()
        {
            int numOfLParen = 0;
            ttype = getToken();
            if(ttype == TokenTypes.LPAREN)
            {
                numOfLParen++;
                while(ttype != TokenTypes.EOS && numOfLParen != 0)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.RPAREN)
                        numOfLParen--;
                }
            }
            else
            {
                ungetToken();
            }

            if (ttype == TokenTypes.EOS || numOfLParen != 0)
            {
                syntaxMessage = "Error in line " + (line_no - 1) +
                    ": There is an error when formatting the conditional statement";
                actions.Add(keyActions.ERROR);
            }
        } // ClearCondition()

        private void ClearUntilNewLine()
        {
            int currLineNo = line_no;
            while(currLineNo == line_no)
            {
                ttype = getToken();
            }
            ungetToken();
        }

        private void ReadCondition()
        {
            int numOfLParen = 0;
            int numQuotes = 0;

            ttype = getToken();
            loopCode[loopCode.Count - 1] += token + " ";
            if (ttype == TokenTypes.LPAREN)
            {
                numOfLParen++;
                while (ttype != TokenTypes.EOS && numOfLParen != 0)
                {
                    ttype = getToken();

                    if (ttype == TokenTypes.QUOTE && numQuotes < 1)
                    {
                        quoteSwitch = true;
                        numQuotes++;
                    }
                    else if (ttype == TokenTypes.QUOTE && numQuotes >= 1)
                    {
                        numQuotes = 0;
                    }

                    if (quoteSwitch || numQuotes != 0) 
                    {
                        loopCode[loopCode.Count - 1] += token;
                    }
                    else
                    {
                        loopCode[loopCode.Count - 1] += token + " ";
                    }
                   
                    if (ttype == TokenTypes.RPAREN)
                        numOfLParen--;
                }
            }
            else
            {
                ungetToken();
            }

            if (ttype == TokenTypes.EOS || numOfLParen != 0)
            {
                syntaxMessage = "Error in line " + (line_no - 1) +
                ": There is an error when formatting the conditional statement";
                actions.Add(keyActions.ERROR);
            }
        } // ReadCondition()

        private string ReadForCondition()
        {
            string concat = "";
            ttype = getToken();
            if(ttype == TokenTypes.LPAREN)
            {
                ttype = getToken();
                if (ttype == TokenTypes.INT || ttype == TokenTypes.REAL ||
                  ttype == TokenTypes.STRING || ttype == TokenTypes.BOOLEAN ||
                  ttype == TokenTypes.ID)
                {
                    symbol = default(variable); // resets the symbol
                    symbol.type = (TokenTypes)ttype;
                    if (ttype == TokenTypes.ID)
                    {
                        symbol.name = token;
                    }
                    else
                    {
                        if (newAssignments.Count > 0)
                            newAssignments[newAssignments.Count - 1]++;
                    }
                    ParseAssignment();
                }

                // dont need to do ttype = getToken() because ParseAssignment already gets the SEMICOLON

                if(ttype == TokenTypes.SEMICOLON)
                {
                    ttype = getToken();
                    loopCode[loopCode.Count - 1] += "(" + token + " ";
                    while(ttype != TokenTypes.SEMICOLON && ttype != TokenTypes.EOS)
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.SEMICOLON)
                            loopCode[loopCode.Count - 1] += ")";
                        else
                            loopCode[loopCode.Count - 1] += token + " ";
                    }

                    if(ttype == TokenTypes.EOS)
                    {
                        syntaxMessage = "Error in line " + (line_no - 1) +
                            ": There is an error in the formatting of the for-loop";
                        actions.Add(keyActions.ERROR);
                        return "";
                    } else if(ttype == TokenTypes.SEMICOLON)
                    {
                        ttype = getToken();
                        concat += token + " ";
                        while(ttype != TokenTypes.RPAREN && ttype != TokenTypes.EOS)
                        {
                            ttype = getToken();
                            if (ttype != TokenTypes.RPAREN)
                                concat += token + " ";
                            else
                                concat += ";";
                        }

                        if(ttype == TokenTypes.EOS)
                        {
                            syntaxMessage = "Error in line " + (line_no - 1) +
                                ": There is an error in the formatting of the for-loop";
                            actions.Add(keyActions.ERROR);
                            return "";
                        } else if(ttype == TokenTypes.RPAREN)
                        {
                            return concat;
                        }
                    } 
                }
            }

            syntaxMessage = "Error in line " + (line_no - 1) +
                    ": There is an error in the formatting of the for-loop";
            actions.Add(keyActions.ERROR);
            return "";
        } // ReadForCondition()

        public List<keyActions> Parse(string passedCode)
        {

            loopCode.Clear(); // clear the loop code stack
            newAssignments.Clear(); // clear the scope stack
            symbolTable.Clear(); // clear the symbol table
            syntaxMessage = ""; // clear syntax message

            errorCount = 0; // reset error count
            activeToken = 0; // reset active token
            
            outputValue = new List<string>(); // resets output values
            waitTimes = new List<float>(); // reset wait times
                  
            code = passedCode;
            ttype = 0;
            keepParsing = true;

            // acts like a stack.
            // determines what to do with RBRACE, if
            // we are evaluating a loop, then loop. Else,
            // do other stuff.
            List<StatementTypes> statementTag = new List<StatementTypes>();
            List<int> isInfinite = new List<int>();

            int numBraces = 0;
            
            while ((code.Length > 0 || ttype != TokenTypes.EOS) && !actions.Contains(keyActions.ERROR) && !isInfinite.Contains(100))
            {
                if (!keepParsing)
                {                  
                    ClearCondition();
                    ClearBody();
                    keepParsing = true;
                }

                ttype = getToken();
                
                if (ttype == TokenTypes.SYSTEM)
                {
                    ungetToken();                   
                    ParseSystem();
                }
                else if (ttype == TokenTypes.INT || ttype == TokenTypes.REAL ||
                  ttype == TokenTypes.STRING || ttype == TokenTypes.BOOLEAN ||
                  ttype == TokenTypes.ID)
                {
                    symbol = default(variable); // resets the symbol
                    symbol.type = (TokenTypes)ttype;
                    if (ttype == TokenTypes.ID)
                    {
                        symbol.name = token;
                    }
                    else
                    {
                        if (newAssignments.Count > 0)
                            newAssignments[newAssignments.Count - 1]++;
                    }
                    
                    ParseAssignment();
                }
                else if (ttype == TokenTypes.WHILE)
                {
                    statementTag.Add(StatementTypes.ISWHILE);
                    isInfinite.Add(0);
                    loopLineNo = line_no;
                    loopCode.Add("");
                    ReadCondition();
                    ReadBody("");
                    code = loopCode[loopCode.Count - 1] + code;
                    line_no = loopLineNo;                                     
                } else if(ttype == TokenTypes.FOR) // turn the for loop into a while loop for easier parsing
                {
                    statementTag.Add(StatementTypes.ISFOR);
                    loopCode.Add("");
                    newAssignments.Add(0);
                    loopLineNo = line_no;
                    string incrementer = ReadForCondition();
                    ReadBody(incrementer);
                    code = loopCode[loopCode.Count - 1] + code;
                    statementTag.Add(StatementTypes.ISWHILE);
                    isInfinite.Add(0);
                    line_no = loopLineNo;                
                }
                else if(ttype == TokenTypes.LPAREN && statementTag.LastOrDefault() == StatementTypes.ISWHILE)
                {
                    ungetToken();
                    keepParsing = ParseCondition();
                    isInfinite[isInfinite.Count - 1]++; // if we are evaluating the condition more than 100 times,
                                  // then we have an infinite loop
                    if (!keepParsing)
                    {
                        isInfinite.RemoveAt(isInfinite.Count - 1); // condition evaluates to false so reset counter
                        loopCode.RemoveAt(loopCode.Count - 1);
                        statementTag.RemoveAt(statementTag.Count - 1);
                        if (newAssignments.Count > 0 && statementTag.Count > 0 && statementTag.Last() == StatementTypes.ISFOR)
                        {
                            for (int i = 0; i < newAssignments[newAssignments.Count - 1]; i++)
                            {
                                symbolTable.Remove(symbolTable.Keys.Last());
                            }
                            newAssignments.RemoveAt(newAssignments.Count - 1);
                            statementTag.RemoveAt(statementTag.Count - 1);
                        }
                    }
                }
                else if (ttype == TokenTypes.IF)
                {
                    statementTag.Add(StatementTypes.HASIF);
                    keepParsing = ParseCondition();
                    if (keepParsing)
                        statementTag.Add(StatementTypes.EXECUTEIF);
                }
                else if (ttype == TokenTypes.ELSE)
                {
                    if ((statementTag.Count <= 0) || statementTag.Last() == StatementTypes.ISEXECUTING
                        || (statementTag.Last() != StatementTypes.HASIF && 
                        statementTag.Last() != StatementTypes.EXECUTEIF))
                    {
                        syntaxMessage = "Error in line " + (line_no - 1) +
                ": There needs to be an \"if\" before an \"else\"";
                        actions.Add(keyActions.ERROR);
                    }
                    else
                    {
                        ttype = getToken();
                        if (ttype == TokenTypes.IF && statementTag.Last() != StatementTypes.EXECUTEIF)
                        {
                            keepParsing = ParseCondition();
                            if (keepParsing)
                                statementTag.Add(StatementTypes.EXECUTEIF);
                        }
                        else if (ttype == TokenTypes.IF && statementTag.Last() == StatementTypes.EXECUTEIF)
                        {
                            keepParsing = false;
                        }
                        else if (ttype == TokenTypes.LBRACE && statementTag.Last() != StatementTypes.EXECUTEIF)
                        {
                            ungetToken();
                        } else if(ttype == TokenTypes.LBRACE && statementTag.Last() == StatementTypes.EXECUTEIF)
                        {
                            ungetToken();
                            keepParsing = false;
                        }
                        else 
                        {
                            syntaxMessage = "Error in line " + (line_no - 1) +
                                ": Missing a \"{\" after an \"else\"";
                            actions.Add(keyActions.ERROR);
                        }
                    }
                }
                else if (ttype == TokenTypes.LBRACE && statementTag.Count >= 1)
                {
                    numBraces++;
                    newAssignments.Add(0);

                    if(statementTag.Last() == StatementTypes.EXECUTEIF)
                        statementTag.Add(StatementTypes.ISEXECUTING);                
                }
                else if (ttype == TokenTypes.RBRACE && statementTag.Count >= 1)
                {
                    numBraces--;
                    if (newAssignments.Count > 0) {
                        for (int i = 0; i < newAssignments[newAssignments.Count-1]; i++)
                        {
                            symbolTable.Remove(symbolTable.Keys.Last());
                        }
                        newAssignments.RemoveAt(newAssignments.Count - 1);
                    }
            
                    ttype = getToken(); // check if there is an else after the if
                    if (ttype == TokenTypes.ELSE)
                    {
                        ungetToken();

                        if (statementTag.Last() == StatementTypes.ISEXECUTING)
                            statementTag.RemoveAt(statementTag.Count - 1);

                        if (statementTag.Last() != StatementTypes.HASIF &&
                            statementTag.Last() != StatementTypes.ISWHILE && statementTag.Last() != StatementTypes.EXECUTEIF)
                        {
                            syntaxMessage = "Error in line " + (line_no - 1) +
                                ": There needs to be an \"if\" before an \"else\"";
                            actions.Add(keyActions.ERROR);
                        }
                        else if (statementTag.Last() == StatementTypes.HASIF)
                        {
                            statementTag.RemoveAt(statementTag.Count - 1);
                        }                        
                    }
                    else
                    {
                        if(ttype != TokenTypes.EOS)    
                            ungetToken();

                        if (statementTag.Count > 0)
                        {
                            if (statementTag.Last() == StatementTypes.ISEXECUTING ||
                                statementTag.Last() == StatementTypes.EXECUTEIF ||
                                statementTag.Last() == StatementTypes.HASIF)
                            {
                                if (statementTag.Last() == StatementTypes.ISEXECUTING)
                                    statementTag.RemoveAt(statementTag.Count - 1);

                                if (statementTag.Last() == StatementTypes.EXECUTEIF)
                                    statementTag.RemoveAt(statementTag.Count - 1);

                                statementTag.RemoveAt(statementTag.Count - 1);
                            }
                        }

                        if (statementTag.Count > 0)
                        {
                            if (statementTag.Last() == StatementTypes.ISWHILE)
                            {
                                ungetToken();
                                line_no = loopLineNo;
                                ttype = getToken();
                                code = token + code;
                                code = loopCode[loopCode.Count - 1] + code;
                            }
                        }
                        
                    } 
                }
                else if(ttype == TokenTypes.DOUBLESLASH)
                {
                    ClearUntilNewLine();
                }
                else if(ttype != TokenTypes.EOS)
                {
                    syntaxMessage = "Error in line " + (line_no - 1) +
                            ": Weird characters in code that do nothing";
                    actions.Add(keyActions.ERROR);
                }                
            }

            if (isInfinite.Contains(100))
            {
                syntaxMessage = "You have an infinite loop";
                actions.Add(keyActions.INFINITELOOP);
            }

            if (numBraces != 0 && !actions.Contains(keyActions.ERROR)
                && !actions.Contains(keyActions.INFINITELOOP))
            {
                syntaxMessage = "Error in line " + (line_no - 1) +
                        ": There needs to be an equal amount of \"{\" and \"}\"";
                actions.Add(keyActions.ERROR);
            }

            if (actions.Contains(keyActions.ERROR))
            {
                actions.Add(keyActions.TURNBLACK);
                errorCount++;
            }
            else if (!(actions.Contains(keyActions.TURNBLUE) || actions.Contains(keyActions.TURNGREEN) || actions.Contains(keyActions.TURNRED)))
            {
                actions.Add(keyActions.TURNBLACK);
                errorCount = 0;
            }
            else { 
                errorCount = 0;
            }

            if(errorCount > 2)
            {
                actions.Add(keyActions.ERRORMSG);
            }

            symbolTable.Clear(); // clear symbol table 

            return actions;
        }

        private void ungetToken()
        {
            activeToken = 1;
        } //Parse()

        private void skipSpace()
        {
            char c;

            try
            {
                c = code[0];
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }

            line_no += Convert.ToInt32(c == '\n');
            while (code.Length > 0 && char.IsWhiteSpace(c))
            {
                code = code.Substring(1);
                try
                {
                    c = code[0];
                }
                catch (IndexOutOfRangeException)
                {
                    return;
                }
                line_no += Convert.ToInt32(c == '\n');
            }
        } // skipSpace()

        private TokenTypes isKeyword(string s)
        {
            for (int i = 0; i <= (int)TokenTypes.GREEN; i++)
                if (reserved[i] == s)
                    return (TokenTypes)i;
            return 0;
        } // isKeyword()

        private TokenTypes scan_number()
        {
            char c;

            c = code[0];
            if (char.IsDigit(c))
            {
                // First collect leading digits before dot
                // 0 is a NUM by itself
                if (c == '0')
                {
                    token += c;
                    code = code.Substring(1);
                }
                else
                {
                    while (char.IsDigit(c))
                    {
                        token += c;
                        code = code.Substring(1);
                        c = code[0];
                    }
                }

                // Check if leading digits are integer part of a REALNUM
                if (code[0] == '.')
                {
                    code = code.Substring(1);
                    c = code[0];

                    if (char.IsDigit(c))
                    {
                        token += '.';
                        while (char.IsDigit(c))
                        {
                            token += c;
                            code = code.Substring(1);
                            c = code[0];
                        }
                        return TokenTypes.REALNUM;
                    }
                    else
                    {
                        return TokenTypes.ERROR;
                    }
                }
                else
                {
                    return TokenTypes.NUM;
                }
            }
            else
                return TokenTypes.ERROR;
        } // scan_numbers()

        private TokenTypes scan_id_or_keyword()
        {
            TokenTypes ttype;
            char c;

            c = code[0];
            if (char.IsLetter(c))
            {
                while (char.IsLetterOrDigit(c))
                {
                    token += c;                   
                    try
                    {
                        code = code.Substring(1);
                        c = code[0];
                    }
                    catch
                    {
                        break;
                    }
                }

                ttype = isKeyword(token);
                if (ttype == 0)
                    ttype = TokenTypes.ID;
                return ttype;
            }
            else
            {
                return TokenTypes.ERROR;
            }
        } // scan_id_or_keyword()

        // called if we are scanning a string literal between QUOTES
        private TokenTypes scan_characters()
        {
            char c;

            try
            {
                c = code[0];
                while (c != '"')
                {
                    token += c;
                    try
                    {
                        code = code.Substring(1);
                        c = code[0];
                    }
                    catch
                    {
                        break;
                    }
                }

                quoteSwitch = false;
                return TokenTypes.ID;
            }
            catch
            {
                return TokenTypes.ERROR;
            }
        } // scan_characters()

        private TokenTypes getToken()
        {
            char c;

            if (activeToken == 1)
            {
                activeToken = 0;
                return ttype;
            }

            if(!quoteSwitch)
                skipSpace();
            token = "";
            try
            {
                c = code[0];
            }
            catch (IndexOutOfRangeException)
            {
                return TokenTypes.EOS;
            }

            if (!quoteSwitch) {
                string before; // for instances where we need to backtrack
                try
                {
                    switch (c)
                    {
                        case '.':
                            if (char.IsDigit(code[1]))
                            {
                                code = code.PadLeft(code.Length + 1, '0');
                                return scan_number();
                            }
                            else {
                                code = code.Substring(1);
                                token = ".";
                                return TokenTypes.DOT;
                            }
                        case '\'':
                            code = code.Substring(1);
                            token = "\'";
                            return TokenTypes.APOSTROPHE;
                        case '"':
                            code = code.Substring(1);
                            token = "\"";
                            c = code[0];
                            if(c == '"')
                            {
                                code = code.Substring(1);
                                token += "\"";
                                return TokenTypes.DOUBLEQUOTE;
                            }
                            return TokenTypes.QUOTE;
                        case '+':
                            code = code.Substring(1);
                            token = "+";
                            return TokenTypes.PLUS;
                        case '-':
                            token = "-";
                            code = code.Substring(1);
                            c = code[0];
                            // checks to see if next token is a digit and
                            // previous type is not a number (which means we need minus)
                            if ((char.IsDigit(c) || c == '.') && (ttype != TokenTypes.NUM 
                                && ttype != TokenTypes.REALNUM && ttype != TokenTypes.RPAREN))
                            {
                                if(c == '.')
                                {
                                    code = code.PadLeft(code.Length + 1, '0');
                                }
                                return scan_number();
                            }
                            else
                            {
                                return TokenTypes.MINUS;
                            }
                        case '/':
                            token = "/";
                            code = code.Substring(1);
                            c = code[0];
                            if(c == '/')
                            {
                                token += "/";
                                code = code.Substring(1);
                                return TokenTypes.DOUBLESLASH;
                            }
                            return TokenTypes.DIV;
                        case '*':
                            token = "*";
                            code = code.Substring(1);
                            return TokenTypes.MULT;
                        case '%':
                            token = "%";
                            code = code.Substring(1);
                            return TokenTypes.MOD;
                        case '=':
                            token = "=";
                            code = code.Substring(1);
                            c = code[0];
                            if (c == '=')
                            {
                                token += "=";
                                code = code.Substring(1);
                                return TokenTypes.EQUALEQUAL;
                            }
                            return TokenTypes.EQUAL;
                        case ':':
                            token = ":";
                            code = code.Substring(1);
                            return TokenTypes.COLON;
                        case ',':
                            token = ",";
                            code = code.Substring(1);
                            return TokenTypes.COMMA;
                        case ';':
                            token = ";";
                            code = code.Substring(1);
                            return TokenTypes.SEMICOLON;
                        case '[':
                            token = "[";
                            code = code.Substring(1);
                            return TokenTypes.LBRAC;
                        case ']':
                            token = "]";
                            code = code.Substring(1);
                            return TokenTypes.RBRAC;
                        case '(':
                            token = "(";
                            code = code.Substring(1);
                            return TokenTypes.LPAREN;
                        case ')':
                            token = ")";
                            code = code.Substring(1);
                            return TokenTypes.RPAREN;
                        case '{':
                            token = "{";
                            code = code.Substring(1);
                            return TokenTypes.LBRACE;
                        case '}':
                            token = "}";
                            code = code.Substring(1);
                            return TokenTypes.RBRACE;
                        case '&':
                            before = code;
                            code = code.Substring(1);
                            c = code[0];
                            if (c == '&')
                            {
                                token = "&&";
                                code = code.Substring(1);
                                return TokenTypes.AND;
                            }
                            code = before;
                            return scan_id_or_keyword();
                        case '|':
                            before = code;
                            code = code.Substring(1);
                            c = code[0];
                            if (c == '|')
                            {
                                token = "||";
                                code = code.Substring(1);
                                return TokenTypes.OR;
                            }
                            code = before;
                            return scan_id_or_keyword();
                        case '!':
                            token = "!";
                            code = code.Substring(1);
                            c = code[0];
                            if (c == '=')
                            {
                                token += "=";
                                code = code.Substring(1);
                                return TokenTypes.NOTEQUAL;
                            }
                            return TokenTypes.NOT;
                        case '<':
                            token = "<";
                            code = code.Substring(1);
                            c = code[0];
                            if (c == '=')
                            {
                                token += "=";
                                code = code.Substring(1);
                                return TokenTypes.LTEQ;
                            }
                            else
                            {
                                return TokenTypes.LESS;
                            }
                        case '>':
                            token = ">";
                            code = code.Substring(1);
                            c = code[0];
                            if (c == '=')
                            {
                                token += "=";
                                code = code.Substring(1);
                                return TokenTypes.GTEQ;
                            }
                            else
                            {
                                return TokenTypes.GREATER;
                            }
                        default:
                            if (char.IsDigit(c))
                            {
                                return scan_number();
                            }
                            else if (char.IsLetter(c))
                            {
                                return scan_id_or_keyword();
                            }
                            else
                            {
                                return TokenTypes.ERROR;
                            }
                    }
                }
                catch
                {
                    return TokenTypes.ERROR;
                }                
            }
            else
            {
                return scan_characters();
            }
        } //getToken()


        #region HelperFunctions
        public List<KeyValuePair<string, string>> FixConditional(string passedCode)
        {
            code = passedCode;
            ttype = 0;  // reset token type
            activeToken = 0; // reset active token
            List<KeyValuePair<string, string>> returnValue = new List<KeyValuePair<string, string>>();
            bool hasIf = false;

            ttype = getToken();
            while(ttype != TokenTypes.EOS)
            {
                if (ttype == TokenTypes.IF)
                {
                    ttype = getToken();
                    if (ttype == TokenTypes.LPAREN)
                    {
                        hasIf = true;
                    }
                }

                if (ttype == TokenTypes.RPAREN && hasIf)
                {
                    hasIf = false;
                }

                if (ttype == TokenTypes.SYSTEM)
                {
                    ungetToken();
                    KeyValuePair<string, string> result = ParseSystemForConditionals(hasIf);
                    if (!result.Equals(default(KeyValuePair<string, string>)))
                    {
                        returnValue.Add(result);
                    }
                }

                ttype = getToken();
            }

            if(returnValue.Count >= 1 && !actions.Contains(keyActions.DISTANCE))
            {
                actions.Add(keyActions.DISTANCE);
            }

            return returnValue;
        } // FindDistances()

        // parses for System.distance(ID); or System.distance(ID)
        private KeyValuePair<string, string> ParseSystemForConditionals(bool hasIf)
        {
            string buildToken = "";

            ttype = getToken();
            if (ttype == TokenTypes.SYSTEM)
            {
                buildToken += token;
                ttype = getToken();
                if (ttype == TokenTypes.DOT)
                {
                    buildToken += token;
                    ttype = getToken();
                    if (ttype == TokenTypes.DISTANCE)
                    {
                        buildToken += token;
                        ttype = getToken();
                        if (ttype == TokenTypes.LPAREN)
                        {
                            buildToken += token;
                            ttype = getToken();
                            if (ttype == TokenTypes.QUOTE)
                            {   
                                ungetToken();
                                string returnString = ParseStringExpression();

                                if (returnString != "error404notfoundit'snotworkingbzzzt")
                                {
                                    buildToken += "\"" + returnString + "\"";
                                    ttype = getToken();
                                    if (ttype == TokenTypes.RPAREN)
                                    {
                                        buildToken += token;
                                        ttype = getToken();
                                        if (ttype == TokenTypes.SEMICOLON)
                                        {
                                            buildToken += token;
                                            return new KeyValuePair<string, string>(buildToken, returnString);
                                        }
                                        else
                                        {
                                            ungetToken();
                                            return new KeyValuePair<string, string>(buildToken, returnString);
                                        }
                                    }
                                }
                                
                                
                            }
                        }
                    } else if(ttype == TokenTypes.BODY && hasIf) // check if we are looking for body and we are in if statement
                    {
                        buildToken += token;
                        ttype = getToken();
                        if(ttype == TokenTypes.LPAREN)
                        {
                            buildToken += token;
                            ttype = getToken();
                            if(ttype == TokenTypes.COLOR)
                            {
                                buildToken += token;
                                ttype = getToken();
                                if(ttype == TokenTypes.DOT)
                                {
                                    buildToken += token;
                                    ttype = getToken();
                                    if(ttype == TokenTypes.GREEN || ttype == TokenTypes.RED || 
                                        ttype == TokenTypes.BLUE || ttype == TokenTypes.BLACK)
                                    {
                                        buildToken += token;
                                        string color = token;
                                        ttype = getToken();
                                        if(ttype == TokenTypes.RPAREN)
                                        {
                                            buildToken += token;
                                            return new KeyValuePair<string, string>(buildToken, color);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return default(KeyValuePair<string, string>);
        } // ParseDistance()
        #endregion
    }
}

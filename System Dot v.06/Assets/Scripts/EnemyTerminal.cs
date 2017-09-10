using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ParserAlgo;

public class EnemyTerminal : MonoBehaviour
{
    public GameObject restrictor; // game object that the player must inhabit in order for object to be hacked (i.e. kernels)

    public static int globalTerminalMode = 0;
    public static bool madeChanges = false;
    public int localTerminalMode = 0;
    public static bool active = false;

    public int numberOfLines = 1;
    public bool[] numOfLegacy = new bool[5];
    public string[] terminalString = new string[5];
    public string[] originalString = new string[5];
    public string classHeader;
    public string parameters = "";
    public float moveSpeed = .025f; 

    public GUIStyle terminalStyle;
    public GameObject terminalPointerDestination;
    public GameObject terminalWindow;
    public GameObject bit;

    [Header("Chest Sprites")]
    public Sprite chestSpriteClosed;
    public Sprite chestSpriteOpen;

    [Header("Colored Sprites")]
    public Sprite redSlime;
    public Sprite blueSlime;
    public Sprite greenSlime;
    public Sprite blackSlime;

    [Header("Centipede Body Sprites")]
    public Sprite redBody;
    public Sprite blueBody;
    public Sprite greenBody;
    public Sprite blackBody;
    public Sprite head;    

    private bool showTerminal = false;

    public List<keyActions> actions = new List<keyActions>();
    public List<string> outputVal = new List<string>();
    Parser parse = new Parser();

    private int numOfSyntaxErrors;

    public AudioSource openTerminal;

    /*LOGGER INFORMATION */
    public bool isPerfect = true;

    // Use this for initialization
    void Start()
    {
        Transform[] trs = GameObject.Find("Main HUD").GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "Terminal Window")
            {
                terminalWindow = t.gameObject;
            }
        }

        checkTerminalString();
        StartCoroutine(evaluateActions());       
    }

    // Update is called once per frame
    void Update()
    {
        if(actions.Contains(keyActions.MOVELEFT) || actions.Contains(keyActions.MOVERIGHT))
            StartCoroutine(evaluateActions());

        if (globalTerminalMode < 2)
        {
            localTerminalMode = 0;
        }

        if (localTerminalMode == 2)
        {
            this.GetComponent<LineRenderer>().enabled = true;
            this.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0.4f, 10));
            if(terminalPointerDestination != null)
                this.GetComponent<LineRenderer>().SetPosition(1,
                    new Vector3(terminalPointerDestination.transform.position.x - this.transform.position.x,
                    terminalPointerDestination.transform.position.y - this.transform.position.y,
                    10));
           // openTerminal.Play();
        }
        else
        {
            this.GetComponent<LineRenderer>().enabled = false;
        }

        ShowTerminalWindow();
      
    }

    public void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                                    gameObject.GetComponent<SpriteRenderer>().material.color.b, .5f);
    }

    public void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                                    gameObject.GetComponent<SpriteRenderer>().material.color.b, 1);      
    }

    public void OnMouseDown()
    {
        // will not click game object if player has not cleared up the restrictor
        if (restrictor == null || (restrictor != null && Kernel.kernelNameCurrentlyIn == restrictor.name))
        {
            // will not click game object if mouse is clicking UI
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                for (int i = 0; i < terminalString.Length; i++)
                    originalString[i] = terminalString[i];

                for (int i = 0; i < numberOfLines; i++)
                {
                    if (terminalString[i] != "")
                    {
                        terminalWindow.transform.GetChild(i + 2).gameObject.GetComponent<InputField>().text = terminalString[i];
                    }
                }

                // open terminal
                if (globalTerminalMode <= 1)
                {
                    GameObject.Find("Sound Controller").GetComponent<SoundController>().play("terminal");
                    EnemyTerminal[] enemies = FindObjectsOfType<EnemyTerminal>();
                    foreach (EnemyTerminal e in enemies)
                        e.localTerminalMode = 0;
                    globalTerminalMode = 2;
                    localTerminalMode = 2;

                    showTerminal = true;
                    /* LOGGER INFORMATION */
                    PlayerStats.numOfEdits++;                   
                }
                else
                {
                    globalTerminalMode = 0;
                    localTerminalMode = 0;
                }
            }
        }
    }

    public void checkTerminalString()
    {
        actions.Clear();
        string passedInString = "";

        // add parameter code to passed-in string
        passedInString += parameters;

        // check if a variabull is with us
        if (terminalWindow.transform.parent.GetComponent<TerminalWindowUI>().variabullRef.activeSelf) {
            passedInString += terminalWindow.transform.parent.GetComponent<TerminalWindowUI>().variaCode.GetComponent<Text>().text;
        }

        foreach(string s in terminalString)
        {
            passedInString += " \n" + s;
        }

        // if there looks like there is a conditional
        if (passedInString.Contains("if("))
        {
            passedInString =  ReplaceSystemsInConditional(passedInString);
        }
        actions = parse.Parse(passedInString);
        outputVal = parse.outputValue;
    }

    #region Condtional Game-End Functions
    public string ReplaceSystemsInConditional(string passedInString)
    {
        passedInString = ReplaceSystemDistance(passedInString);
        return passedInString;
    }

    // finds instances of System.distance(ID) and replaces them with the float number of that actual distance 
    // we do this in enemy terminal because Parser does not contain Unity functions
    public string ReplaceSystemDistance(string passedInString)
    {       
        if (this.transform.childCount >= 1 && this.transform.FindChild("DistanceChild"))
        {
            DistanceLine distanceChild = this.transform.FindChild("DistanceChild").gameObject.GetComponent<DistanceLine>();
            for (int i = 0; i < terminalString.Count(); i++)
            {
                if (terminalString[i].Contains("System.distance("))
                {
                    for (int j = 0; j < distanceChild.parameterNames.Count; j++)
                    {
                        if (distanceChild.target[j] == null)
                        {
                            string replaceString = terminalString[i].Replace("System.distance(" + distanceChild.parameterNames[j] + ")", "0");
                            passedInString = passedInString.Replace(terminalString[i], replaceString);
                        }
                        else
                        {
                            string replaceString = terminalString[i].Replace(distanceChild.parameterNames[j], "\"" + distanceChild.target[j].name + "\"");
                            passedInString = passedInString.Replace(terminalString[i], replaceString);
                        }
                    }
                }
            }

            List<KeyValuePair<string, string>> replaceValues = parse.FixConditional(passedInString);
            foreach (KeyValuePair<string, string> pair in replaceValues)
            {
                passedInString = passedInString.Replace(pair.Key, FindDistanceToObject(pair.Value));
            }
        }
           

        return passedInString;
    }

    private string FindDistanceToObject(string objectName)
    {
        GameObject target = GameObject.Find(objectName);
        float distance = Vector2.Distance(this.gameObject.transform.position, target.transform.position);
        if(target == null)
        {
            return "0";
        }

        if (distance < this.gameObject.transform.FindChild("DistanceChild").gameObject.GetComponent<DistanceLine>().maxDistance)
        {
            return Vector2.Distance(this.gameObject.transform.position, target.transform.position).ToString();
        }
        
        return "a";
        
    }
    #endregion  

    public IEnumerator evaluateActions()
    {
        int waitCount = 0;

        if (GameObject.Find("error message"))
        {
            if (numOfSyntaxErrors > 2)
            {
                GameObject.Find("error message").GetComponent<Text>().text = parse.syntaxMessage;
            }
            else
            {
                GameObject.Find("error message").GetComponent<Text>().text = "";
            }
        }

        if (!actions.Contains(keyActions.ERROR)) {
            foreach (keyActions action in actions)
            {
                switch (action)
                {
                    case keyActions.TURNBLUE:
                        if (this.GetComponent<HurtPlayerOnContact>())
                        {
                            this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.BLUE;
                        }
                        if (this.gameObject.tag == "Enemy")
                        {
                            this.GetComponent<SpriteRenderer>().sprite = blueSlime;
                        }
                        else if (this.gameObject.tag == "Centipede Body")
                        {
                            this.GetComponent<SpriteRenderer>().sprite = blueBody;
                        }
                        numOfSyntaxErrors = 0;
                        break;
                    case keyActions.TURNRED:
                        if (this.GetComponent<HurtPlayerOnContact>())
                        {
                            this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.RED;
                        }
                        if (this.gameObject.tag == "Enemy")
                        {
                            this.GetComponent<SpriteRenderer>().sprite = redSlime;
                        }
                        else if (this.gameObject.tag == "Centipede Body")
                        {
                            this.GetComponent<SpriteRenderer>().sprite = redBody;
                        }
                        numOfSyntaxErrors = 0;
                        break;
                    case keyActions.TURNGREEN:
                        if (this.GetComponent<HurtPlayerOnContact>())
                        {
                            this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.GREEN;
                        }
                        if (this.gameObject.tag == "Enemy")
                        {
                            this.GetComponent<SpriteRenderer>().sprite = greenSlime;
                        }
                        else if (this.gameObject.tag == "Centipede Body")
                        {
                            this.GetComponent<SpriteRenderer>().sprite = greenBody;
                        }
                        numOfSyntaxErrors = 0;
                        break;
                    case keyActions.TURNBLACK:
                        if (this.GetComponent<HurtPlayerOnContact>())
                        {
                            this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.BLACK;
                        }
                        if (this.gameObject.tag == "Enemy")
                        {
                            this.GetComponent<SpriteRenderer>().sprite = blackSlime;
                        }
                        else if (this.gameObject.tag == "Centipede Body" && this.gameObject.name != "Centipede Head")
                        {
                            this.GetComponent<SpriteRenderer>().sprite = blackBody;
                        }
                        else if (this.gameObject.tag == "Centipede Body" && this.gameObject.name == "Centipede Head")
                        {
                            this.GetComponent<SpriteRenderer>().sprite = head;
                        }
                        break;
                    case keyActions.TURNLETTER:
                        if (this.GetComponent<WordManipulator>())
                        {
                            foreach (string output in outputVal)
                            {
                                if (output.Contains("Body: "))
                                {
                                    if (this.GetComponent<WordManipulator>() != null)
                                    {
                                        this.GetComponent<WordManipulator>().constructWord = true;
                                        this.GetComponent<WordManipulator>().wordToSet = output.Substring(6, output.Length - 6);
                                    }
                                }
                            }
                        }
                        break;
                    case keyActions.CLOSE:
                        if (this.gameObject.tag == "Chest")
                        {
                            this.GetComponent<SpriteRenderer>().sprite = chestSpriteClosed;
                            numOfSyntaxErrors = 0;
                        }
                        break;
                    case keyActions.OPEN:
                        if (this.gameObject.tag == "Chest")
                        {
                            this.GetComponent<SpriteRenderer>().sprite = chestSpriteOpen;
                            yield return new WaitForSeconds(1);
                            this.GetComponent<ChestBits>().chestOpen();

                            if (this.gameObject.name == "TutorialChest")
                            {
                                GameObject.Find("Intellisense").GetComponent<IntelliSenseTest>().SetDialogue("unlockChest");
                            }

                            numOfSyntaxErrors = 0;
                        }
                        break;
                    case keyActions.MOVELEFT:
                        for (int i = 0; i < actions.Count; i++)
                        {
                            if (actions[i] == keyActions.MOVERIGHT)
                            {
                                actions.RemoveAt(i);
                                i--;
                            }
                            else if (actions[i] == keyActions.MOVELEFT)
                            {
                                break;
                            }
                        }
                        this.gameObject.transform.position += Vector3.left * moveSpeed;
                      //  this.gameObject.transform.Translate(Vector3.left * Time.deltaTime * 3);

                        if (this.gameObject.name == "TutorialPlatform1" && PlayerStats.highestCheckpoint == 3)
                        {
                            GameObject.Find("Intellisense").GetComponent<IntelliSenseTest>().SetDialogue("movePlatform2");
                            this.gameObject.name = "TutorialPlatform2";
                        }

                        numOfSyntaxErrors = 0;
                        break;
                    case keyActions.MOVERIGHT:
                        for (int i = 0; i < actions.Count; i++)
                        {
                            if (actions[i] == keyActions.MOVELEFT)
                            {
                                actions.RemoveAt(i);
                                i--;
                            }
                            else if (actions[i] == keyActions.MOVERIGHT)
                            {
                                break;
                            }
                        }
                        this.gameObject.transform.position += Vector3.right * moveSpeed;
                       // this.gameObject.transform.Translate(Vector3.right * Time.deltaTime * 3);

                        if (this.gameObject.name == "TutorialPlatform" && PlayerStats.highestCheckpoint == 3)
                        {
                            GameObject.Find("Intellisense").GetComponent<IntelliSenseTest>().SetDialogue("movePlatform");
                            this.gameObject.name = "TutorialPlatform1";
                        }

                        numOfSyntaxErrors = 0;
                        break;
                    case keyActions.WAIT:
                        yield return new WaitForSeconds(parse.waitTimes[waitCount]);
                        waitCount++;
                        numOfSyntaxErrors = 0;
                        break;
                    case keyActions.SMASH:
                        if (this.GetComponent<Smash>() == null)
                        {
                            this.gameObject.AddComponent<Smash>();
                        }
                        if (this.GetComponent<Rigidbody2D>() != null)
                        {
                            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        }
                        numOfSyntaxErrors = 0;
                        break;
                    case keyActions.GRAVITYON:
                        if (this.GetComponent<Rigidbody2D>() != null)
                        {
                            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        }
                        break;
                    case keyActions.GRAVITYOFF:
                        if (this.GetComponent<Rigidbody2D>() != null)
                        {
                            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                        }
                        break;
                    case keyActions.ACTIVATE:
                        foreach (string output in outputVal)
                        {
                            if (output.Contains("Activate: "))
                            {
                                if (this.GetComponent<Activator>() != null)
                                {
                                    this.GetComponent<Activator>().power = float.Parse(output.Substring(10, output.Length - 10));
                                }
                                else if (this.GetComponent<WordManipulator>() != null)
                                {
                                    this.GetComponent<WordManipulator>().activatedIndex = float.Parse(output.Substring(10, output.Length - 10));
                                }
                            }
                        }
                        break;
                    case keyActions.ROTATE:
                        foreach (string output in outputVal)
                        {
                            if (output.Contains("Rotate: "))
                            {
                                if (this.GetComponent<Rotater>() != null)
                                {
                                    float frepitition = float.NaN;
                                    string repetition = string.Empty;
                                    try
                                    {
                                        frepitition = float.Parse(output.Substring(8, output.Length - 8));
                                    } catch(Exception e)
                                    {
                                        repetition = output.Substring(8, output.Length - 8);
                                    }

                                    if (repetition == string.Empty)
                                    {
                                        this.GetComponent<Rotater>().pause = false;
                                        this.GetComponent<Rotater>().goLeft = frepitition >= 0;
                                        this.GetComponent<Rotater>().maxRotation += frepitition * 360;
                                    }
                                    else
                                    {
                                        switch (repetition)
                                        {
                                            case "LEFT":
                                                this.GetComponent<Rotater>().RotateToSpecificPosition(90);
                                                break;
                                            case "RIGHT":
                                                this.GetComponent<Rotater>().RotateToSpecificPosition(270);
                                                break;
                                            case "UP":
                                                this.GetComponent<Rotater>().RotateToSpecificPosition(0);
                                                break;
                                            case "DOWN":
                                                this.GetComponent<Rotater>().RotateToSpecificPosition(180);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case keyActions.DELETE: 
                        if (this.GetComponent<WordManipulator>() != null)
                        {
                            List<string> listToDelete = new List<string>();
                            foreach (string output in outputVal)
                            {
                                if (output.Contains("Delete: "))
                                {                                    
                                    string wordToDelete = output.Substring(8, output.Length - 8);
                                    listToDelete.Add(wordToDelete);
                                }
                            }
                            if (listToDelete.Count != 0) {
                                this.GetComponent<WordManipulator>().whatToDelete = listToDelete;
                            }
                        }           
                        break;
                    case keyActions.OUTPUT:
                        foreach (string output in outputVal)
                        {
                            if (output.Contains("Output: "))
                            {
                                if (this.GetComponent<Gate>())
                                {
                                    // for gates, there can only be one System.output
                                    if (actions.Count(x => x == keyActions.OUTPUT) == 1)
                                    {
                                        string variableInfo = output.Substring(8, output.Length - 8);
                                        // variable info comes in [0] name [1] type [2] value
                                        if (variableInfo.Split(Environment.NewLine.ToCharArray())[1] == "BOOLEAN")
                                        {
                                            this.GetComponent<Gate>().ActivateDestination(Convert.ToBoolean(variableInfo.Split(Environment.NewLine.ToCharArray())[2].ToLower()));
                                        }
                                    }
                                    else
                                    {
                                        actions.Add(keyActions.ERROR);
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            } else{
            // ERROR (turn object black if error and halt all current actions)
                ObjectError();
                numOfSyntaxErrors++;
            }        
    }

    public void ObjectError()
    {
        if (this.GetComponent<HurtPlayerOnContact>())
        {
            this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.BLACK;
        }
        if (this.gameObject.tag == "Enemy")
        {
            this.GetComponent<SpriteRenderer>().sprite = blackSlime;
        }
        else if (this.gameObject.tag == "Centipede Body" && this.gameObject.name != "Centipede Head")
        {
            this.GetComponent<SpriteRenderer>().sprite = blackBody;
        }
        else if (this.gameObject.tag == "Centipede Body" && this.gameObject.name == "Centipede Head")
        {
            this.GetComponent<SpriteRenderer>().sprite = head;
        }

        // stop rotating
        if (this.GetComponent<Rotater>() != null)
        {
            this.GetComponent<Rotater>().pause = true;
        }

        // close door
        if (this.GetComponent<Gate>())
        {
            this.GetComponent<Gate>().ActivateDestination(false);
        }
    }

    public void ShowTerminalWindow()
    {
        // terminal window pops up
        if (localTerminalMode == 2)
        {
            terminalWindow.SetActive(true);

            if (showTerminal)
            {
                // when opening terminal window, start the cursor at the end of the first line
                InputField line1 = terminalWindow.transform.FindChild("line 1").GetComponent<InputField>();                
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(line1.gameObject, null);
                StartCoroutine(MoveTextEnd_NextFrame(line1));
                showTerminal = false;
            }

            switch (numberOfLines)
            {
                case 1:
                    terminalWindow.transform.FindChild("line 3").gameObject.SetActive(false);
                    terminalWindow.transform.FindChild("line 4").gameObject.SetActive(false);
                    terminalWindow.transform.FindChild("line 5").gameObject.SetActive(false);
                    terminalWindow.transform.FindChild("closing bracket").gameObject.SetActive(false);
                    terminalWindow.transform.FindChild("line 2").gameObject.GetComponent<InputField>().text = "}";
                    terminalWindow.transform.FindChild("line 2").gameObject.GetComponent<InputField>().readOnly = true;
                    break;
                case 2:
                    terminalWindow.transform.FindChild("line 3").gameObject.SetActive(true);
                    terminalWindow.transform.FindChild("line 4").gameObject.SetActive(false);
                    terminalWindow.transform.FindChild("line 5").gameObject.SetActive(false);
                    terminalWindow.transform.FindChild("closing bracket").gameObject.SetActive(false);
                    terminalWindow.transform.FindChild("line 3").gameObject.GetComponent<InputField>().text = "}";
                    terminalWindow.transform.FindChild("line 2").gameObject.GetComponent<InputField>().readOnly = false;
                    terminalWindow.transform.FindChild("line 3").gameObject.GetComponent<InputField>().readOnly = true;
                    break;
                case 3:
                    terminalWindow.transform.FindChild("line 3").gameObject.SetActive(true);
                    terminalWindow.transform.FindChild("line 4").gameObject.SetActive(true);
                    terminalWindow.transform.FindChild("line 5").gameObject.SetActive(false);
                    terminalWindow.transform.FindChild("closing bracket").gameObject.SetActive(false);
                    terminalWindow.transform.FindChild("line 4").gameObject.GetComponent<InputField>().text = "}";
                    terminalWindow.transform.FindChild("line 2").gameObject.GetComponent<InputField>().readOnly = false;
                    terminalWindow.transform.FindChild("line 3").gameObject.GetComponent<InputField>().readOnly = false;
                    terminalWindow.transform.FindChild("line 4").gameObject.GetComponent<InputField>().readOnly = true;
                    break;
                case 4:
                    terminalWindow.transform.FindChild("line 3").gameObject.SetActive(true);
                    terminalWindow.transform.FindChild("line 4").gameObject.SetActive(true);
                    terminalWindow.transform.FindChild("line 5").gameObject.SetActive(true);
                    terminalWindow.transform.FindChild("closing bracket").gameObject.SetActive(false);
                    terminalWindow.transform.FindChild("line 5").gameObject.GetComponent<InputField>().text = "}";
                    terminalWindow.transform.FindChild("line 2").gameObject.GetComponent<InputField>().readOnly = false;
                    terminalWindow.transform.FindChild("line 3").gameObject.GetComponent<InputField>().readOnly = false;
                    terminalWindow.transform.FindChild("line 4").gameObject.GetComponent<InputField>().readOnly = false;
                    terminalWindow.transform.FindChild("line 5").gameObject.GetComponent<InputField>().readOnly = true;
                    break;
                case 5:
                    terminalWindow.transform.FindChild("line 3").gameObject.SetActive(true);
                    terminalWindow.transform.FindChild("line 4").gameObject.SetActive(true);
                    terminalWindow.transform.FindChild("line 5").gameObject.SetActive(true);
                    terminalWindow.transform.FindChild("line 2").gameObject.GetComponent<InputField>().readOnly = false;
                    terminalWindow.transform.FindChild("line 3").gameObject.GetComponent<InputField>().readOnly = false;
                    terminalWindow.transform.FindChild("line 4").gameObject.GetComponent<InputField>().readOnly = false;
                    terminalWindow.transform.FindChild("line 5").gameObject.GetComponent<InputField>().readOnly = false;
                    terminalWindow.transform.FindChild("closing bracket").gameObject.SetActive(true);
                    break;
            }

            for (int i = 0; i < numberOfLines; i++)
            {
                terminalString[i] = terminalWindow.transform.GetChild(i + 2).gameObject.GetComponent<InputField>().text;
            }

            for (int i = 0; i < numOfLegacy.Length; i++)
            {
                string access = "line " + (i + 1);
                terminalWindow.transform.FindChild(access).gameObject.GetComponent<InputField>().readOnly = numOfLegacy[i];
                if (numOfLegacy[i])
                {
                    terminalWindow.transform.FindChild(access).gameObject.GetComponent<InputField>().textComponent.color = Color.red;
                }
                else
                {
                    terminalWindow.transform.FindChild(access).gameObject.GetComponent<InputField>().textComponent.color = Color.white;
                }
            }

            terminalWindow.transform.FindChild("class header").gameObject.GetComponent<InputField>().text = classHeader;

            // check for changes in terminal
            for (int i = 0; i < terminalString.Length; i++)
            {
                if (terminalString[i] != originalString[i])
                {
                    madeChanges = true;
                }
            }
                  
        }
        else if (globalTerminalMode != 2)
        {

            terminalWindow.SetActive(false);

            // delete text from input fields  
            terminalWindow.transform.FindChild("line 1").gameObject.GetComponent<InputField>().text = "";
            terminalWindow.transform.FindChild("line 2").gameObject.GetComponent<InputField>().text = "";
            terminalWindow.transform.FindChild("line 3").gameObject.GetComponent<InputField>().text = "";
            terminalWindow.transform.FindChild("line 4").gameObject.GetComponent<InputField>().text = "";
            terminalWindow.transform.FindChild("line 5").gameObject.GetComponent<InputField>().text = "";
        }
    }

    IEnumerator MoveTextEnd_NextFrame(InputField lineNO)
    {
        yield return 0; // Skip the first frame in which this is called.
        lineNO.MoveTextEnd(false); // Do this during the next frame.
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConditionalGenerator : MonoBehaviour {

    public Text conditionalStatement;

    
    public int min, max;

    public int[] statements;
    public string[] variableIdentifiers;
    public char[] chars;
    public string[] strings;
    public string[] methods;

    private bool answer;
    
    System.Random rng;

	// Use this for initialization
	void Start () {
        conditionalStatement = GetComponent<Text>();
        rng = new System.Random();

        generateStatement();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void generateStatement()
    {
        int roll;
        roll = rng.Next(0, statements.Length);
        switch(roll)
        {
            case 0:
                generateEasyBasicStatement();
                break;
            case 1:
                generateEasyVariableStatement();
                break;
            default:
                break;

        }

        
    }

    public string getOp(int rand1, int rand2)
    {
        string op = "";
        int rand;
        rand = rng.Next(0, 5);

        switch(rand)
        {
            case 0:
                op = ">";
                answer = (rand1 > rand2);
                break;
            case 1:
                op = ">=";
                answer = (rand1 >= rand2);
                break;
            case 2:
                op = "==";
                answer = (rand1 == rand2);
                break;
            case 3:
                op = "<=";
                answer = (rand1 <= rand2);
                break;
            case 4:
                op = "<";
                answer = (rand1 < rand2);
                break;
            default:
                break;

        }
        Debug.Log(answer);
        return op;
    }

    public void generateEasyBasicStatement()
    {
        int arithmetic = rng.Next(0,2);

        if (arithmetic == 1)
        {
            int rand1, rand2;
            string op;
            rand1 = rng.Next(min, max);
            if (rng.Next(0, 100) >= 80)
            {
                rand2 = rand1;
            }
            else
            {
                rand2 = rng.Next(min, max);
            }
            op = getOp(rand1, rand2);

            conditionalStatement.text = rand1 + " " + op + " " + rand2;
        }
        else
        {
            int rand, x1, x2, y1, y2;
            int xAnswer = 0;
            int yAnswer = 0;
            string arithStringX = "";
            string arithStringY = "";

            x1 = rng.Next(min, max);
            x2 = rng.Next(min, max);
            y1 = rng.Next(min, max);
            y2 = rng.Next(min, max);

            rand = rng.Next(0, 4);

            switch (rand)
            {
                case 0:
                    xAnswer = x1 + x2;
                    arithStringX = x1 + " + " + x2;
                    break;
                case 1:
                    xAnswer = x1 - x2;
                    arithStringX = x1 + " - " + x2;
                    break;
                case 2:
                    xAnswer = x1 * x2;
                    arithStringX = x1 + " * " + x2;
                    break;
                case 3:
                    if (x2 == 0)
                    {
                        x2 = 1;
                    }
                    xAnswer = x1 / x2;
                    arithStringX = x1 + " / " + x2;
                    break;
                default:
                    break;

            }

            rand = rng.Next(0, 4);

            switch (rand)
            {
                case 0:
                    yAnswer = y1 + y2;
                    arithStringY = y1 + " + " + y2;
                    break;
                case 1:
                    yAnswer = y1 - y2;
                    arithStringY = y1 + " - " + y2;
                    break;
                case 2:
                    yAnswer = y1 * y2;
                    arithStringY = y1 + " * " + y2;
                    break;
                case 3:
                    if(y2 == 0)
                    {
                        y2 = 1;
                    }
                    yAnswer = y1 / y2;
                    arithStringY = y1 + " / " + y2;
                    break;
                default:
                    break;

            }


            conditionalStatement.text = "("+ arithStringX + ") " + getOp(xAnswer, yAnswer) + " (" + arithStringY + ")";

            
        }
    }

    public void generateEasyVariableStatement()
    {
        int arithmetic = rng.Next(0, 2);
        string var1, var2;
        var1 = variableIdentifiers[rng.Next(0, variableIdentifiers.Length)];
        var2 = variableIdentifiers[rng.Next(0, variableIdentifiers.Length)];

        while (var1.Equals(var2))
        {
            var2 = variableIdentifiers[rng.Next(0, variableIdentifiers.Length)];
        }

        if(arithmetic == 1)
        {
            int rand1, rand2;
            string op;
            rand1 = rng.Next(min, max);
            if (rng.Next(0, 100) >= 80)
            {
                rand2 = rand1;
            }
            else
            {
                rand2 = rng.Next(min, max);
            }
            op = getOp(rand1, rand2);

           

            conditionalStatement.text = "int " + var1 + " = " + rand1 + "\nint " + var2 + " = " + rand2 + "\n" + var1 + " " + op + " " + var2;
        }
        else
        {
            int rand, x1, x2, y1, y2;
            int xAnswer = 0;
            int yAnswer = 0;
            string arithStringX = "";
            string arithStringY = "";

            x1 = rng.Next(min, max);
            x2 = rng.Next(min, max);
            y1 = rng.Next(min, max);
            y2 = rng.Next(min, max);

            rand = rng.Next(0, 4);

            switch (rand)
            {
                case 0:
                    xAnswer = x1 + x2;
                    arithStringX = x1 + " + " + x2;
                    break;
                case 1:
                    xAnswer = x1 - x2;
                    arithStringX = x1 + " - " + x2;
                    break;
                case 2:
                    xAnswer = x1 * x2;
                    arithStringX = x1 + " * " + x2;
                    break;
                case 3:
                    if (x2 == 0)
                    {
                        x2 = 1;
                    }
                    xAnswer = x1 / x2;
                    arithStringX = x1 + " / " + x2;
                    break;
                default:
                    break;

            }

            rand = rng.Next(0, 4);

            switch (rand)
            {
                case 0:
                    yAnswer = y1 + y2;
                    arithStringY = y1 + " + " + y2;
                    break;
                case 1:
                    yAnswer = y1 - y2;
                    arithStringY = y1 + " - " + y2;
                    break;
                case 2:
                    yAnswer = y1 * y2;
                    arithStringY = y1 + " * " + y2;
                    break;
                case 3:
                    if (y2 == 0)
                    {
                        y2 = 1;
                    }
                    yAnswer = y1 / y2;
                    arithStringY = y1 + " / " + y2;
                    break;
                default:
                    break;

            }

            conditionalStatement.text = "int " + var1 + " = " + "(" + arithStringX + ")" + "\nint " + var2 + " = " + "(" + arithStringY + ")" + "\n" + var1 + " " + getOp(xAnswer, yAnswer) + " " + var2;
        }
    }

  


    public bool getAnswer()
    {
        return answer;
    }
}

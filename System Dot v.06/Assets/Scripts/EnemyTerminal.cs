using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using ParserAlgo;



public class EnemyTerminal : MonoBehaviour
{

    public static int globalTerminalMode = 0;
    public int localTerminalMode = 0;
    public static bool active = false;

    public int numberOfLines = 1;

    public bool[] numOfLegacy = new bool[5];
    public string[] terminalString = new string[5];
    public string classHeader;

    public GUIStyle terminalStyle;
    public GameObject terminalPointerDestination;
    public GameObject terminalWindow;

    public Sprite chestSpriteClosed;
    public Sprite chestSpriteOpen;
    public Sprite redSlime;
    public Sprite blueSlime;
    public Sprite greenSlime;
    public Sprite blackSlime;

    private bool showTerminal = false;
    public string colorString;

    public List<keyActions> actions = new List<keyActions>();
    Parser parse = new Parser();

    private int numOfSyntaxErrors;

    // Use this for initialization
    void Start()
    {
        terminalWindow = GameObject.Find("Terminal Window");

        if (this.tag == "Enemy")
        {
            if (this.GetComponent<HurtPlayerOnContact>().enemyState == HurtEnemyOnContact.colorState.RED)
                colorString = "RED";
            else if (this.GetComponent<HurtPlayerOnContact>().enemyState == HurtEnemyOnContact.colorState.BLUE)
                colorString = "BLUE";
            else if (this.GetComponent<HurtPlayerOnContact>().enemyState == HurtEnemyOnContact.colorState.GREEN)
                colorString = "GREEN";

            //terminalString = "public class VBot{\n     " + writtenString + " \n}";
        }
        else if (this.tag == "Chest")
        {
            //terminalString = "public class Chest{\n     System.close(); \n}";
        } else if (this.tag == "movingPlatform")
        {
            //terminalString = "public class MovingPlatform{\n   " + writtenString + " \n}";
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
            //checkTerminalString();
            // evaluateActions();
            this.GetComponent<LineRenderer>().enabled = true;
            this.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0.4f, 10));
            if(terminalPointerDestination != null)
                this.GetComponent<LineRenderer>().SetPosition(1, new Vector3(terminalPointerDestination.transform.position.x - this.transform.position.x,
                                                        terminalPointerDestination.transform.position.y - this.transform.position.y,
                                                        10));
        }
        else
        {
            this.GetComponent<LineRenderer>().enabled = false;
        }     


        // terminal window pops up
        if(localTerminalMode == 2)
        {
            terminalWindow.SetActive(true);


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
        }
        else if(globalTerminalMode != 2)
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

    void OnMouseOver()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                                    gameObject.GetComponent<SpriteRenderer>().material.color.b, .5f);
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                                    gameObject.GetComponent<SpriteRenderer>().material.color.b, 1);
      
    }

    void OnMouseDown()
    {
        for (int i = 0; i < numberOfLines; i++)
        {
            if (terminalString[i] != "")
            {
                terminalWindow.transform.GetChild(i + 2).gameObject.GetComponent<InputField>().text = terminalString[i];
            }
        }

        if (globalTerminalMode <=1)
        {
            EnemyTerminal[] enemies = FindObjectsOfType<EnemyTerminal>();
            foreach (EnemyTerminal e in enemies)
                e.localTerminalMode = 0;
            globalTerminalMode = 2;
            localTerminalMode = 2;
        }
        else
        {
            globalTerminalMode = 0;
            localTerminalMode = 0;
        }
    }

    public void checkTerminalString()
    {
        actions.Clear();
        string passedInString = "";
        foreach(string s in terminalString)
        {
            passedInString += " " + s;
        }

        actions = parse.Parse(passedInString);
    }

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

        foreach (keyActions action in actions)
        {
            switch (action)
            {
                case keyActions.TURNBLUE:
                    if (this.gameObject.tag == "Enemy")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.BLUE;
                        this.GetComponent<SpriteRenderer>().sprite = blueSlime;
                        numOfSyntaxErrors = 0;
                    }
                    break;
                case keyActions.TURNRED:
                    if (this.gameObject.tag == "Enemy")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.RED;
                        this.GetComponent<SpriteRenderer>().sprite = redSlime;
                        numOfSyntaxErrors = 0;
                    }
                    break;
                case keyActions.TURNGREEN:
                    if (this.gameObject.tag == "Enemy")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.GREEN;
                        this.GetComponent<SpriteRenderer>().sprite = greenSlime;
                        numOfSyntaxErrors = 0;
                    }
                    break;
                case keyActions.TURNBLACK:
                    if (this.gameObject.tag == "Enemy")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.BLACK;
                        this.GetComponent<SpriteRenderer>().sprite = blackSlime;
                    }
                    break;
                case keyActions.OPEN:
                    if(this.gameObject.tag == "Chest")
                    {
                        this.GetComponent<SpriteRenderer>().sprite = chestSpriteClosed;
                        numOfSyntaxErrors = 0;
                    }
                    break;
                case keyActions.CLOSE:
                    if(this.gameObject.tag == "Chest")
                    {
                        this.GetComponent<SpriteRenderer>().sprite = chestSpriteOpen;
                        numOfSyntaxErrors = 0;
                    }
                    break;
                case keyActions.MOVELEFT:
                    this.gameObject.transform.Translate(Vector3.left * Time.deltaTime * 3);
                    numOfSyntaxErrors = 0;
                    break;
                case keyActions.MOVERIGHT:
                    this.gameObject.transform.Translate(Vector3.right * Time.deltaTime * 3);
                    numOfSyntaxErrors = 0;
                    break;
                case keyActions.WAIT:
                    yield return new WaitForSeconds(parse.waitTimes[waitCount]);
                    waitCount++;
                    numOfSyntaxErrors = 0;
                    break;
                case keyActions.ERROR:
                    numOfSyntaxErrors++;
                    break;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using ParserAlgo;



public class EnemyTerminal : MonoBehaviour
{

    public static int globalTerminalMode = 0;
    public static bool madeChanges = false;
    public int localTerminalMode = 0;
    public static bool active = false;

    public int numberOfLines = 1;
    public bool[] numOfLegacy = new bool[5];
    public string[] terminalString = new string[5];
    public string[] originalString = new string[5];
    public string classHeader;

    public GUIStyle terminalStyle;
    public GameObject terminalPointerDestination;
    public GameObject terminalWindow;
    public GameObject bit;

    [Header("Chest Sprites")]
    public Sprite chestSpriteClosed;
    public Sprite chestSpriteOpen;

    [Header("VBot Sprites")]
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
    private bool tutorialCheck = false;

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
        foreach(Transform t in trs)
        {
            if(t.name == "Terminal Window")
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

                /* LOGGER INFORMATION */
                PlayerStats.numOfEdits++;                

                // for comment tutorial in level 1
                if (this.gameObject.name.Contains("Comment"))
                {
                    GameObject.Find("Intellisense").GetComponent<IntelliSenseTest>().SetDialogue("discoverComments");
                }
            }
            else
            {
                globalTerminalMode = 0;
                localTerminalMode = 0;
            }
        }
    }

    public void checkTerminalString()
    {
        actions.Clear();
        string passedInString = "";
        foreach(string s in terminalString)
        {
            passedInString += " \n" + s;
        }

        actions = parse.Parse(passedInString);
        outputVal = parse.outputValue;
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
                    } else if (this.gameObject.tag == "Centipede Body")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.BLUE;
                        this.GetComponent<SpriteRenderer>().sprite = blueBody;
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
                    else if (this.gameObject.tag == "Centipede Body")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.RED;
                        this.GetComponent<SpriteRenderer>().sprite = redBody;
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
                    else if (this.gameObject.tag == "Centipede Body")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.GREEN;
                        this.GetComponent<SpriteRenderer>().sprite = greenBody;
                        numOfSyntaxErrors = 0;
                    }
                    break;
                case keyActions.TURNBLACK:
                    if (this.gameObject.tag == "Enemy")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.BLACK;
                        this.GetComponent<SpriteRenderer>().sprite = blackSlime;
                    }
                    else if (this.gameObject.tag == "Centipede Body" && this.gameObject.name != "Centipede Head")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.BLACK;
                        this.GetComponent<SpriteRenderer>().sprite = blackBody;
                    } else if (this.gameObject.tag == "Centipede Body" && this.gameObject.name == "Centipede Head")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.BLACK;
                        this.GetComponent<SpriteRenderer>().sprite = head;
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
                    for(int i = 0; i < actions.Count; i++)
                    {
                        if(actions[i] == keyActions.MOVERIGHT)
                        {
                            actions.RemoveAt(i);
                            i--;
                        } else if(actions[i] == keyActions.MOVELEFT)
                        {
                            break;
                        }
                    }

                    this.gameObject.transform.Translate(Vector3.left * Time.deltaTime * 3);

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
                    this.gameObject.transform.Translate(Vector3.right * Time.deltaTime * 3);

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
                    if (this.GetComponent<Rigidbody2D>() != null) {
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
                    foreach(string output in outputVal)
                    {
                        if (output.Contains("Activate: "))
                        {
                            if(this.GetComponent<Activator>() != null)
                            {
                                this.GetComponent<Activator>().power = float.Parse(output.Substring(10, output.Length - 10));
                            }
                        }
                    }
                    break;
                case keyActions.ERROR:
                    numOfSyntaxErrors++;
                    break;
            }
        }
    }

    public void ShowTerminalWindow()
    {
        // terminal window pops up
        if (localTerminalMode == 2)
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

            // check for changes in terminal
            for (int i = 0; i < terminalString.Length; i++)
            {
                if (terminalString[i] != originalString[i])
                {
                    madeChanges = true;
                }
            }

            // for black Vbot tutorial
            if(this.gameObject.name == "TutorialEnemy2" && !tutorialCheck)
            {
                if(terminalString[0] == "System.body(Color.GREEN);")
                {
                    tutorialCheck = true;
                    GameObject.Find("Intellisense").GetComponent<IntelliSenseTest>().SetDialogue("codeFixed");
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
}

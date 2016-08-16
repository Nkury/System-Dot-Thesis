using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using ParserAlgo;


public class EnemyTerminal : MonoBehaviour
{

    public static int globalTerminalMode = 0;
    public int localTerminalMode = 0;
    public static bool active = false;

    public string writtenString;
    public string terminalString;

    public GUIStyle terminalStyle;
    public GameObject terminalPointerDestination;

    public Sprite chestSpriteClosed;
    public Sprite chestSpriteOpen;
    public Sprite redSlime;
    public Sprite blueSlime;
    public Sprite greenSlime;
    public Sprite blackSlime;

    private bool showTerminal = false;
    public string colorString;

    List<keyActions> actions = new List<keyActions>();
    Parser parse = new Parser();

    // Use this for initialization
    void Start()
    {
        if (this.tag == "Enemy")
        {
            if (this.GetComponent<HurtPlayerOnContact>().enemyState == HurtEnemyOnContact.colorState.RED)
                colorString = "RED";
            else if (this.GetComponent<HurtPlayerOnContact>().enemyState == HurtEnemyOnContact.colorState.BLUE)
                colorString = "BLUE";
            else if (this.GetComponent<HurtPlayerOnContact>().enemyState == HurtEnemyOnContact.colorState.GREEN)
                colorString = "GREEN";

            terminalString = "public class VBot{\n     " + writtenString + " \n}";
        }
        else if (this.tag == "Chest")
        {
            terminalString = "public class Chest{\n     System.close(); \n}";
        } else if (this.tag == "movingPlatform")
        {
            terminalString = "public class MovingPlatform{\n   " + writtenString + " \n}";
        }

        checkTerminalString();
        evaluateActions();
    }

    // Update is called once per frame
    void Update()
    {
        if(actions.Contains(keyActions.MOVELEFT) || actions.Contains(keyActions.MOVERIGHT))
            evaluateActions();

        if (globalTerminalMode < 2)
        {
            localTerminalMode = 0;
        }

        if (localTerminalMode == 2)
        {
            checkTerminalString();
            evaluateActions();
            this.GetComponent<LineRenderer>().enabled = true;
            this.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0.4f, 10));
            this.GetComponent<LineRenderer>().SetPosition(1, new Vector3(terminalPointerDestination.transform.position.x - this.transform.position.x,
                                                        terminalPointerDestination.transform.position.y - this.transform.position.y,
                                                        10));
        }
        else
        {
            this.GetComponent<LineRenderer>().enabled = false;
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

    void OnGUI()
    {
        if (localTerminalMode == 2)
        {            
            //terminalString = GUI.TextArea(new Rect(.66f * Screen.width, 10, Screen.width / 3, Screen.height / 2), terminalString, 200);
            terminalString = GUI.TextArea(new Rect(.6f * Screen.width, 10, .4f * Screen.width, Screen.height / 2), terminalString, 200, terminalStyle);
        }
    }

    public void checkTerminalString()
    {
        actions.Clear();
        if (this.tag == "Enemy")
        {
            if (!terminalString.Contains("public class VBot{\n"))
            {
                terminalString = "public class VBot{\n" + terminalString.Substring(18);
            }

            if (!terminalString.Contains("}"))
            {
                terminalString = terminalString + "}";
            }

            string passedInString = terminalString;
            passedInString = passedInString.Replace("public class VBot{\n", "");
            passedInString = passedInString.Replace("}", "");
            actions = parse.Parse(passedInString);

        } else if(this.tag == "Chest")
        {
            if (!terminalString.Contains("public class Chest{\n"))
            {
                terminalString = "public class Chest{\n" + terminalString.Substring(19);
            }

            if (!terminalString.Contains("}"))
            {
                terminalString = terminalString + "}";
            }

            string passedInString = terminalString;
            passedInString = passedInString.Replace("public class Chest{\n", "");
            passedInString = passedInString.Replace("}", "");
            actions = parse.Parse(passedInString);
        } else if(this.tag == "movingPlatform")
        {
            if (!terminalString.Contains("public class MovingPlatform{\n"))
            {
                terminalString = "public class MovingPlatform{\n" + terminalString.Substring(27);
            }


            if (!terminalString.Contains("}"))
            {
                terminalString = terminalString + "}";
            }

            string passedInString = terminalString;
            passedInString = passedInString.Replace("public class MovingPlatform{\n", "");
            passedInString = passedInString.Replace("}", "");
            actions = parse.Parse(passedInString);

        }
    }

    public void evaluateActions()
    {
        foreach (keyActions action in actions)
        {
            switch (action)
            {
                case keyActions.TURNBLUE:
                    if (this.gameObject.tag == "Enemy")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.BLUE;
                        this.GetComponent<SpriteRenderer>().sprite = blueSlime;
                    }
                    break;
                case keyActions.TURNRED:
                    if (this.gameObject.tag == "Enemy")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.RED;
                        this.GetComponent<SpriteRenderer>().sprite = redSlime;
                    }
                    break;
                case keyActions.TURNGREEN:
                    if (this.gameObject.tag == "Enemy")
                    {
                        this.GetComponent<HurtPlayerOnContact>().enemyState = HurtEnemyOnContact.colorState.GREEN;
                        this.GetComponent<SpriteRenderer>().sprite = greenSlime;
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
                    }
                    break;
                case keyActions.CLOSE:
                    if(this.gameObject.tag == "Chest")
                    {
                        this.GetComponent<SpriteRenderer>().sprite = chestSpriteOpen;
                    }
                    break;
                case keyActions.MOVELEFT:
                    this.gameObject.transform.Translate(Vector3.left * Time.deltaTime);
                    break;
                case keyActions.MOVERIGHT:
                    this.gameObject.transform.Translate(Vector3.right * Time.deltaTime);
                    break;
            }
        }
    }
}

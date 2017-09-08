using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class IntelliSenseLevel3 : IntelliSense
{

    float moveSpeed = 7;

    [Header("In-Game Objects")]
    public GameObject mouseClickPrompt;

    [Header("Level 2 Characters")]
    public GameObject addressTable;
    public Sprite flint;
    public Sprite dec;
    public Sprite word;
    public Sprite boole;

    [Header("Level 3 Objects")]
    [Tooltip("Only attach game objects if in level one")]
    public GameObject firstTutorialObjective;
    public GameObject secondTutorialObjective;
    public GameObject thirdTutorialObjective;
    public GameObject fourthTutorialObjective;
    public GameObject fourthObjectiveBarrier;
    public GameObject seventhTutorialBarrier;
    public GameObject levelTitle;


    private bool tutorialCheck = false;
    private bool kernelCheck = false;
    public static bool clickOnce = false;

    private GameObject terminalWindow;
    private float startTime = 0;
    private int numClicks = 0;

    // Use this for initialization
    public void Start()
    {

        base.Start();

        if (PlayerStats.checkpoint == "Checkpoint1")
        {
            SetDialogue("startLevel");
        }

        Transform[] trs = GameObject.Find("Main HUD").GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "Terminal Window")
            {
                terminalWindow = t.gameObject;
            }
        }

        this.gameObject.transform.parent = null;
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();

        // THIS SECTION IS TO MAKE INTELLISENSE MOVE UP AND DOWN PERIODICALLY
        if (whatToSay != null && PlayerStats.checkpoint == "Checkpoint1" && talking && (dialogueIndex < whatToSay.Count || !eventName.Contains("moveTo")))
        {
            transform.position = new Vector2(transform.position.x, y0 + amplitude * Mathf.Sin(speed * Time.time));
        }

        if (talking)
        {           
            if (whatToSay[dialogueIndex].who == "NULL")
            {
                base.SetCharacterIcon(this.GetComponent<SpriteRenderer>().sprite);
            }           
        }

        // THIS SECTION CHECKS IF ENEMY HAS BEEN CLICKED FOR TUTORIAL PURPOSES
       // if (Input.GetMouseButtonDown(0))
       // {
       //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
       //     RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
       //     string variabullText = "";

       //     // check if a variabull is with us
       //     if (terminalWindow.transform.parent.GetComponent<TerminalWindowUI>().variabullRef.activeSelf)
       //     {
       //         variabullText = terminalWindow.transform.parent.GetComponent<TerminalWindowUI>().variaCode.GetComponent<Text>().text;
       //     }

       //     if (hit
       //        && ((hit.collider.name == "Double Entrance" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().parameters.Contains("int flint = 5;") && variabullText != "int flint = 5;")
       //        || (hit.collider.name == "String Entrance" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().parameters.Contains("double dec = 0.25;") && variabullText != "double dec = 0.25;")
       //        || (hit.collider.name == "Boolean Entrance" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().parameters.Contains("string word = \"sentence\";") && variabullText != "string word = \"sentence\";"))
       //        || (hit.collider.name == "ExitEntrance" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().parameters.Contains("boolean bool = true;") && variabullText != "boolean bool = true;")
       //        && hit.collider.GetComponent<EnemyTerminal>().localTerminalMode == 2 && !talking)
       //     {
       //         SetDialogue("cannotAccess"); // clicked any object to access another section without variableS               
       //     }
       //     else if (hit && hit.collider.name == "FlintActivator" && variabullText != "int flint = 5;"
       //       && hit.collider.GetComponent<EnemyTerminal>().localTerminalMode == 2 && !talking)
       //     {
       //         SetDialogue("seeFlint");
       //     }
       //     else if (hit && hit.collider.name == "FlintActivator" && variabullText == "int flint = 5;"
       //       && hit.collider.GetComponent<EnemyTerminal>().localTerminalMode == 2 && !talking && !clickOnce)
       //     {
       //         clickOnce = true;
       //         SetDialogue("useFlint");
       //     }
       //     else if (hit && hit.collider.name == "DecRotator" && hit.collider.GetComponent<EnemyTerminal>().localTerminalMode == 2 && !talking
       //       && variabullText != "double dec = 0.25;")
       //     {
       //         SetDialogue("seeDec");
       //     }
       //     else if (hit && hit.collider.name == "DoubleNotEqualBlocks")
       //     {
       //         numClicks++;
       //         if (numClicks == 3)
       //         {
       //             SetDialogue("DoubleNotEqualInt");
       //         }
       //     }
       //     else if (hit && hit.collider.name == "FirstLetterActivator" && !talking && clickOnce)
       //     {
       //         clickOnce = false;
       //         SetDialogue("SystemDelete");
       //     }
       //     else if (hit && hit.collider.name == "WordActivator" && hit.collider.GetComponent<EnemyTerminal>().localTerminalMode == 2 && !talking
       //       && variabullText != "word word = \"sentence\";")
       //     {
       //         SetDialogue("seeWord");
       //     }
       //     else if (hit && hit.collider.name == "WordActivator" && hit.collider.GetComponent<EnemyTerminal>().localTerminalMode == 2 && !talking
       // && variabullText == "word word = \"sentence\";" && clickOnce)
       //     {
       //         if (clickOnce)
       //         {
       //             clickOnce = false;
       //             SetDialogue("fillInWord");
       //         }
       //     }
       //     else if (hit && hit.collider.name == "SubstringActivator" && hit.collider.GetComponent<EnemyTerminal>().localTerminalMode == 2 && !talking
       //&& variabullText == "word word = \"sentence\";" && clickOnce)
       //     {
       //         if (!clickOnce)
       //         {
       //             clickOnce = true;
       //             SetDialogue("Substring");
       //         }
       //     }
       //     else if (hit && hit.collider.name == "VCrush" && hit.collider.GetComponent<EnemyTerminal>().localTerminalMode == 2)
       //     {
       //         if (clickOnce)
       //         {
       //             clickOnce = false;
       //             smashAPI.SetActive(true);
       //         }
       //     }
       //     else if (hit && hit.collider.name == "Transistor 1" && hit.collider.GetComponent<EnemyTerminal>().localTerminalMode == 2 && !talking)
       //     {
       //         SetDialogue("GateTutorial");
       //     }
       //     else if (hit && hit.collider.name == "Transistor 9" && hit.collider.GetComponent<EnemyTerminal>().localTerminalMode == 2 && !talking)
       //     {
       //         SetDialogue("GateTutorial2");
       //     }
       // }
    }

    #region EventSystem
    /// <summary>
    /// Only executes once when the dialogue needs to be initiated in the game
    /// </summary> 
    public override void initialEvent(string eName)
    {
        switch (eName)
        {
            
        }
    }

    /// <summary>
    /// The event system looks at the event name key and executes certain
    /// actions depending on that event name key.
    /// </summary> 
    public override void performEvent()
    {
        switch (eventName)
        {
        }
    }
    #endregion
}

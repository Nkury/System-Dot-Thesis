using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class IntelliSenseLevel7 : IntelliSense
{

    float moveSpeed = 7;

    [Header("In-Game Objects")]
    public GameObject mouseClickPrompt;
    public GameObject virusCount;
    public GameObject levelExit;

    [Header("Level 3 Objects")]
    [Tooltip("Only attach game objects if in level one")]
    public GameObject firstZoneBarrier;
    public GameObject firstTutorialObjective;
    public GameObject secondTutorialObjective;

    public static int virusCount1 = 13;
    public static int virusCount2 = 9;
    public static int virusCount3 = 5;
    public static int virusCount4 = 1;

    public GameObject thirdTutorialObjective;
    public GameObject fourthTutorialObjective;
    public GameObject fourthObjectiveBarrier;
    public GameObject seventhTutorialBarrier;
    public GameObject levelTitle;


    private bool tutorialCheck = false;
    private bool kernelCheck = false;

    public int virusCheck = 0;

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
 
        if (int.Parse(virusCount.GetComponent<Text>().text) <= 0 && virusCheck == 4)
        {
            virusCheck = 5;
            levelExit.SetActive(true);
            SaveLoad.Save();
            SetDialogue("KillAllViruses");
        }
        else if (int.Parse(virusCount.GetComponent<Text>().text) <= virusCount4 && virusCheck == 3)
        {
            virusCheck = 4;
            SaveLoad.Save();
            SetDialogue("KillingViruses4");
        }
        else if (int.Parse(virusCount.GetComponent<Text>().text) <= virusCount3 && virusCheck == 2)
        {
            virusCheck = 3;
            SaveLoad.Save();
            SetDialogue("KillingViruses3");
        }
        else if (int.Parse(virusCount.GetComponent<Text>().text) <= virusCount2 && virusCheck == 1)
        {
            virusCheck = 2;
            SaveLoad.Save();
            SetDialogue("KillingViruses2");
        }
        else if (int.Parse(virusCount.GetComponent<Text>().text) <= virusCount1 && virusCheck == 0)
        {
            virusCheck = 1;
            SaveLoad.Save();
            SetDialogue("KillingViruses1");
        }

        // THIS SECTION CHECKS IF ENEMY HAS BEEN CLICKED FOR TUTORIAL PURPOSES
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            //     // check if a variabull is with us
            //     if (terminalWindow.transform.parent.GetComponent<TerminalWindowUI>().variabullRef.activeSelf)
            //     {
            //         variabullText = terminalWindow.transform.parent.GetComponent<TerminalWindowUI>().variaCode.GetComponent<Text>().text;
            //     }

            if (hit && hit.collider.name == "movingPlatform (6)" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce)
            {
                hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce = true;
                SetDialogue("discoverIf");
            } else if(hit && hit.collider.name == "firstBranch" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce)
            {
                hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce = true;
                SetDialogue("firstPipe");
            }
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
        }
    }

    public override void SetDialogue(string keyWord)
    {
        base.SetDialogue(keyWord);
        SetCharacterIcon(this.GetComponent<SpriteRenderer>().sprite);
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
            case "proceedPastFirstZone":
                PlayerStats.deadObjects.Add(firstZoneBarrier.name);
                Destroy(firstZoneBarrier);
                break;
            case "pushOut":
                PlayerController player = GameObject.FindObjectOfType<PlayerController>();
                player.gameObject.transform.position = new Vector2(-105.76f, -0.95f);
                break;

        }
    }
    #endregion
}

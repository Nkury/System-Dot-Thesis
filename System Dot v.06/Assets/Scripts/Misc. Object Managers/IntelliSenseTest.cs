using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class IntelliSenseTest : IntelliSense {
    
    float moveSpeed = 7;

    [Header("In-Game Objects")]    
    public GameObject mouseClickPrompt;

    [Header("Level 1 Objects")]
    [Tooltip("Only attach game objects if in level one")]
    public GameObject firstTutorialObjective;
    public GameObject secondTutorialObjective;
    public GameObject thirdTutorialObjective;
    public GameObject fourthTutorialObjective;
    public GameObject fourthObjectiveBarrier;
    public GameObject seventhTutorialBarrier;
    public GameObject namePrompt;
    public GameObject hackPrompt;
    public GameObject levelTitle;

    [Header("UI Components")]
    public GameObject apiButton;
    public GameObject UIClickPrompt;
    public GameObject debugButton;
    public InputField tutorialLine;
    public GameObject APIInfo;
    public GameObject directionHelpButton;
    public GameObject chestHelpButton;

    private bool tutorialCheck = true;
    private bool noMoreReadOnly = true;
    private bool kernelCheck = false;
    public static bool clickOnce = false;

    private float startTime = 0;

    // Use this for initialization
    public void Start () {
        
        if(PlayerStats.checkpoint == "Checkpoint1"){
            startDifferent = true;                 
            base.Start();
            SetDialogue("startGame");
        }
        else
        {
            startDifferent = false;
            base.Start();
        }       
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();

        // THIS SECTION IS TO HAVE INTELLISENSE ZOOM OUT WHEN STARTING LEVEL 1
        allowZooming = PlayerStats.checkpoint != "Checkpoint1";
   
        // THIS SECTION IS TO MAKE INTELLISENSE MOVE UP AND DOWN PERIODICALLY
        if (whatToSay != null && PlayerStats.checkpoint == "Checkpoint1" && talking && (dialogueIndex < whatToSay.Count || !eventName.Contains("moveTo")))
        {
            transform.position = new Vector2(transform.position.x, y0 + amplitude * Mathf.Sin(speed * Time.time));
        }     

        // THIS SECTION CHECKS IF ENEMY HAS BEEN CLICKED FOR TUTORIAL PURPOSES
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit && hit.collider.name == "TutorialEnemy" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce)
            {
                hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce = true;
                SetDialogue("postHack"); // clicked the first VBot encountered
            } else if(hit && hit.collider.name == "TutorialEnemy2" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce)
            {
                hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce = true;
                SetDialogue("clickBlackVBot"); // clicked the first black VBot
            } else if(hit && hit.collider.name == "TutorialChest" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce)
            {
                mouseClickPrompt.SetActive(false);
                if (!PlayerStats.deadObjects.Contains("SixthTutorialObjective"))
                {
                    PlayerStats.deadObjects.Add("SixthTutorialObjective");
                    Destroy(GameObject.Find("SixthTutorialObjective"));
                }
                hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce = true;
                SetDialogue("clickChest"); // click the first chest encountered
            } else if(hit && hit.collider.name == "TutorialPlatform" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce)
            {
                mouseClickPrompt.SetActive(false);
                hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce = true;
                SetDialogue("clickPlatform"); // click first platform encountered
                mouseClickPrompt.SetActive(false);
            } // for comment tutorial in level 1
            else if (hit && hit.collider.name.Contains("Comment") && !hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce)
            {
                hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce = true;
                SetDialogue("discoverComments");
            }
        }

        if (PlayerStats.highestCheckpoint < 3)
        {
            // FOR BLACK VBOT TUTORIAL
            // CHECKS TO MAKE SURE CODE IS NOT LEGACY BEFORE DOING THIS CHECK
            if (tutorialLine.textComponent.color != Color.red && noMoreReadOnly)
            {
                tutorialLine.readOnly = tutorialCheck;
            }

            // for black Vbot tutorial
            if (!tutorialCheck)
            {
                if (tutorialLine.textComponent.text == "System.body(Color.GREEN);")
                {
                    tutorialCheck = true;
                    noMoreReadOnly = false;
                    GameObject.Find("Intellisense").GetComponent<IntelliSenseTest>().SetDialogue("codeFixed");
                }
            }
        }
    }  

    //public void commentFound()
    //{
    //    if (!clickOnce)
    //    {
    //        SetDialogue("discoverComments");
    //        eventName = "finishDialogue";
    //    }
    //}



    // looks in dictionary and sets the dialogue to certain keyword passed in
    public override void SetDialogue(string message)
    {
        base.SetCharacterIcon(this.GetComponent<SpriteRenderer>().sprite);
        base.SetDialogue(message);
    }

    #region EventSystem
    /// <summary>
    /// Only executes once when the dialogue needs to be initiated in the game
    /// </summary> 
    public override void initialEvent(string eName)
    {
        switch (eName)
        {
            case "receiveName":
                PlayerStats.playerName = namePrompt.transform.Find("name").GetComponent<Text>().text;
                namePrompt.SetActive(false);

                // if nothing is entered by the player, then give the player the default name "BOB"
                if (PlayerStats.playerName == "")
                {
                    PlayerStats.playerName = "Bob";
                    whatToSay[0].say = "Nothing? Will \"BOB\" suffice then" + whatToSay[0].say;
                }
    
                break;
            case "unlockChest":
                initialEvent("APIClicked"); // call the event system
                if (!PlayerStats.deadObjects.Contains("SixthTutorialObjective"))
                {
                    PlayerStats.deadObjects.Add("SixthTutorialObjective");
                    Destroy(GameObject.Find("SixthTutorialObjective"));
                }
                chestHelpButton.SetActive(true);
                startTime = 0;
                break;
            case "colorChanged":
                if (GameObject.Find("FifthTutorialObjective"))
                {
                    GameObject destroy = GameObject.Find("FifthTutorialObjective");
                    Destroy(destroy);
                    PlayerStats.deadObjects.Add(destroy.name);
                }
                break;
            case "killedTutorialEnemy":
                PlayerStats.deadObjects.Add(fourthObjectiveBarrier.name);
                Destroy(fourthObjectiveBarrier);
                break;
            case "inputHack":
                string inputtedCode = hackPrompt.transform.Find("code").GetComponent<Text>().text;
                if (inputtedCode == "System.body(Color.BLUE);")
                {
                    SetDialogue("correctHack");
                    hackPrompt.SetActive(false);
                }
                else
                {
                    SetDialogue("wrongHack");
                    hackPrompt.transform.Find("code").GetComponent<Text>().text = "";
                }
                break;
            case "movePlatform2":
                initialEvent("APIClicked");
                PlayerStats.deadObjects.Add(seventhTutorialBarrier.name);
                Destroy(seventhTutorialBarrier.gameObject);
                directionHelpButton.SetActive(true);
                break;
            case "TutorialKernel":
                if (!kernelCheck)
                {
                    kernelCheck = true;
                    SetDialogue("startMovingPlatform");
                }
               break;
            case "movePlatform": // OR
            case "codeFixed": // OR 
            case "APIClicked":
                GameObject myEventSystem = GameObject.Find("EventSystem");
                myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
                break;
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
            case "promptForName":
                if (namePrompt)
                {
                    namePrompt.SetActive(true);
                    namePrompt.GetComponent<InputField>().Select();
                }
                break;
            case "moveToFirstTutorial":
                if (firstTutorialObjective != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, firstTutorialObjective.transform.position, moveSpeed * Time.deltaTime);
                    if (transform.position.x >= firstTutorialObjective.transform.position.x)
                        y0 = firstTutorialObjective.transform.position.y;
                }
                talking = false;
                break;
            case "moveToSecondTutorial":
                if (secondTutorialObjective != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, secondTutorialObjective.transform.position, moveSpeed * Time.deltaTime);
                    if (transform.position.x >= secondTutorialObjective.transform.position.x)
                        y0 = secondTutorialObjective.transform.position.y;
                }
                talking = false;
                break;
            case "moveToThirdTutorial":
                if (thirdTutorialObjective != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, thirdTutorialObjective.transform.position, moveSpeed * Time.deltaTime);
                    if (transform.position.x >= thirdTutorialObjective.transform.position.x)
                        y0 = thirdTutorialObjective.transform.position.y;
                }
                talking = false;
                break;
            case "moveToFourthTutorial":
                LevelManager.canPressTab = true;
                if (fourthTutorialObjective != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, fourthTutorialObjective.transform.position, moveSpeed * Time.deltaTime);
                    if (transform.position.x >= fourthTutorialObjective.transform.position.x)
                        y0 = fourthTutorialObjective.transform.position.y;
                }
                talking = false;
                break;
            case "promptClick":
                mouseClickPrompt.SetActive(true);
                break;
            case "clickAPI":
                UIClickPrompt.SetActive(true);
                apiButton.SetActive(true);
                break;
            case "promptCode":
                mouseClickPrompt.SetActive(false);
                hackPrompt.SetActive(true);
                hackPrompt.GetComponent<InputField>().Select();
                break;
            case "stopTalking":
                EnemyTerminal.globalTerminalMode = 0;
                talking = false;
                this.transform.parent = null;
                break;
            case "titleSequence":
                if (levelTitle != null)
                    levelTitle.SetActive(true);
                talking = false;
                ZoomIntoPlayer();
                break;
            case "editCode":
                tutorialCheck = false;
                break;
            case "clickDebug":
                UIClickPrompt.SetActive(true);
                mouseClickPrompt.SetActive(false);
                debugButton.SetActive(true);
                UIClickPrompt.GetComponent<RectTransform>().anchoredPosition = new Vector2(710, -30);
                break;
            case "startTimer":
                startTime += Time.deltaTime;
                if (startTime > 30)
                    SetDialogue("helpWithChest");
                break;
            case "finishDialogue":
                APIInfo.SetActive(false);
                eventName = "";
                dialogueIndex++;
                break;
        }
        
    }
    #endregion
}

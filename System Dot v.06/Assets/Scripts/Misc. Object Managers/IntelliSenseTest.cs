using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class IntelliSenseTest : MonoBehaviour {

    Dictionary<string, List<string>> dialogue = new Dictionary<string, List<string>>();
    Dictionary<string, string> events = new Dictionary<string, string>();

    [Header("Talking")]
    public bool talking;
    public List<string> whatToSay;
    public string dialogueFileName;

    float y0;
    float amplitude = .2f;
    float speed = 1.5f;
    float moveSpeed = 7;

    [Header("In-Game Objects")]
    public GameObject player;
    public GameObject intelliLocation;
    public GameObject dialogueBox;
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

    private int index = 0;
    private int interval = 0;
    private int dialogueIndex = 0;
    private string eventName = "";

    private bool tutorialCheck = false;
    public static bool clickOnce = false;

    private float startTime = 0;

    // Use this for initialization
    void Start () {

        XDocument loadedData = XDocument.Load("../System Dot v.06/Assets/Scripts/Dialogue/" + dialogueFileName + ".xml");
        List<string> addedDialogue = new List<string>();
        string keyName;
        string eventName;

        foreach(XElement messElement in loadedData.Descendants("message"))
        {
            keyName = messElement.Attribute("id").Value;
            eventName = "";
            if(messElement.Attribute("event") != null)
            {
                eventName = messElement.Attribute("event").Value;
            }       

            addedDialogue = new List<string>();
            foreach (XElement element in messElement.Elements("say"))
            {
                addedDialogue.Add(element.Value);
            }

            dialogue.Add(keyName, addedDialogue);
            events.Add(keyName, eventName);
        }

        if(PlayerStats.checkpoint == "Checkpoint1"){
            SetDialogue("startGame");
        }
        else
        {
            this.transform.parent = player.transform;
            this.transform.localPosition = new Vector2(0, 0);
            this.transform.localScale = new Vector2(0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // THIS SECTION IS TO SKIP AND DISPLAY ALL THE TEXT AT ONCE
        if (dialogueIndex < whatToSay.Count && index < whatToSay[dialogueIndex].Length)
        {
            if(PlayerStats.checkpoint != "Checkpoint1")
            {
                ZoomOutPlayer();
            }
            
            dialogueBox.transform.Find("spacebar image").gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                index = whatToSay[dialogueIndex].Length;
            }
        }
        // THIS SECTION IS RESPONSIBLE FOR PRINTING OUT TEXT LIKE A VIDEO GAME
        else if (dialogueIndex < whatToSay.Count)
        {
            // go to the next string
            dialogueBox.transform.Find("spacebar image").gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogueIndex++;
                index = 0;
            }
            // THIS SECTION IS TO KEEP THE TEXT PRESENT WHILE ASSESSING AN EVENT
        }
        else if (eventName != "")
        {
            dialogueBox.transform.Find("spacebar image").gameObject.SetActive(false);
            // check for special events mid-dialogue
            performEvent();
            // THIS SECTION IS TO SIGNAL THAT WE ARE DONE TALKING AND PLAYER IS FREE TO MOVE
        }
        else {
            if(PlayerStats.checkpoint != "Checkpoint1")
            {
                ZoomIntoPlayer();
            }
            talking = false;
        }

        // THIS SECTION IS TO MAKE INTELLISENSE MOVE UP AND DOWN PERIODICALLY
        if (PlayerStats.checkpoint == "Checkpoint1" && talking && (dialogueIndex < whatToSay.Count || !eventName.Contains("moveTo")))
        {
            transform.position = new Vector2(transform.position.x, y0 + amplitude * Mathf.Sin(speed * Time.time));
        }

        // THIS SECTION IS TO PRINT OUT WHAT THE CHARACTER SAYS ON THE SCREEN
        if (dialogueIndex < whatToSay.Count && index <= whatToSay[dialogueIndex].Length)
        {
            dialogueBox.GetComponentInChildren<Text>().text = whatToSay[dialogueIndex].Substring(0, index);
        }

        // THIS SECTION IS TO DISPLAY THE DIALOGUE BOX IF THE PLAYER IS TALKING
        if (talking)
        {
            dialogueBox.SetActive(true);
        }
        else
        {
            dialogueBox.SetActive(false);
        }

        // THIS PART IMMOBOLIZES THE PLAYER
        if(player.GetComponentInParent<PlayerController>())
            player.GetComponentInParent<PlayerController>().IntelliSenseTalking(talking);

        // THIS SECTION IS TO MEDIATE THE TIME THE TEXT APPEARS ON SCREEN
        if (interval % 3 == 0)
            index++;

        interval++;

        // THIS SECTION CHECKS IF ENEMY HAS BEEN CLICKED FOR TUTORIAL PURPOSES
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (!clickOnce && hit && hit.collider.name == "TutorialEnemy")
            {
                clickOnce = true;
                SetDialogue("postHack"); // clicked the first VBot encountered
            } else if(clickOnce && hit && hit.collider.name == "TutorialEnemy2")
            {
                clickOnce = false;
                SetDialogue("clickBlackVBot"); // clicked the first black VBot
            } else if(!clickOnce && hit && hit.collider.name == "TutorialChest")
            {
                mouseClickPrompt.SetActive(false);
                if (!PlayerStats.deadObjects.Contains("SixthTutorialObjective"))
                {
                    PlayerStats.deadObjects.Add("SixthTutorialObjective");
                    Destroy(GameObject.Find("SixthTutorialObjective"));
                }
                clickOnce = true;
                SetDialogue("clickChest"); // click the first chest encountered
            } else if(clickOnce && hit && hit.collider.name == "TutorialPlatform")
            {
                mouseClickPrompt.SetActive(false);
                clickOnce = false;
                SetDialogue("clickPlatform"); // click first platform encountered
                mouseClickPrompt.SetActive(false);
            }
        }

        // FOR BLACK VBOT TUTORIAL
        // CHECKS TO MAKE SURE CODE IS NOT LEGACY BEFORE DOING THIS CHECK
        if (tutorialLine.textComponent.color != Color.red)
        {
            tutorialLine.readOnly = tutorialCheck;
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

    public void ZoomIntoPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 10 * Time.deltaTime);
        if (transform.localScale.x > 0)
            transform.localScale += Vector3.one * -.01f;
    }

    public void ZoomOutPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, intelliLocation.transform.position, 3 * Time.deltaTime);
        if (transform.localScale.x < .25f)
            transform.localScale += Vector3.one * .01f;
        else
        {
            y0 = this.transform.position.y;
            transform.localScale = new Vector3(.25f, .25f, 1);
        }
    }    

    // looks in dictionary and sets the dialogue to certain keyword passed in
    public void SetDialogue(string keyWord)
    {
        // resets dialogue
        dialogueIndex = 0;
        index = 0;

        List<string> sayThis;
        if (dialogue.TryGetValue(keyWord, out sayThis))
        {
            whatToSay = sayThis;
        }

        string eName;
        if(events.TryGetValue(keyWord, out eName))
        {
            eventName = eName;
        }

        initialEvent(keyWord);

        talking = true;
    }

    #region EventSystem
    /// <summary>
    /// Only executes once when the dialogue needs to be initiated in the game
    /// </summary> 
    public void initialEvent(string eName)
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
                    whatToSay[0] = "Nothing? Will \"BOB\" suffice then" + whatToSay[0];

                }
                else
                {
                    whatToSay[0] = PlayerStats.playerName.ToUpper() + whatToSay[0];
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
    public void performEvent()
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

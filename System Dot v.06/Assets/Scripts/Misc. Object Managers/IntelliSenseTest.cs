﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class IntelliSenseTest : MonoBehaviour {

    Dictionary<string, List<string>> dialogue = new Dictionary<string, List<string>>();

    public bool talking;
    public List<string> whatToSay;

    float y0;
    float amplitude = .2f;
    float speed = 1.5f;

    public GameObject player;
    public GameObject intelliLocation;
    public GameObject dialogueBox;
    public GameObject namePrompt;
    public GameObject hackPrompt;
    public GameObject mouseClickPrompt;

    public GameObject firstTutorialObjective;
    public GameObject secondTutorialObjective;
    public GameObject thirdTutorialObjective;
    public GameObject fourthTutorialObjective;
    public GameObject fourthObjectiveBarrier;
    public GameObject levelTitle;

    private int index = 0;
    private int interval = 0;
    private int dialogueIndex = 0;
    private string eventName = "";

	// Use this for initialization
	void Start () {

        XDocument loadedData = XDocument.Load("../System Dot v.06/Assets/Scripts/Dialogue/TutorialDialogue.xml");
        List<string> addedDialogue = new List<string>();
        string keyName;

        foreach(XElement messElement in loadedData.Descendants("message"))
        {
            keyName = messElement.Attribute("id").Value;
            addedDialogue = new List<string>();
            foreach (XElement element in messElement.Elements("say"))
            {
                addedDialogue.Add(element.Value);
            }

            dialogue.Add(keyName, addedDialogue);
        }

        startTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        // THIS SECTION IS RESPONSIBLE FOR PRINTING OUT TEXT LIKE A VIDEO GAME
        if (dialogueIndex < whatToSay.Count && index < whatToSay[dialogueIndex].Length)
        {
            dialogueBox.transform.Find("spacebar image").gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                index = whatToSay[dialogueIndex].Length;
            }
        }
        // THIS SECTION IS TO SKIP AND DISPLAY ALL THE TEXT AT ONCE
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
            talking = false;
        }

        // THIS SECTION IS TO MAKE INTELLISENSE MOVE UP AND DOWN PERIODICALLY
        if (dialogueIndex < whatToSay.Count || !eventName.Contains("moveTo"))
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

            if (hit && hit.collider.name == "TutorialEnemy")
            {
                botClicked();
            }
        }
    }

    public void startTutorial()
    {
        SetDialogue("startGame");
        eventName = "promptForName";
    }

    public void nameEntered()
    {
        PlayerStats.playerName = namePrompt.transform.Find("name").GetComponent<Text>().text;
        namePrompt.SetActive(false);
        SetDialogue("receiveName");
        whatToSay[0] = PlayerStats.playerName.ToUpper() + whatToSay[0];
        eventName = "moveToFirstTutorial";
    }

    public void StartSecondTutorial()
    {
        SetDialogue("catchUp1");
        eventName = "moveToSecondTutorial";
    }

    public void StartThirdTutorial()
    {
        SetDialogue("catchUp2");
        eventName = "moveToThirdTutorial";
    }

    public void StartFourthTutorial()
    {
        SetDialogue("catchUp3");
        eventName = "moveToFourthTutorial";
    }

    public void StartFifthTutorial()
    {
        SetDialogue("preHack");
        eventName = "promptClick";
    }

    public void botClicked()
    {
        SetDialogue("postHack");
        eventName = "promptCode";
    }

    public void botKilled()
    {
        SetDialogue("killedTutorialEnemy");
        PlayerStats.deadObjects.Add(fourthObjectiveBarrier.name);
        Destroy(fourthObjectiveBarrier);
        eventName = "titleSequence";
    }

    public void InputtedCode()
    {
        string inputtedCode = hackPrompt.transform.Find("code").GetComponent<Text>().text;
        if (inputtedCode == "System.body(Color.BLUE);")
        {
            SetDialogue("correctHack");
            hackPrompt.SetActive(false);
            eventName = "killEnemy1";
        }
        else {
            SetDialogue("wrongHack");
            hackPrompt.transform.Find("code").GetComponent<Text>().text = "";
        }
    }

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


    public void performEvent()
    {
        switch (eventName)
        {
            case "promptForName":
                namePrompt.SetActive(true);
                namePrompt.GetComponent<InputField>().Select();
                break;
            case "moveToFirstTutorial":
                if (firstTutorialObjective != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, firstTutorialObjective.transform.position, 4 * Time.deltaTime);
                    if (transform.position.x >= firstTutorialObjective.transform.position.x)
                        y0 = firstTutorialObjective.transform.position.y;
                }
                talking = false;
                break;
            case "moveToSecondTutorial":
                if (secondTutorialObjective != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, secondTutorialObjective.transform.position, 6 * Time.deltaTime);
                    if (transform.position.x >= secondTutorialObjective.transform.position.x)
                        y0 = secondTutorialObjective.transform.position.y;
                }
                talking = false;
                break;
            case "moveToThirdTutorial":
                if (thirdTutorialObjective != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, thirdTutorialObjective.transform.position, 4 * Time.deltaTime);
                    if (transform.position.x >= thirdTutorialObjective.transform.position.x)
                        y0 = thirdTutorialObjective.transform.position.y;
                }
                talking = false;
                break;
            case "moveToFourthTutorial":
                if (fourthTutorialObjective != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, fourthTutorialObjective.transform.position, 4 * Time.deltaTime);
                    if (transform.position.x >= fourthTutorialObjective.transform.position.x)
                        y0 = fourthTutorialObjective.transform.position.y;
                }
                talking = false;
                break;
            case "promptClick":
                mouseClickPrompt.SetActive(true);
                break;
            case "promptCode":
                mouseClickPrompt.SetActive(false);
                PlayerStats.deadObjects.Add(mouseClickPrompt.name);
                hackPrompt.SetActive(true);
                hackPrompt.GetComponent<InputField>().Select();
                break;
            case "killEnemy1":
                EnemyTerminal.globalTerminalMode = 0;
                talking = false;
                break;
            case "titleSequence":
                if (levelTitle != null)
                    levelTitle.SetActive(true);
                talking = false;
                ZoomIntoPlayer();
                break;
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
            talking = true;
        }
    }
}

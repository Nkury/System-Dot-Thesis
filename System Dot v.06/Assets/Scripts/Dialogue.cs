using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

    Dictionary<string, List<string>> dialogue = new Dictionary<string, List<string>>();
    Dictionary<string, string> events = new Dictionary<string, string>();

    public int index = 0;
    private int interval = 0;
    public int dialogueIndex = 0;

    public GameObject dialogueBox;
    public GameObject player;

    [Header("Talking")]
    public bool talking;
    public List<string> whatToSay;
    public string dialogueFileName;

    public string eventName = "";

    // Use this for initialization
    public void Start () {
        XDocument loadedData = XDocument.Load("../System Dot v.06/Assets/Scripts/Dialogue/" + dialogueFileName + ".xml");
        List<string> addedDialogue = new List<string>();
        string keyName;

        string eventName;

        foreach (XElement messElement in loadedData.Descendants("message"))
        {
            keyName = messElement.Attribute("id").Value;
            eventName = "";
            if (messElement.Attribute("event") != null)
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
    }
	
	// Update is called once per frame
	public void Update () {
        // THIS SECTION IS TO SKIP AND DISPLAY ALL THE TEXT AT ONCE
        if (dialogueIndex < whatToSay.Count && index < whatToSay[dialogueIndex].Length)
        {  
            dialogueBox.transform.Find("spacebar image").gameObject.SetActive(false);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                index = whatToSay[dialogueIndex].Length;
            }
        }// THIS SECTION IS RESPONSIBLE FOR PRINTING OUT TEXT LIKE A VIDEO GAME
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
        else
        {
            talking = false;
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
        if (player.GetComponentInParent<PlayerController>())
            player.GetComponentInParent<PlayerController>().IntelliSenseTalking(talking);

        // THIS SECTION IS TO MEDIATE THE TIME THE TEXT APPEARS ON SCREEN
        if (interval % 3 == 0)
            index++;

        interval++;
    }

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
        if (events.TryGetValue(keyWord, out eName))
        {
            eventName = eName;
        }

        initialEvent(keyWord);

        talking = true;
    }

    public virtual void initialEvent(string eName)
    {

    }

    public virtual void performEvent()
    {

    }
}

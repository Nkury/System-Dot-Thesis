using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class sayList
{
    public string who;
    public string say;
    
    public sayList(string whoo, string sayy)
    {
        who = whoo;
        say = sayy;
    }
}

public class Dialogue : MonoBehaviour {

    Dictionary<string, List<sayList>> dialogue = new Dictionary<string, List<sayList>>();
    Dictionary<string, string> events = new Dictionary<string, string>();


    public int index = 0;
    private int interval = 0;
    public int dialogueIndex = 0;

    public GameObject dialogueBox;
    public GameObject characterIcon;
    public GameObject player;

    public AudioSource talkingSoundEffect;

    [Header("Talking")]
    public bool talking;
    public List<sayList> whatToSay;
    public string dialogueFileName;

    public string eventName = "";

    // autoscroll variables
    public bool isAutoScroll = false;
    protected bool nextDialogue;

    // Use this for initialization
    public void Start () {
        XDocument loadedData = XDocument.Load("../System Dot v.06/Assets/Scripts/Dialogue/" + dialogueFileName + ".xml");
        List<sayList> addedDialogue;
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

            addedDialogue = new List<sayList>();
            
            foreach (XElement element in messElement.Elements("say"))
            {
                sayList sayGroup = new sayList(element.Attribute("char").Value, element.Value);
                addedDialogue.Add(sayGroup);
            }

            dialogue.Add(keyName, addedDialogue);
            events.Add(keyName, eventName);
        }
    }   

	public void Update () {

        // for autoscrolling scenarios
        if (isAutoScroll)
        {
            if (dialogueIndex < whatToSay.Count && index >= whatToSay[dialogueIndex].say.Length && nextDialogue){
                AutoScroll();
            }
        }
        else {
            if (whatToSay != null)
            {
                // REPLACE PLAYERNAME WITH PLAYERSTATS.PLAYERNAME
                if (dialogueIndex < whatToSay.Count && whatToSay[dialogueIndex].say.Contains("PLAYERNAME"))
                {
                    if (PlayerStats.playerName == null || PlayerStats.playerName == string.Empty)
                    {
                        whatToSay[dialogueIndex].say = whatToSay[dialogueIndex].say.Replace("PLAYERNAME", "BOB");
                    }
                    else
                    {
                        whatToSay[dialogueIndex].say = whatToSay[dialogueIndex].say.Replace("PLAYERNAME", PlayerStats.playerName.ToUpper());
                    }
                }

                // THIS SECTION IS TO SKIP AND DISPLAY ALL THE TEXT AT ONCE
                if (dialogueIndex < whatToSay.Count && index < whatToSay[dialogueIndex].say.Length)
                {
                    if (dialogueBox.transform.Find("spacebar image"))
                    {
                        dialogueBox.transform.Find("spacebar image").gameObject.SetActive(false);
                    }

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        index = whatToSay[dialogueIndex].say.Length;
                    }
                }// THIS SECTION IS RESPONSIBLE FOR PRINTING OUT TEXT LIKE A VIDEO GAME
                else if (dialogueIndex < whatToSay.Count)
                {
                    // go to the next string
                    if (dialogueBox.transform.Find("spacebar image"))
                    {
                        dialogueBox.transform.Find("spacebar image").gameObject.SetActive(true);
                    }
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        dialogueIndex++;
                        index = 0;
                    }
                    // THIS SECTION IS TO KEEP THE TEXT PRESENT WHILE ASSESSING AN EVENT
                }
                else if (eventName != "")
                {
                    if (dialogueBox.transform.Find("spacebar image"))
                    {
                        dialogueBox.transform.Find("spacebar image").gameObject.SetActive(false);
                    }
                    // check for special events mid-dialogue
                    performEvent();
                    // THIS SECTION IS TO SIGNAL THAT WE ARE DONE TALKING AND PLAYER IS FREE TO MOVE
                }
                else
                {
                    talking = false;
                }
            }        
        }

        if (whatToSay != null)
        {
            // THIS SECTION IS TO PRINT OUT WHAT THE CHARACTER SAYS ON THE SCREEN
            if (dialogueIndex < whatToSay.Count && index <= whatToSay[dialogueIndex].say.Length)
            {
                dialogueBox.GetComponentInChildren<Text>().text = whatToSay[dialogueIndex].say.Substring(0, index);
            }
        }

        // THIS SECTION IS TO DISPLAY THE DIALOGUE BOX IF THE PLAYER IS TALKING  
        dialogueBox.SetActive(talking);

        // THIS SECTION IS TO MEDIATE THE TIME THE TEXT APPEARS ON SCREEN
        if (interval % 3 == 0)
        {
            index++;
            if (whatToSay != null && talking && dialogueIndex < whatToSay.Count && index < whatToSay[dialogueIndex].say.Length)
            {
                talkingSoundEffect.Play();
            }
        }

        interval++;
    }

    public virtual void SetDialogue(string keyWord)
    {
        // resets dialogue
        dialogueIndex = 0;
        index = 0;

        List<sayList> sayThis;
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

    public void SetCharacterIcon(Sprite character)
    {
        characterIcon.GetComponent<Image>().sprite = character;
    }

    public virtual void initialEvent(string eName)
    {

    }

    public virtual void performEvent()
    {

    }

    public virtual void AutoScroll()
    {
        /* EXAMPLE
        nextDialogue = false;
        yield return new WaitForSeconds(1.5f);
        dialogueIndex++;
        index = 0;
        nextDialogue = true;
        */
    }
}

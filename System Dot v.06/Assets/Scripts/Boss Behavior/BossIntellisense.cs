using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class BossIntellisense : MonoBehaviour {

    Dictionary<string, List<string>> dialogue = new Dictionary<string, List<string>>();

    [Header("Talking")]
    public bool talking;
    public List<string> whatToSay;

    float y0;
    float amplitude = .2f;
    float speed = 1.5f;

    [Header("In-Game Objects")]
    public GameObject player;
    public GameObject intelliLocation;
    public GameObject dialogueBox;
    public GameObject ground1;
    public GameObject ground2;
    public GameObject hallway;
    public GameObject bossHealth;

    private int index = 0;
    private int interval = 0;
    private int dialogueIndex = 0;
    private bool nextDialogue = true;
    private bool zoomOut = false;
    private string eventName = "";


    public static bool startBoss = false; // to start the boss battle
    

    // Use this for initialization
    void Start () {
        XDocument loadedData = XDocument.Load("Dialogue/BossDialogue.xml");
        List<string> addedDialogue = new List<string>();
        string keyName;

        foreach (XElement messElement in loadedData.Descendants("message"))
        {
            keyName = messElement.Attribute("id").Value;
            addedDialogue = new List<string>();
            foreach (XElement element in messElement.Elements("say"))
            {
                addedDialogue.Add(element.Value);
            }

            dialogue.Add(keyName, addedDialogue);
        }
        
        this.transform.parent = player.transform;
        this.transform.localPosition = new Vector2(0, 0);
        this.transform.localScale = new Vector2(0, 0);

        dialogueBox.transform.Find("spacebar image").gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        // THIS SECTION IS RESPONSIBLE FOR PRINTING OUT TEXT LIKE A VIDEO GAME
        if (dialogueIndex < whatToSay.Count && index >= whatToSay[dialogueIndex].Length && nextDialogue)
        {
            StartCoroutine(NextDialogue());
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
        if (interval % 2 == 0)
            index++;

        interval++;

        if (zoomOut)
        {
            ZoomOutPlayer();
        }
        else {
            ZoomIntoPlayer();
        }

    }

    public void StartBoss()
    {
        zoomOut = true;
        SetDialogue("startBoss");
    }

    public void invGauntlet()
    {
        zoomOut = true;
        SetDialogue("invincibilityGauntlet");
    }

    public IEnumerator NextDialogue()
    {
        nextDialogue = false;
        yield return new WaitForSeconds(1.5f);
        dialogueIndex++;
        index = 0;

        // when we're finished speaking
        if (dialogueIndex == whatToSay.Count)
        {
            talking = false;

            // set up the boss room
            ground1.SetActive(true);
            ground2.SetActive(true);
            hallway.SetActive(false);

            // set up the boss camera perspective
            Camera.main.orthographicSize = 11;
            Camera.main.GetComponent<CameraController>().yOffset = 9.55f;
            Camera.main.GetComponent<CameraController>().xOffset = 4.5f;
            Camera.main.transform.position = new Vector3(30.36603f, 10.42f, -10);
            Camera.main.GetComponent<CameraController>().isFollowing = false;

            startBoss = true;
            bossHealth.SetActive(true);
            bossHealth.GetComponentInChildren<Slider>().enabled = false;

            // give player armor
            PlayerStats.armorHealth = 1;
            GameObject armorBar = GameObject.Find("Armor Bar");
            armorBar.GetComponent<HealthManager>().SetMaxArmor();
            zoomOut = false;
        }
        nextDialogue = true;
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

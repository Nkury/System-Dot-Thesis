using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntelliSense : MonoBehaviour {

    int interval;

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

    public static bool wait;
    public bool idle;
    public bool zoomIn; // true is zoom in, false is zoom out
    public static bool talking = true; // true if talking, false otherwise
    public bool canBeIdle;

    private int checkForName;
    private int checkForHack;
    private int checkForDeath;
    private int firstTutorialIndex;
    private int secondTutorialIndex;
    private int thirdTutorialIndex;
    private int fourthTutorialIndex;
    private int fifthTutorialIndex;
    private int finishTutorial;

    private string[] dialogue =
    {
        "Oh, hi! I didn’t mean to startle you. I don’t recognize you.", " Who do you identify as?",
        PlayerStats.playerName + "? That is an interesting variable name.", "I am IntelliSense—IS for short. Nice to meet you.",
        "...", "Are you one of those silent video game protagonists", "who can only pronounce their names and nothing else?",
        "Wow, you are, aren't you?", "Hmm... well, don't follow me by pressing either \"A\" or \"D\".", "I'm not going to help you just because I'm the first talking object you've seen.", "Adieu.",

        /*----------10-13------------*/
        "FILLER 11", "So you think you can catch up to me?", "Are you some kind of rogue virus?!", "I'm reporting you to the CPU!",
        "Surely a rogue virus cannot jump over this platform by holding \"W\".",

        /*----------14-19------------*/
        "FILLER 16", "You're relentless, aren't you? Just like a virus...", "Please leave me alone. I've lived a good life and I don't want to get terminated.",
        "It's as if you want to progress through some kind of story and win a game.", "Luckily, you don't know how to double jump by pressing \"W\" twice.",
        "See you never!",

        /*---------20-23------------*/
        "FILLER 22", "Hahahaha! Silly virus! What kind of virus doesn't know that", "its \"termination boots\" destroy things of equal color?",
        "Obviously a dumb one who doesn't know pressing TAB changes its boots' color.", "Now I'm embarrased you're following me...",

        /*---------24-25------------*/
        "FILLER 27", "Oh no! Vbots... how am I going to get past them?", "Hey, buddy ol' pal. Mind helping me out with these things?",

        /*---------26-27-----------*/
        "FILLER 30", "Hey! You can hack those things?! What does the red code say?",

        /*------------------------*/
        "FILLER 32", "Hey, you can talk, but what you said doesn't make sense. Try again!", "This is so easy. Just type what you see in red back to me verbatim in the same case.",

        /*------------------------*/
        "FILLER 35", "Hmm... it sounds like that \"code\" turns that Vbot blue!", "I wonder how we'd kill it, though...", "Maybe you can do something with your \"termination boots\"?",

        /*------------------------*/
        "FILLER 39", "Thanks for helping but you must be from another dimension!", "There's no other way someone can read another object's code! It's impossible!",
        "Look, even though I don't want to conform to that \"one guy\" who needs help,", "I'm going to fess up. I need help.", "But I tell you what. If you take me to the CPU, I won't report you.",
        "Sound like a deal?", "...", "Oh yeah, you don't talk unless it's totally necessary. Got it. Well, let's go!"
    };
    private int dialogueIndex = 0;
    private int index = 0;

	// Use this for initialization
	void Start () {
        checkForName = 1;
        firstTutorialIndex = 11;
        secondTutorialIndex = 16;
        thirdTutorialIndex = 22;
        fourthTutorialIndex = 27;
        fifthTutorialIndex = 30;
        checkForHack = 31;
        checkForDeath = 39;
        finishTutorial = 48;
        transform.position = intelliLocation.transform.position;
        y0 = this.transform.position.y;
        wait = false;
        idle = true;
        zoomIn = true;
        canBeIdle = true;
       // counter();
    }
	
	// Update is called once per frame
	void Update () {
        if(talking && Input.GetKeyDown(KeyCode.Space) && !wait)
        {
            if(index < dialogue[dialogueIndex].Length)
            {
                index = dialogue[dialogueIndex].Length;
            }
            else
            {
                NextText();
            }
        } 

        if (talking)
        {
            dialogueBox.SetActive(true);
        }
        else
        {
            dialogueBox.SetActive(false);
        }

        if(dialogueIndex < dialogue.Length)
        {
            if (index <= dialogue[dialogueIndex].Length) {
                dialogueBox.transform.Find("spacebar image").gameObject.SetActive(false);

                if (dialogueIndex == 2)
                {
                    dialogue[2] = PlayerStats.playerName.ToUpper() + "? That is an interesting variable name.";
                    dialogueBox.GetComponentInChildren<Text>().text = dialogue[2].Substring(0, index);
                }
                else {
                    dialogueBox.GetComponentInChildren<Text>().text = dialogue[dialogueIndex].Substring(0, index);
                }
            }
            else
            {
                if (dialogueIndex == checkForName)
                {
                    wait = true;
                    namePrompt.SetActive(true);
                    namePrompt.GetComponent<InputField>().Select();
                } else if(dialogueIndex == checkForHack)
                {
                    wait = true;
                    mouseClickPrompt.SetActive(false);
                    hackPrompt.SetActive(true);
                    hackPrompt.GetComponent<InputField>().Select();
                }

                if(!wait)
                    dialogueBox.transform.Find("spacebar image").gameObject.SetActive(true);
            }
        }

        if(interval % 3 == 0)
            index++;

        interval++;

        if (dialogueIndex == firstTutorialIndex)
        {
            if (firstTutorialObjective != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, firstTutorialObjective.transform.position, 4 * Time.deltaTime);
                if (transform.position.x >= firstTutorialObjective.transform.position.x)
                    y0 = firstTutorialObjective.transform.position.y;
            }
            talking = false;
            canBeIdle = false;
        }
        else if (dialogueIndex == secondTutorialIndex)
        {
            if (secondTutorialObjective != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, secondTutorialObjective.transform.position, 6 * Time.deltaTime);
                if (transform.position.x >= secondTutorialObjective.transform.position.x)
                    y0 = secondTutorialObjective.transform.position.y;
            }
            talking = false;
            canBeIdle = false;
        }
        else if (dialogueIndex == thirdTutorialIndex)
        {
            if (thirdTutorialObjective != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, thirdTutorialObjective.transform.position, 4 * Time.deltaTime);
                if (transform.position.x >= thirdTutorialObjective.transform.position.x)
                    y0 = thirdTutorialObjective.transform.position.y;
            }
            talking = false;
            canBeIdle = false;
        }
        else if (dialogueIndex == fourthTutorialIndex)
        {
            if (fourthTutorialObjective != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, fourthTutorialObjective.transform.position, 4 * Time.deltaTime);
                if (transform.position.x >= fourthTutorialObjective.transform.position.x)
                    y0 = fourthTutorialObjective.transform.position.y;
            }
            talking = false;
            canBeIdle = false;
        }
        else if (dialogueIndex == fifthTutorialIndex)
        {
            mouseClickPrompt.SetActive(true);
            wait = true;
            talking = false;
            canBeIdle = false;
        } else if(dialogueIndex == checkForDeath)
        {
            talking = false;
        }
        else if (dialogueIndex == finishTutorial)
        {
            if(levelTitle != null)
                levelTitle.SetActive(true);
            talking = false;
            idle = false;
            zoomIn = true;
        }



        if (canBeIdle)
        {
            if (idle)
                transform.position = new Vector2(transform.position.x, y0 + amplitude * Mathf.Sin(speed * Time.time));
            else if (!idle && zoomIn)
                ZoomIntoPlayer();
            else if (!idle && !zoomIn)
                ZoomOutPlayer();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit && hit.collider.name == "TutorialEnemy" && dialogueIndex < 32)
            {
                botClicked();
            }
        }
    }

    public void NextText()
    {
        index = 0;
        dialogueIndex++;
    }

    public void ZoomIntoPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 10 *Time.deltaTime);
        if(transform.localScale.x > 0)
            transform.localScale += Vector3.one * -.01f;
    }

    public void ZoomOutPlayer()
    { 
        transform.position = Vector2.MoveTowards(transform.position, intelliLocation.transform.position, 3* Time.deltaTime);
        if (transform.localScale.x < .25f)
            transform.localScale += Vector3.one * .01f;
        else
        {
            y0 = this.transform.position.y;
            transform.localScale = new Vector3(.25f, .25f, 1);
            idle = true;
        }
    }

    public void StartSecondTutorial()
    {
        talking = true;
        canBeIdle = true;
        index = 0;
        dialogueIndex = 12;
    }

    public void StartThirdTutorial()
    {
        talking = true;
        canBeIdle = true;
        index = 0;
        dialogueIndex = 17;
    }

    public void StartFourthTutorial()
    {
        talking = true;
        canBeIdle = true;
        index = 0;
        dialogueIndex = 23;
    }

    public void StartFifthTutorial()
    {
        talking = true;
        canBeIdle = true;
        index = 0;
        dialogueIndex = 28;
    }

    public void InputtedName()
    {
        wait = false;
        index = 0;
        dialogueIndex = 2;
        PlayerStats.playerName = namePrompt.transform.Find("name").GetComponent<Text>().text;
        namePrompt.SetActive(false);
    }

    public void InputtedHack()
    {
        string inputtedCode = hackPrompt.transform.Find("code").GetComponent<Text>().text;
        if(inputtedCode == "System.body(Color.BLUE);")
        {
            wait = false;
            index = 0;
            dialogueIndex = 36;
            hackPrompt.SetActive(false);
        }
        else {
            index = 0;
            if(dialogueIndex == 33)
            {
                dialogueIndex = 34;
            }
            else
            {
                dialogueIndex = 33;
            }

            hackPrompt.transform.Find("code").GetComponent<Text>().text = "";

        }
    }

    public void botClicked()
    {
        talking = true;
        canBeIdle = true;
        wait = false;
        dialogueIndex = 31;
        index = 0;
    }

    public void botKilled()
    {
        talking = true;
        canBeIdle = true;
        wait = false;
        dialogueIndex = 40;
        index = 0;
        Destroy(fourthObjectiveBarrier);
    }
    //public IEnumerator counter() {
    //    for (int i = 0; i < 6; i++)
    //    {
    //        yield return new WaitForSeconds(3);
    //        idle = false;
    //        zoomIn = true;
    //        yield return new WaitForSeconds(5);
    //        idle = false;
    //        zoomIn = false;
    //    }
    //}
}

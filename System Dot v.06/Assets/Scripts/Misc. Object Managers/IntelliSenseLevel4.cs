using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using UnityEngine.UI;

public class IntelliSenseLevel4 : IntelliSense
{

    float moveSpeed = 7;

    [Header("In-Game Objects")]
    public GameObject mouseClickPrompt;
    public Sprite intelliSenseSprite;
    private GameObject terminalWindow;

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
            } else if(whatToSay[dialogueIndex].who == "IntelliSense")
            {
                base.SetCharacterIcon(intelliSenseSprite);
            }
        }

        // THIS SECTION CHECKS IF ENEMY HAS BEEN CLICKED FOR TUTORIAL PURPOSES
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            //     string variabullText = "";



            if (hit && hit.collider.name == "whileTutorial" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce)
            {
                hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce = true;
                SetDialogue("firstWhileLoop");
            } else if(hit && hit.collider.name == "TutorialSpawner" && !hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce)
            {
                hit.collider.gameObject.GetComponent<EnemyTerminal>().clickOnce = true;
                SetDialogue("firstSpawner");
            }
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntelliSenseBoss2 : IntelliSense {

    private GameObject terminalWindow;

	// Use this for initialization
	void Start () {
        base.Start();
        
        SetDialogue("startBoss");
        
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
	void Update () {
        base.Update();

        // THIS SECTION IS TO MAKE INTELLISENSE MOVE UP AND DOWN PERIODICALLY
        if (whatToSay != null && PlayerStats.checkpoint == "Checkpoint1" && talking && (dialogueIndex < whatToSay.Count || !eventName.Contains("moveTo")))
        {
            transform.position = new Vector2(transform.position.x, y0 + amplitude * Mathf.Sin(speed * Time.time));
        }
    }

    public override void SetDialogue(string message)
    {
        base.SetCharacterIcon(this.GetComponent<SpriteRenderer>().sprite);
        base.SetDialogue(message);
    }
}

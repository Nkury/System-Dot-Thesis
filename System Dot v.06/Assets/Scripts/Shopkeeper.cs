using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : Dialogue {

    bool explain;
    // Use this for initialization
    protected new void Start()
    {
        explain = true;
        base.Start();
    }


    public override void SetDialogue(string keyWord)
    {
        if (explain)
        {
            explain = false;
            base.SetCharacterIcon(this.GetComponent<SpriteRenderer>().sprite);
            base.SetDialogue(keyWord);
        }
    }

    public void CanStartSpeakingAgain()
    {
        explain = true;
        dialogueIndex++;
        index = 0;
    }
}

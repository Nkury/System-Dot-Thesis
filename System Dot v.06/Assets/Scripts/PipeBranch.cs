using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeBranch : MonoBehaviour {

    public GameObject trueBranch;
    public GameObject falseBranch;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        // if this pipe branche's terminal window is open
        if (this.GetComponent<EnemyTerminal>() && this.GetComponent<EnemyTerminal>().localTerminalMode >= 2)
        {
            if (trueBranch)
            {
                trueBranch.GetComponent<SpriteRenderer>().color = new Color(92 / 255f, 208 / 255f, 126 / 255f); // green tint
            }

            if (falseBranch)
            {
                falseBranch.GetComponent<SpriteRenderer>().color = new Color(212 / 255f, 117 / 255f, 117 / 255f); // red tint
            }
        }
        else
        {
            if (trueBranch)
            {
                trueBranch.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1); // return to original color
            }

            if (falseBranch)
            {
                falseBranch.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1); // return to original color
            }
        }
	}

}

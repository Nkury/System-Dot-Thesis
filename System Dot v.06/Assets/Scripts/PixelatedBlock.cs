using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelatedBlock : MonoBehaviour {

    public float timeToPixelate;
    public float timeToRecover;

    bool recover = false;
    float time = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if(!recover && time >= timeToPixelate)
        {
            time = 0;
            this.GetComponent<BoxCollider2D>().enabled = false;
            recover = true;
            this.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
        }
        else
        {
            if(recover && time >= timeToRecover)
            {
                time = 0;
                this.GetComponent<BoxCollider2D>().enabled = true;
                recover = false;
                this.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);      
            }
        }
	}
}

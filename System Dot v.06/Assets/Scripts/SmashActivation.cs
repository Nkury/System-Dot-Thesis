using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashActivation : Activation {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void activate()
    {
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GameObject.FindObjectOfType<Level2BossBehavior>().SetUp();
    }

    public override void deactivate()
    {
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
}

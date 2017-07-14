using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorActivation : Activation {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void activate()
    {
        this.gameObject.SetActive(false);
    }

    public override void deactivate()
    {
        this.gameObject.SetActive(true);
    }
}

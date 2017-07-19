using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

    public float timeToDestroy;

    float timeTaken;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        timeTaken += Time.deltaTime;
        if(timeTaken > timeToDestroy)
        {
            Destroy(this.gameObject);
        }
	}
}

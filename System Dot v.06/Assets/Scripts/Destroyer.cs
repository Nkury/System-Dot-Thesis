using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Destroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    foreach(string name in PlayerStats.deadObjects)
        {
            Destroy(GameObject.Find(name));
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

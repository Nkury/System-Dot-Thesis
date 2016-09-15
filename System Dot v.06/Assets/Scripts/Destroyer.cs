using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(PlayerStats.deadObjects.Count);
	    foreach(string name in PlayerStats.deadObjects)
        {
            Destroy(GameObject.Find(name));
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

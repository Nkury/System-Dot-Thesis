using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOverSprite : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.GetComponent<MeshRenderer>().sortingLayerName = "Default";
        this.GetComponent<MeshRenderer>().sortingOrder = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

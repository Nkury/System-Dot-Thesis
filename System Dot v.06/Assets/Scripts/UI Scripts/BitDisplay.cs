using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class BitDisplay : MonoBehaviour {

    Text txt;
    bool mouseOver;
    
	// Use this for initialization
	void Start () {
        txt = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (mouseOver) {          
            txt.text = System.Convert.ToString(PlayerStats.bitsCollected, 2);
        }
        else {
            txt.text = PlayerStats.bitsCollected.ToString();
        }
	}

    public void OnMouseEnter()
    {
        mouseOver = true;
    }

    public void OnMouseExit()
    {
        mouseOver = false;
    }
}

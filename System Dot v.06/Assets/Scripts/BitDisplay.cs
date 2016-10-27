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
        if (!mouseOver)
        {          
            txt.text = System.Convert.ToString(PlayerStats.bitsCollected, 2);
            Debug.Log(txt.text);
        }
	}

    public void OnMouseEnter()
    {
        mouseOver = true;
        txt.text = Convert.ToInt64(txt.text, 2).ToString();
        Debug.Log(txt.text);
    }

    public void OnMouseExit()
    {
        mouseOver = false;
    }
}

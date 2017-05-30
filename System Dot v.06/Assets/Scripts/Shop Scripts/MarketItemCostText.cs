using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class MarketItemCostText : MonoBehaviour {

    public int itemPrice;
    Text txt;
    bool mouseOver;
	// Use this for initialization
	void Start () {
        txt = GetComponent<Text>();
       
	}
	
	// Update is called once per frame
	void Update () {
        if (!mouseOver)
        {
            txt.text = System.Convert.ToString(itemPrice, 2) + " Bits";
        }
        else
        {
            txt.text = itemPrice.ToString() + " Bits";
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

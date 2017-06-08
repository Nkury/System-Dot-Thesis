﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class APISystem : MonoBehaviour {

    public GameObject clickAPI;

    public GameObject APImenu;
    public GameObject SystemHelp;

    // System code
    public GameObject colorInfo;
    public GameObject directionInfo;
    public GameObject chestInfo;
    public Image arrowIndicator;

    public static bool clicked = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void APIButtonClicked()
    {
        /* LOGGER INFORMATION */
        if(!APImenu.activeSelf)
            PlayerStats.numOfAPIUses++;
 
        clickAPI.GetComponent<RectTransform>().anchoredPosition = new Vector2(60, -30);
        APImenu.SetActive(!APImenu.activeSelf);
    }

    public void SystemButtonClicked()
    {
        if (!clicked)
        {
            clicked = true;
            clickAPI.SetActive(false);
            GameObject.Find("Intellisense").GetComponent<IntelliSenseTest>().SetDialogue("APIClicked");
        }
        SystemHelp.SetActive(!SystemHelp.activeSelf);
    }

    public void ColorButtonClicked()
    {
        colorInfo.SetActive(true);
        directionInfo.SetActive(false);
        chestInfo.SetActive(false);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 514.7f);
    }

    public void DirectionButtonClicked()
    {
        colorInfo.SetActive(false);
        directionInfo.SetActive(true);
        chestInfo.SetActive(false);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 463.67f);
    }

    public void ChestButtonClicked()
    {
        colorInfo.SetActive(false);
        directionInfo.SetActive(false);
        chestInfo.SetActive(true);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 414.4f);
    }
}
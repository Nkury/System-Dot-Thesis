using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class APISystem : MonoBehaviour {

    public GameObject clickAPI;

    public GameObject APImenu;
    

    // System code
    [Header ("Info")]
    public GameObject colorInfo;
    public GameObject directionInfo;
    public GameObject chestInfo;
    public GameObject smashInfo;
    public GameObject intInfo;
    public GameObject doubleInfo;
    public GameObject stringInfo;
    public GameObject booleanInfo;
    public Image arrowIndicator;

    [Header("Buttons")]
    public GameObject SystemHelp;
    public GameObject DataTypeHelp;

    public static bool clicked = true;

    private List<GameObject> listOfIcons = new List<GameObject>();
    private List<GameObject> listOfHelps = new List<GameObject>();

    // Use this for initialization
    void Start () {
        listOfIcons.Add(colorInfo);
        listOfIcons.Add(directionInfo);
        listOfIcons.Add(chestInfo);
        listOfIcons.Add(smashInfo);
        listOfIcons.Add(intInfo);
        listOfIcons.Add(doubleInfo);
        listOfIcons.Add(stringInfo);
        listOfIcons.Add(booleanInfo);

        listOfHelps.Add(SystemHelp);
        listOfHelps.Add(DataTypeHelp);
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
        ActivateAPIButton(colorInfo);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 514.7f);
    }

    public void DirectionButtonClicked()
    {
        ActivateAPIButton(directionInfo);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 463.67f);
    }

    public void ChestButtonClicked()
    {
        ActivateAPIButton(chestInfo);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 414.4f);
    }

    public void SmashButtonClicked()
    {
        ActivateAPIButton(smashInfo);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 354.39f);
    }   

    public void IntButtonClicked()
    {
        ActivateAPIButton(intInfo);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 514.7f);
    }

    public void DoubleButtonClicked()
    {
        ActivateAPIButton(doubleInfo);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 463.67f);
    }

    public void StringButtonClicked()
    {
        ActivateAPIButton(stringInfo);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 414.4f);
    }

    public void BooleanClicked()
    {
        ActivateAPIButton(booleanInfo);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 354.39f);
    }

    public void ActivateAPIButton(GameObject iconClicked)
    {
        iconClicked.SetActive(true);
        foreach(GameObject icon in listOfIcons)
        {
            if(icon != iconClicked)
            {
                icon.SetActive(false);
            }
        }
    }

    public void ActivateHelpButton(GameObject helpClicked)
    {
        helpClicked.SetActive(true);
        foreach(GameObject icon in listOfHelps)
        {
            if(helpClicked != icon)
            {
                icon.SetActive(false);
            }
        }
    }
}

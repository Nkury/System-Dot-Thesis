using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class APISystem : MonoBehaviour {

    public GameObject APImenu;
    public GameObject SystemHelp;

    // System code
    public GameObject colorInfo;
    public GameObject directionInfo;
    public Image arrowIndicator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void APIButtonClicked()
    {
        APImenu.SetActive(!APImenu.activeSelf);
    }

    public void SystemButtonClicked()
    {
        SystemHelp.SetActive(!SystemHelp.activeSelf);
    }

    public void ColorButtonClicked()
    {
        colorInfo.SetActive(true);
        directionInfo.SetActive(false);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 514.7f);
    }

    public void DirectionButtonClicked()
    {
        colorInfo.SetActive(false);
        directionInfo.SetActive(true);
        arrowIndicator.rectTransform.localPosition = new Vector2(11.81049f, 463.67f);
    }
}

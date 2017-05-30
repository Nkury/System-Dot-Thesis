using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DemoStats : MonoBehaviour {

    public Text[] uiText; // 0- time, 1- bits, 2- deaths

	// Use this for initialization
	void Start () {
        int seconds = (int)PlayerStats.totalSecondsOfPlaytime % 60;
        int minutes = (int)PlayerStats.totalSecondsOfPlaytime / 60;

        uiText[0].text = string.Format("{0:00}:{1:00}", minutes, seconds);
        uiText[1].text = PlayerStats.bitsCollected.ToString();
        uiText[2].text = PlayerStats.numberOfDeaths.ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

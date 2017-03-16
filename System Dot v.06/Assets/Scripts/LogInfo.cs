using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogInfo : MonoBehaviour {

    public Text avgEditTime;
    public Text mostEditTime;
    public Text avgBackspaces;
    public Text mostBackspaces;
    public Text avgDeletes;
    public Text mostDeletes;
    public Text avgClicks;
    public Text mostClicks;
    public Text avgInactivity;
    public Text mostInactivity;
    public Text numFailures;
    public Text numPerfects;
    public Text numAPI;
    public Text numF5s;
    public Text numEdits;
    public Text typingSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        avgEditTime.text = PlayerStats.averageTimeOnEditing.ToString();
        mostEditTime.text = PlayerStats.longestTimeOnEditing.ToString();
        avgBackspaces.text = PlayerStats.averageNumberOfBackspaces.ToString();
        mostBackspaces.text = PlayerStats.mostNumberOfBackspaces.ToString();
        avgDeletes.text = PlayerStats.averageNumberOfDeletes.ToString();
        mostDeletes.text = PlayerStats.mostNumberOfDeletes.ToString();
        avgClicks.text = PlayerStats.averageNumberofMouseClicks.ToString();
        mostClicks.text = PlayerStats.mostNumberofMouseClicks.ToString();
        avgInactivity.text = PlayerStats.averageTimeOfMouseInactivity.ToString();
        mostInactivity.text = PlayerStats.mostTimeofMouseInactivity.ToString();
        numFailures.text = PlayerStats.mostNumberofAttempts.ToString();
        numPerfects.text = PlayerStats.numberOfPerfectEdits.ToString();
        numAPI.text = PlayerStats.numOfAPIUses.ToString();
        numF5s.text = PlayerStats.numOfF5.ToString();
        numEdits.text = PlayerStats.numOfEdits.ToString();
        typingSpeed.text = PlayerStats.typingSpeed.ToString();
	}
}

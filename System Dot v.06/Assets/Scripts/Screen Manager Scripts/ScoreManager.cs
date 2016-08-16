using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public static int score;

	Text text;

	void Start()
	{
        score = PlayerStats.bitsCollected;
		text = GetComponent<Text> ();

		score = PlayerPrefs.GetInt ("CurrentScore");
	}

	void Update()
	{
		if (score < 0) 
		{
			score = 0;
		}

        text.text = "" + PlayerStats.bitsCollected;
	}
   
}

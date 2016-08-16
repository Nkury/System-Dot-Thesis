using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SimonScoreManager : MonoBehaviour {

    private int score;

    public Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = "" + score;
	}

    public void addPoint()
    {
        score++;
    }

    public void subtractPoint()
    {
        score--;
        if(score < 0)
        {
            score = 0;
        }
    }
}

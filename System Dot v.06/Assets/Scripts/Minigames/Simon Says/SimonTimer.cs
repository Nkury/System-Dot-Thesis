using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SimonTimer : MonoBehaviour {

    public float timeAlloted;
    private float countDown;

    private bool timeOver;

    public Text text;

    public float speed;
    public float fastForward;
	
    // Use this for initialization
	void Start () {
        countDown = timeAlloted;
        timeOver = false;

        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKey(KeyCode.Return))
        {
            speed = fastForward;
        }
        else
        {
            speed = 1;
        }

        if (!timeOver)
        {
            countDown -= Time.deltaTime * speed;
        }
      
        if(countDown <= 0)
        {
            timeOver = true;
        }

        text.text = "" + Mathf.Round(countDown);

	}

    public void resetTime()
    {
        countDown = timeAlloted;
        timeOver = false;
    }

    public bool isTimeOver()
    {
        return timeOver;
    }
}

using UnityEngine;
using System.Collections;

public class PlayerFeedbackManager : MonoBehaviour {

    public GameObject wrongAnswer;
    public GameObject rightAnswer;

    public float displayTime;
    private float countdown;
    

	// Use this for initialization
	void Start () {
        countdown = 0;
        wrongAnswer.SetActive(false);
        rightAnswer.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	    if(countdown > 0)
        {
            countdown -= Time.deltaTime;
        }

        if(countdown <= 0)
        {
            rightAnswer.SetActive(false);
            wrongAnswer.SetActive(false);
        }
	}

    public void provideFeedback(bool answer)
    {
        if(answer)
        {
            rightAnswer.SetActive(true);
            countdown = displayTime;
        }

        else
        {
            wrongAnswer.SetActive(true);
            countdown = displayTime;
           
        }
    }
}

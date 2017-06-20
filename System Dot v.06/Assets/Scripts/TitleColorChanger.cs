using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TitleColorChanger : MonoBehaviour {
    public Gradient[] myGradient = new Gradient[5];
    public float strobeDuration = 2f;

    private float timeElapsed;
    public int gradientSelection;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (timeElapsed > strobeDuration)
        {
            timeElapsed = 0;
            gradientSelection--;
            if(gradientSelection == -1)
            {
                gradientSelection = 4;
            }
            gradientSelection %= myGradient.Length;
        }

        timeElapsed += Time.deltaTime;
        float t = Mathf.PingPong(Time.time / strobeDuration, 1f);
        this.GetComponent<Text>().color = myGradient[gradientSelection].Evaluate(t);


    }
}

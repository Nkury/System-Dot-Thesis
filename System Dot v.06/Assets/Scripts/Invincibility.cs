using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour {

    public static bool invincibility = false;
    public static bool invincibilityOnce = false;

    public int blinks;
    public float tOn;
    public float tOff;
    int index = 0;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {        
        if (invincibility && invincibilityOnce)
        {
            StartCoroutine(Blink(blinks, tOn, tOff));
            invincibilityOnce = false;
        } 
	}

    IEnumerator Blink(int nTimes, float timeOn, float timeOff)
    {
        while (nTimes > 0)
        {
            this.transform.FindChild("Player").GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(timeOn);
            this.transform.FindChild("Player").GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(timeOff);
            nTimes--;
        }

        this.transform.FindChild("Player").GetComponent<Renderer>().enabled = true;
        invincibility = false;
    }
}

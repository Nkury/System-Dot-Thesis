using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : triggerZoneScript {

    public GameObject slider;

    public float initialPower;
    public float remainingPower;
    public float intervalToAdd = 1;

    public float secondsToCharge;
    public float secondsToDeplete;

    private bool chargeCheck = true;
    private bool startToDeplete = false;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if(startToDeplete && chargeCheck)
        {
            StartCoroutine(Deplete());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Escort")
        {
            startToDeplete = false;
            slider.GetComponent<IntelliSenseCharge>().AddCharge((int)intervalToAdd);
            if (remainingPower <= 0)
            {
                other.GetComponent<EscortIntelliSense>().isCharging = false;
                other.GetComponent<EscortIntelliSense>().playButton.SetActive(true);
                other.GetComponent<EscortIntelliSense>().pauseButton.SetActive(false);
                PlayerStats.deadObjects.Add(door.name);
                Destroy(door);
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Escort" && chargeCheck)
        {
            other.GetComponent<EscortIntelliSense>().isCharging = true;
            startToDeplete = false;
            StartCoroutine(Charge());
            if (remainingPower <= 0)
            {
                other.GetComponent<EscortIntelliSense>().isCharging = false;
                other.GetComponent<EscortIntelliSense>().playButton.SetActive(true);
                other.GetComponent<EscortIntelliSense>().pauseButton.SetActive(false);
                PlayerStats.deadObjects.Add(door.name);
                Destroy(door);
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Escort")
        {
            other.GetComponent<EscortIntelliSense>().isCharging = false;
            startToDeplete = true;
            chargeCheck = true;         
        }
    }

    public IEnumerator Charge()
    {
        chargeCheck = false;
        yield return new WaitForSeconds(secondsToCharge);
        slider.GetComponent<IntelliSenseCharge>().AddCharge((int)intervalToAdd);
        remainingPower -= intervalToAdd;
        chargeCheck = true;
    }

    public IEnumerator Deplete()
    {
        chargeCheck = false;
        yield return new WaitForSeconds(secondsToDeplete);
        slider.GetComponent<IntelliSenseCharge>().AddCharge(-(int)intervalToAdd);
        remainingPower += intervalToAdd;
        chargeCheck = true;
    }
}


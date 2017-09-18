using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntelliSenseCharge : MonoBehaviour {

    public int totalCharge;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddCharge(int charge)
    {
        if(this.GetComponent<Slider>().value <= totalCharge)
        {
            this.GetComponent<Slider>().value += charge;
        }
    }
}

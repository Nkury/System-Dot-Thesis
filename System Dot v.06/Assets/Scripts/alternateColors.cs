using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class alternateColors : MonoBehaviour {

    public bool isBoot;
    public Sprite[] bootColors = new Sprite[3];
    int bootInc = 0;

    int interval = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(interval % 60 == 0)
        {
            
            if (isBoot)
            {

                if (bootInc <= 2)
                {
                    this.gameObject.GetComponent<Image>().sprite = bootColors[bootInc];
                    bootInc++;
                }
                else
                    bootInc = 0;
            }
        }
        interval++;	
	}
}

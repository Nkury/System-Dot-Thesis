using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class alternateColors : MonoBehaviour {

    public bool isBoot;
    public bool isBit;
    public Sprite[] bootColors = new Sprite[3];
    public Sprite[] bitColors = new Sprite[2];
    int inc = 0;

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

                if (inc <= 2)
                {
                    this.gameObject.GetComponent<Image>().sprite = bootColors[inc];
                    inc++;
                }
                else
                    inc = 0;
            } else if (isBit)
            {
                if (inc <= 1)
                {
                    this.gameObject.GetComponent<Image>().sprite = bitColors[inc];
                    inc++;
                }
                else
                    inc = 0;
            }
        }
        interval++;	
	}
}

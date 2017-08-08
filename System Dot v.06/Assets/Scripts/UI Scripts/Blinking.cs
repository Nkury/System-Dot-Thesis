using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Blinking : MonoBehaviour {

    int interval = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        interval++;
        if (interval % 20 == 0)
        {
            if (this.GetComponent<Image>())
                this.GetComponent<Image>().enabled = !this.GetComponent<Image>().enabled;
            else if(this.GetComponent<SpriteRenderer>())
            {
                this.GetComponent<SpriteRenderer>().enabled = !this.GetComponent<SpriteRenderer>().enabled;
            } else if (this.GetComponent<MeshRenderer>())
            {
                this.GetComponent<MeshRenderer>().enabled = !this.GetComponent<MeshRenderer>().enabled;
            }
        }
    }
}

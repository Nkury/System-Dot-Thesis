using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Virus : MonoBehaviour {

    public static int numOfViruses;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponent<SpriteRenderer>())
        {
            this.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.magenta, Mathf.PingPong(Time.time, 1));
        } else if (this.GetComponent<Image>())
        {
            this.GetComponent<Image>().color = Color.Lerp(Color.red, Color.magenta, Mathf.PingPong(Time.time, 1));

            // take the count label of the UI object and replace with # of viruses. We -1 because we are subtracting the UI virus object
            numOfViruses = GameObject.FindObjectsOfType<Virus>().Length - 1;
            this.gameObject.transform.parent.FindChild("Count").GetComponent<Text>().text = numOfViruses.ToString();
        }
    }

    //void OnMouseOver()
    //{
    //    if (this.GetComponent<SpriteRenderer>())
    //    {
    //        Cursor.visible = false;
    //    }
    //}

    //void OnMouseExit()
    //{
    //    if (this.GetComponent<SpriteRenderer>())
    //    {
    //        Cursor.visible = true;
    //    }
    //}
}

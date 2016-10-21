using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BitToolTip : MonoBehaviour {

    public Text txt;
	// Use this for initialization
	void Start () {
        txt.text = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseEnter()
    {
        txt.text = "HELLO";
    }

    void OnMouseExit()
    {
        txt.text = "";
    }
}

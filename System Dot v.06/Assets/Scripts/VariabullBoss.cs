using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariabullBoss : Variabull {

    public Text variabullText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(variabullText.text != this.statement)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
            this.GetComponent<CircleCollider2D>().enabled = true;
            this.transform.FindChild("Help").GetComponent<MeshRenderer>().enabled = true;
        }
	}

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("Main HUD").GetComponent<TerminalWindowUI>().setVariabullCode(statement);
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<CircleCollider2D>().enabled = false;
            this.transform.FindChild("Help").GetComponent<MeshRenderer>().enabled = false;
        }
    }
}

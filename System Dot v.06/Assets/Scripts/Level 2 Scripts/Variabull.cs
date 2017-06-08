using UnityEngine;
using System.Collections;

public class Variabull : MonoBehaviour {

    public string statement;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject.Find("Main HUD").GetComponent<TerminalWindowUI>().setVariabullCode(statement);
            Destroy(this.gameObject);
        }
    }
}

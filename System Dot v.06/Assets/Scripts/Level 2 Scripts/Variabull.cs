using UnityEngine;
using System.Collections;

public class Variabull : MonoBehaviour {

    public static string statement;
    public static bool taken = false;

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
            taken = true;
            Destroy(this);
        }
    }
}

using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(this.gameObject.name == "teleporter" && other.gameObject.tag == "Enemy")
        {
            other.gameObject.transform.position = new Vector2(188.241f, 27.5216f);
        } else if(this.gameObject.name == "teleporter2" || this.gameObject.name == "teleporter3" && other.gameObject.tag == "Enemy")
        {
            other.gameObject.transform.position = new Vector2(200.771f, 21.5816f);
        }
    }
}

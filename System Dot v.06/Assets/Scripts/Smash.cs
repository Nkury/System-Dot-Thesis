using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : MonoBehaviour {

    Vector3 startingPosition;
    bool goUp;
	// Use this for initialization
	void Start () {
        startingPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (goUp)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, startingPosition, 2 * Time.deltaTime);
        }

        if(this.transform.position.y >= startingPosition.y)
        {
            goUp = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            this.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 0);
            goUp = true;                     
        }
    }
}

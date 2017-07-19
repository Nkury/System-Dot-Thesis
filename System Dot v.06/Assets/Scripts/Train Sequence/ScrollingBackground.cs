using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {
    public float speed;
    public float stopPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (this.transform.position.x > stopPos)
        {
            float step = speed * Time.deltaTime;
            transform.position = new Vector2(this.transform.position.x - step, transform.position.y);
        }
	}
}

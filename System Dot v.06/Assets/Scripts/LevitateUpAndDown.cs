using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitateUpAndDown : MonoBehaviour {

    float y0;
    float amplitude = .2f;
    float speed = 1.5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(transform.position.x, transform.position.y + y0 + amplitude * Mathf.Sin(speed * Time.time));
    }
}

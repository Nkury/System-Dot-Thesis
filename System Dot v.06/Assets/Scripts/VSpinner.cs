using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VSpinner : MonoBehaviour {

    public float rotationSpeed;
    public float maxRotation;
    public bool pause;

    private float maxRotated = 0f;
   

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!pause)
        {
            float rotated = rotationSpeed * Time.deltaTime;
            this.transform.Rotate(new Vector3(this.transform.rotation.x, this.transform.rotation.y, rotated));
            maxRotated += Math.Abs(rotated);

            if(maxRotated >= maxRotation)
            {
                pause = true;
            }
        }
	}
}

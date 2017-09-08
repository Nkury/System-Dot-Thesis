using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rotater : MonoBehaviour {

    public float rotationSpeed;
    public float maxRotation;
    public bool pause;
    public bool goLeft = true; 

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

            if (goLeft)
            {
                rotationSpeed = Math.Abs(rotationSpeed);
                maxRotated += Math.Abs(rotated);
            }
            else
            {
                rotationSpeed = -Math.Abs(rotationSpeed);
                maxRotated -= Math.Abs(rotated);
            }
            
            if(goLeft && maxRotated >= maxRotation)
            {
                pause = true;
            } else if(!goLeft && maxRotated <= maxRotation)
            {
                pause = true;
            }
        }
	}

    public void RotateToSpecificPosition(int rotation)
    {
        this.transform.rotation = Quaternion.Euler(0, 0, rotation);
        pause = true;
    }
}

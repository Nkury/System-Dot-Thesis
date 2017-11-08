using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour {

    public GameObject blackHole;
    public float maxForce = 30.0f;
    public float maxDist = 5.0f;

    public void FixedUpdate()
    {
        Vector2 dir = blackHole.transform.position - this.transform.position;
        float frac = dir.magnitude / maxDist;
        if(frac < 1.0)
        {
            this.GetComponent<Rigidbody2D>().AddForce(dir.normalized * maxForce * (1.0f - frac));
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "black_hole")
        {
            Destroy(this.gameObject);
        }
    }
}

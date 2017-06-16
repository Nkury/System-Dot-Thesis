using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vlail : MonoBehaviour {

    public float timeToLive;
    public float rotationSpeed;

    private float timeAlive;

	// Use this for initialization
	void Start () {
		
	}

    int z = 0;
	// Update is called once per frame
	void Update () {
        timeAlive += Time.deltaTime;
        this.transform.Rotate(0, 0, rotationSpeed);

        if (timeAlive > timeToLive) {
            Destroy(this.gameObject);
        }
        //this.GetComponent<Rigidbody2D>().angularVelocity += 1;
        //Vector2 dir = this.GetComponent<Rigidbody2D>().velocity;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);   
	}
}

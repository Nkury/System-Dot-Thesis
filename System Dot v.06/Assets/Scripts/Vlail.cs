using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vlail : MonoBehaviour {

    public float timeToLive;
    public float rotationSpeed;
    public bool isObstacle;

    private float timeAlive;
    private Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = this.transform.position;
	}

    int z = 0;
    // Update is called once per frame
    void Update() {        
        //this.transform.Rotate(0, 0, rotationSpeed);
        if (timeToLive != -1)
        {
            timeAlive += Time.deltaTime;
            if (timeAlive > timeToLive)
            {
                Destroy(this.gameObject);
            }
        }

        if(this.transform.position.y > startingPos.y)
        {
            this.transform.position = startingPos;
        }
      
        //this.GetComponent<Rigidbody2D>().angularVelocity += 1;
        //Vector2 dir = this.GetComponent<Rigidbody2D>().velocity;
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);   
	}
}

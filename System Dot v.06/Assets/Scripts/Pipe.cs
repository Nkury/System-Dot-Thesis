using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour {

    public float speed;
    public GameObject branch;
    // position 0 = up, position 1 = left, position 2 = down, position 3 = right
    public GameObject[] pipeExits = new GameObject[4]; 

    Vector2 dir;
    GameObject PlayerBody;
    private bool atDestination = false;
    private bool finished = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerBody != null && dir != Vector2.zero && !finished)
        {
            // see which direction (x or y) player is coming from
            if (Math.Abs(dir.x) > Math.Abs(dir.y))
            {
                if (dir.x >= 0 ? branch.transform.position.x > PlayerBody.transform.position.x : branch.transform.position.x < PlayerBody.transform.position.x)
                {
                    PlayerBody.GetComponent<Transform>().position += new Vector3(dir.x * speed, 0);
                }
                else if(!atDestination)
                {
                    MoveToPipeExit();
                } else if (atDestination)
                {
                    PlayerBody.GetComponent<PlayerController>().pauseMovement = false;
                    PlayerBody.GetComponent<CircleCollider2D>().enabled = true;
                    PlayerBody.GetComponent<Rigidbody2D>().isKinematic = false;
                    PlayerBody.transform.FindChild("Player").gameObject.SetActive(true);
                    atDestination = false;
                    finished = true;
                }
            }
            else
            {
                if (dir.y >= 0 ? branch.transform.position.y > PlayerBody.transform.position.y : branch.transform.position.y < PlayerBody.transform.position.y)
                {
                    PlayerBody.GetComponent<Transform>().position += new Vector3(0, dir.y * speed);
                } else if (!atDestination)
                {
                    MoveToPipeExit();
                }
                else if (atDestination)
                {
                    PlayerBody.GetComponent<PlayerController>().pauseMovement = false;
                    PlayerBody.GetComponent<CircleCollider2D>().enabled = true;
                    PlayerBody.GetComponent<Rigidbody2D>().isKinematic = false;
                    PlayerBody.transform.FindChild("Player").gameObject.SetActive(true);
                    atDestination = false;
                    finished = true;
                }
            }
        }
    }

    void MoveToPipeExit()
    {
        switch ((int)branch.transform.rotation.z)
        {
            case 0: // up
                PlayerBody.transform.position += new Vector3(0, speed);
                atDestination = PlayerBody.transform.position.y > pipeExits[0].transform.position.y;
                break;
            case 90: // left
                PlayerBody.transform.position += new Vector3(-speed, 0);
                atDestination = PlayerBody.transform.position.x < pipeExits[1].transform.position.x;
                break;
            case 180: // down
                PlayerBody.transform.position += new Vector3(0, speed);
                atDestination = PlayerBody.transform.position.y < pipeExits[2].transform.position.y;
                break;
            case 270:
                PlayerBody.transform.position += new Vector3(-speed, 0);
                atDestination = PlayerBody.transform.position.x > pipeExits[3].transform.position.x;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerBody = other.gameObject;

            // Figure out what direction player enters the pipe
            dir = other.GetComponent<Rigidbody2D>().velocity;
            // Normalize the vector
            dir = dir.normalized;

            finished = false; // start the pipe journey

            PlayerBody.GetComponent<PlayerController>().pauseMovement = true;
            PlayerBody.GetComponent<CircleCollider2D>().enabled = false;
            PlayerBody.GetComponent<Rigidbody2D>().isKinematic = true;
            other.transform.FindChild("Player").gameObject.SetActive(false);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pipe : MonoBehaviour {

    public float speed;
    public List<GameObject> branches;
    public List<GameObject> pipeExits;
    
    Vector2 dir;
    GameObject PlayerBody;
    private bool atDestination = false;
    private bool finished = false;

    private GameObject atBranch; // stores game object of branch that we collide with
    private List<GameObject> backUpBranches; // since we are removing branches, this will store them for refresh

    // Use this for initialization
    void Start () {
        backUpBranches = new List<GameObject>(branches);
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerBody != null && dir != Vector2.zero && !finished)
        {
            // see which direction (x or y) player is coming from
            if (AtBranch(Math.Abs(dir.x) > Math.Abs(dir.y)))
            {
                PlayerBody.GetComponent<Transform>().position += new Vector3(dir.x * speed, dir.y * speed);           
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
                atBranch = null;
                branches = backUpBranches;
                atDestination = false;
                finished = true;
            }            
        }
    }

    public bool AtBranch(bool isX)
    {
        foreach(GameObject pipeExit in pipeExits)
        {
            if (isX) {
                // we are at the destination if the player is past the x coordinate of the pipe exit
                // and is within the y value range
                atDestination = ((dir.x >= 0 ?
                    PlayerBody.transform.position.x > pipeExit.transform.position.x :
                    PlayerBody.transform.position.x < pipeExit.transform.position.x) &&
                    PlayerBody.transform.position.y < pipeExit.transform.position.y + 1 &&
                    PlayerBody.transform.position.y > pipeExit.transform.position.y - 1);
            }
            else
            {
                // we are at the destination if the player is past the y coordinate of the pipe exit
                // and is within the x value range
                atDestination = ((dir.y >= 0 ?
                    PlayerBody.transform.position.y > pipeExit.transform.position.y :
                    PlayerBody.transform.position.y < pipeExit.transform.position.y) &&
                    PlayerBody.transform.position.x < pipeExit.transform.position.x + 1 &&
                    PlayerBody.transform.position.x > pipeExit.transform.position.x - 1);
            }
        }

        foreach(GameObject branch in branches)
        {
            if (isX) {
                dir.y = 0;  // when moving the player through the pipe, we don't want to move in y direction

                // if we are at the x value and the branch we are observing is at the same
                // y value, then return true. This is only if we are referring to the X-direction
                if (dir.x >= 0 ? branch.transform.position.x > PlayerBody.transform.position.x 
                    : branch.transform.position.x < PlayerBody.transform.position.x &&
                    PlayerBody.transform.position.y < branch.transform.position.y + 1 &&
                    PlayerBody.transform.position.y > branch.transform.position.y - 1)
                {
                    atBranch = branch;                 
                    return true;
                }
            }
            else
            {
                dir.x = 0; // when moving the player through the pipe, we don't want to move in x direction

                // if we are at the y value and the branch we are observing is at the same
                // x value, then return true. This is only if we are referring to the Y-direction
                if (dir.y >= 0 ? branch.transform.position.y > PlayerBody.transform.position.y 
                    : branch.transform.position.y < PlayerBody.transform.position.y &&
                    PlayerBody.transform.position.x < branch.transform.position.x + 1 &&
                    PlayerBody.transform.position.x > branch.transform.position.x - 1)
                {
                    atBranch = branch;
                    return true;
                }
            }
        }

        return false;
    }

    void MoveToPipeExit()
    {
        switch ((int)atBranch.transform.eulerAngles.z)
        {
            case 0: // up
                branches.Remove(atBranch);
                dir = new Vector2(0, 1); // will cause the player to move up with AtBranch method                
                break;
            case 90: // left
                dir = new Vector2(-1, 0); // will cause the player to move left with AtBranch method                
                break;
            case 180: // down
                dir = new Vector2(0, -1); // will cause the player to move down with AtBranch method                
                break;
            case 270: // right
                dir = new Vector2(1, 0); // will cause the player to move right with AtBranch method                
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

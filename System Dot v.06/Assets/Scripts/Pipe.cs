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
    private List<GameObject> backUpBranches = new List<GameObject>(); // since we are removing branches, this will store them for refresh

    // Use this for initialization
    void Awake () {
        foreach(GameObject branch in branches)
        {
            backUpBranches.Add(branch);
        }          
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
                // to avoid adding a pointer from one list to another
                foreach(GameObject missingBranch in backUpBranches.Except(branches).ToList())
                {
                    branches.Add(missingBranch);
                }               
                atDestination = false;
                finished = true;
            }            
        }
    }

    public bool AtBranch(bool isX)
    {
        foreach(GameObject pipeExit in pipeExits)
        {
            if (atDestination)
                break;

            if (isX) {
                // we are at the destination if the player is past the x coordinate of the pipe exit
                // and is within the y value range
                atDestination = ((dir.x >= 0 ?
                    PlayerBody.transform.position.x > pipeExit.transform.position.x + .5 :
                    PlayerBody.transform.position.x < pipeExit.transform.position.x - .5) &&
                    PlayerBody.transform.position.y < pipeExit.transform.position.y + 1 &&
                    PlayerBody.transform.position.y > pipeExit.transform.position.y - 1);
            }
            else
            {
                // we are at the destination if the player is past the y coordinate of the pipe exit
                // and is within the x value range
                atDestination = ((dir.y >= 0 ?
                    PlayerBody.transform.position.y > pipeExit.transform.position.y + .5 :
                    PlayerBody.transform.position.y < pipeExit.transform.position.y - .5) &&
                    PlayerBody.transform.position.x < pipeExit.transform.position.x + 1 &&
                    PlayerBody.transform.position.x > pipeExit.transform.position.x - 1);
            }
        }

        if (atDestination)
            return false;

        foreach(GameObject branch in branches)
        {
            if (isX) {
                dir.y = 0;  // when moving the player through the pipe, we don't want to move in y direction

                // if we are at the x value and the branch we are observing is at the same
                // y value, then return true. This is only if we are referring to the X-direction
                if (Math.Abs(PlayerBody.transform.position.x - branch.transform.position.x) < .2 &&                    
                    PlayerBody.transform.position.y < branch.transform.position.y + 1 &&
                    PlayerBody.transform.position.y > branch.transform.position.y - 1)
                {
                    atBranch = branch;                 
                    return false;
                }
            }
            else
            {
                dir.x = 0; // when moving the player through the pipe, we don't want to move in x direction

                // if we are at the y value and the branch we are observing is at the same
                // x value, then return true. This is only if we are referring to the Y-direction
                if (Math.Abs(PlayerBody.transform.position.y - branch.transform.position.y) < .2 &&
                    PlayerBody.transform.position.x < branch.transform.position.x + 1 &&
                    PlayerBody.transform.position.x > branch.transform.position.x - 1)
                {
                    atBranch = branch;
                    return false;
                }
            }
        }

        return true;
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
                branches.Remove(atBranch);
                dir = new Vector2(-1, 0); // will cause the player to move left with AtBranch method                
                break;
            case 180: // down
                branches.Remove(atBranch);
                dir = new Vector2(0, -1); // will cause the player to move down with AtBranch method                
                break;
            case 270: // right
                branches.Remove(atBranch);
                dir = new Vector2(1, 0); // will cause the player to move right with AtBranch method                
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerBody = other.gameObject;

            // determine direction the player initially is moving by getting direction to nearest branch
            float maxDistance = Mathf.Infinity;
            GameObject closestBranch = new GameObject();
            foreach(GameObject branch in branches)
            {
                float distanceToBranch = Vector2.Distance(branch.transform.position, other.transform.position);
                if (distanceToBranch < maxDistance)
                {
                    maxDistance = distanceToBranch;
                    closestBranch = branch;
                }
            }

            if (closestBranch != null)
            {
                dir = closestBranch.transform.position - other.transform.position;
            }
            else
            {
                // if there are no branches, then there must be just a pipe exit
                dir = pipeExits[0].transform.position - other.transform.position;
            }
     

            // Figure out what direction player enters the pipe
            //dir = other.GetComponent<Rigidbody2D>().velocity;
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

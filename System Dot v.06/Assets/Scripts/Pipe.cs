using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pipe : MonoBehaviour {

    public float speed;
    public List<GameObject> branches;
    public List<GameObject> pipeExits;
    public bool acceptPlayer = true;

        Vector2 dir;
    GameObject TravelObject;
    private bool atDestination = false;
    private bool finished = false;

    private GameObject atBranch; // stores game object of branch that we collide with
    private List<GameObject> backUpBranches = new List<GameObject>(); // since we are removing branches, this will store them for refresh
    private GameObject intelliSense;

    // Use this for initialization
    void Awake () {
        foreach(GameObject branch in branches)
        {
            backUpBranches.Add(branch);
        }

        intelliSense = GameObject.Find("Intellisense");
	}
	
	// Update is called once per frame
	void Update () {
        if (TravelObject != null && dir != Vector2.zero && !finished)
        {
            // see which direction (x or y) player is coming from
            if (AtBranch(Math.Abs(dir.x) > Math.Abs(dir.y)))
            {
                TravelObject.GetComponent<Transform>().position += new Vector3(dir.x * speed, dir.y * speed);           
            } else if (!atDestination)
            {
                MoveToPipeExit();
            }
            else if (atDestination)
            {
                if (TravelObject.tag == "Player")
                {
                    TravelObject.GetComponent<PlayerController>().pauseMovement = false;
                    TravelObject.GetComponent<CircleCollider2D>().enabled = true;
                    TravelObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    if (intelliSense)
                    {
                        intelliSense.SetActive(true);
                    }
                    TravelObject.transform.FindChild("Player").gameObject.SetActive(true);
                } else if(TravelObject.tag == "Escort")
                {
                    TravelObject.GetComponent<BoxCollider2D>().enabled = true;
                    TravelObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    TravelObject.GetComponent<SpriteRenderer>().enabled = true;
                    TravelObject.GetComponent<EscortIntelliSense>().playButton.SetActive(true);
                    TravelObject.GetComponent<EscortIntelliSense>().pauseButton.SetActive(false);
                } else if(TravelObject.tag == "PipeObject")
                {
                    TravelObject.GetComponent <CircleCollider2D> ().enabled = true;
                    TravelObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    TravelObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    TravelObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
                    TravelObject.GetComponent<SpriteRenderer>().enabled = true;
                    foreach (Transform child in TravelObject.transform)
                    {
                        if (child.GetComponent<SpriteRenderer>())
                        {
                            child.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        else if (child.GetComponent<MeshRenderer>())
                        {
                            child.GetComponent<MeshRenderer>().enabled = true;
                        }
                    }
                }
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
        foreach (GameObject pipeExit in pipeExits)
        {
            if (atDestination)
                break;

            if (isX)
            {
                // we are at the destination if the player is past the x coordinate of the pipe exit
                // and is within the y value range
                atDestination = (
                    TravelObject.transform.position.x < pipeExit.transform.position.x + 1 &&
                    TravelObject.transform.position.x > pipeExit.transform.position.x - 1 &&
                    TravelObject.transform.position.y < pipeExit.transform.position.y + 1 &&
                    TravelObject.transform.position.y > pipeExit.transform.position.y - 1);

                if (atDestination)
                {
                    if (dir.x >= 0 ?
                       TravelObject.transform.position.x > pipeExit.transform.position.x + .5 :
                       TravelObject.transform.position.x < pipeExit.transform.position.x - .5)
                    {
                        return false;
                    }
                    else
                    {
                        atDestination = false;
                    }
                }
            }
            else
            {
                // we are at the destination if the player is past the y coordinate of the pipe exit
                // and is within the x value range
                atDestination = (
                    TravelObject.transform.position.y < pipeExit.transform.position.y + 1 &&
                    TravelObject.transform.position.y > pipeExit.transform.position.y - 1 &&
                    TravelObject.transform.position.x < pipeExit.transform.position.x + 1 &&
                    TravelObject.transform.position.x > pipeExit.transform.position.x - 1);

                if (atDestination)
                {
                    if (dir.y >= 0 ?
                     TravelObject.transform.position.y > pipeExit.transform.position.y + .5 :
                     TravelObject.transform.position.y < pipeExit.transform.position.y - .5)
                    {
                        return false;
                    }
                    else
                    {
                        atDestination = false;
                    }
                }
            }            
        }            
        

   

        foreach(GameObject branch in branches)
        {
            if (isX) {
                dir.y = 0;  // when moving the player through the pipe, we don't want to move in y direction

                // if we are at the x value and the branch we are observing is at the same
                // y value, then return true. This is only if we are referring to the X-direction
                if (Math.Abs(TravelObject.transform.position.x - branch.transform.position.x) < .2 &&                    
                    TravelObject.transform.position.y < branch.transform.position.y + 1 &&
                    TravelObject.transform.position.y > branch.transform.position.y - 1)
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
                if (Math.Abs(TravelObject.transform.position.y - branch.transform.position.y) < .2 &&
                    TravelObject.transform.position.x < branch.transform.position.x + 1 &&
                    TravelObject.transform.position.x > branch.transform.position.x - 1)
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
        if(other.tag == "Player" && acceptPlayer)
        {
            TravelObject = other.gameObject;

            GetInitialDirection(other);

            finished = false; // start the pipe journey

            TravelObject.GetComponent<PlayerController>().pauseMovement = true;
            TravelObject.GetComponent<CircleCollider2D>().enabled = false;
            TravelObject.GetComponent<Rigidbody2D>().isKinematic = true;
            if (intelliSense)
            {
                intelliSense.SetActive(false);
            }
            other.transform.FindChild("Player").gameObject.SetActive(false);
        } else if(other.tag == "Escort")
        {
            TravelObject = other.gameObject;

            GetInitialDirection(other);

            finished = false; // start the pipe journey

            TravelObject.GetComponent<BoxCollider2D>().enabled = false;
            TravelObject.GetComponent<Rigidbody2D>().isKinematic = true;
            TravelObject.GetComponent<SpriteRenderer>().enabled = false;
            TravelObject.GetComponent<EscortIntelliSense>().playButton.SetActive(false);
            TravelObject.GetComponent<EscortIntelliSense>().pauseButton.SetActive(false);
        } else if(other.tag == "PipeObject")
        {
            TravelObject = other.gameObject;

            GetInitialDirection(other);

            finished = false; // start the pipe journey

            TravelObject.GetComponent<CircleCollider2D>().enabled = false;
            foreach(Transform child in TravelObject.transform)
            {
                if (child.GetComponent<SpriteRenderer>())
                {
                    child.GetComponent<SpriteRenderer>().enabled = false;
                } else if (child.GetComponent<MeshRenderer>())
                {
                    child.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            TravelObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            TravelObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
            TravelObject.GetComponent<Rigidbody2D>().isKinematic = true;
            TravelObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void GetInitialDirection(Collider2D other)
    {
        // determine direction the player initially is moving by getting direction to nearest branch
        float maxDistance = Mathf.Infinity;
        GameObject closestBranch = new GameObject();
        foreach (GameObject branch in branches)
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
    }
}

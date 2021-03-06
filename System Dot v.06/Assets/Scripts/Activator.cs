﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour {

    public int powerNeeded; // number of power needed

    public float power; // number of power currently charged
    public bool activate = false;

    private float standardPower; // keep track if power is changing    
    public GameObject[] pellets;
    public GameObject connectedTo;

  	// Use this for initialization
	void Start () {
     
	}
	
	// Update is called once per frame
	void Update () {

        if (activate)
        {
            if (standardPower != power)
            {
                bool incVal = (standardPower < power) || (standardPower > powerNeeded);
                standardPower = power;
                if (power != powerNeeded)
                {
                    connectedTo.GetComponent<Activation>().deactivate();
                }
                StartCoroutine(colorPellets(incVal));
            }
        }
        else
        {       
            // in case the power changes while we are still updating
            foreach (GameObject pellet in pellets)
            {
                pellet.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }


    }

    public IEnumerator colorPellets(bool increasePower)
    {
        if (power > powerNeeded)
        {
            foreach(GameObject pellet in pellets)
            {
                pellet.GetComponent<SpriteRenderer>().color = Color.magenta;    
            }

            for (int i = 0; i < pellets.Length; i++)
            {
                pellets[i].GetComponent<SpriteRenderer>().color = Color.gray;
                yield return new WaitForSeconds(.5f);
            }
        }
        else
        {
            if (increasePower)
            {
                for (int i = 0; i < pellets.Length; i++)
                {
                    if (i < power)
                    {
                        pellets[i].GetComponent<SpriteRenderer>().color = Color.yellow;
                        yield return new WaitForSeconds(.5f);
                    }
                    else
                    {
                        pellets[i].GetComponent<SpriteRenderer>().color = Color.magenta;
                        yield return new WaitForSeconds(.5f);
                    }
                }
            }
            else
            {
                for(int i = pellets.Length - 1; i >= 0; i--)
                {
                    if (i >= power && pellets[i].GetComponent<SpriteRenderer>().color == Color.yellow)
                    {
                        pellets[i].GetComponent<SpriteRenderer>().color = Color.magenta;
                        yield return new WaitForSeconds(.5f);
                    }
                }
            }

            if (power == powerNeeded)
            {
                connectedTo.GetComponent<Activation>().activate();
            }
        }


    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "ActivationZone")
        {
            activate = true;
            powerNeeded = other.gameObject.GetComponent<ActivationZone>().powerNeeded;
            connectedTo = other.gameObject.GetComponent<ActivationZone>().connectedTo;
            pellets = other.gameObject.GetComponent<ActivationZone>().pellets;
            standardPower = -1;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "ActivationZone")
        {
            activate = true;
            powerNeeded = other.gameObject.GetComponent<ActivationZone>().powerNeeded;
            connectedTo = other.gameObject.GetComponent<ActivationZone>().connectedTo;
            pellets = other.gameObject.GetComponent<ActivationZone>().pellets;
        //    standardPower = -1;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "ActivationZone")
        {
            activate = false;
            foreach (GameObject pellet in pellets)
            {
                pellet.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
            connectedTo.GetComponent<Activation>().deactivate();
        }
    }


}

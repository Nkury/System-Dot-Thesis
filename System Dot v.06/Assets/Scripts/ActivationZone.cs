using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationZone : MonoBehaviour {

    public int powerNeeded;
    public GameObject connectedTo;
    public GameObject[] pellets;

    public GameObject connectedBlock;
    public void Update()
    {
        if (connectedBlock != null) {
           if(connectedBlock.GetComponent<SpriteRenderer>().color == Color.yellow)
            {
                StartCoroutine(colorPellets(true));
            }
            else
            {
                StartCoroutine(colorPellets(false));
            }
        }
    }  

    public IEnumerator colorPellets(bool increasePower)
    {
        for (int i = 0; i < pellets.Length; i++)
        {
            if (increasePower)
            {
                pellets[i].GetComponent<SpriteRenderer>().color = Color.yellow;
                yield return new WaitForSeconds(.5f);
            }
            else
            {
                pellets[i].GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }

        if (increasePower)
        {
            connectedTo.GetComponent<Activation>().activate();
        }
        else
        {
            connectedTo.GetComponent<Activation>().deactivate();
        }
    }
}

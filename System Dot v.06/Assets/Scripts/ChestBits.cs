﻿using UnityEngine;
using System.Collections;

public class ChestBits : MonoBehaviour {

    public int numOfBits;
    public GameObject bit;

    public void chestOpen()
    {
        GameObject.Find("Sound Controller").GetComponent<SoundController>().play("chest");
        for (int i = 0; i < numOfBits; i++)
        {
            Instantiate(bit, transform.position + new Vector3(UnityEngine.Random.Range(-.5f, .5f), UnityEngine.Random.Range(-.5f, .5f)), transform.rotation);
        }

        PlayerStats.deadObjects.Add(this.gameObject.name);
        Destroy(this.gameObject);
    }
}

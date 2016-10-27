using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Checkpoint : MonoBehaviour {

	public LevelManager levelManager;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "Player") 
		{

            // autosave feature
            if (Game.current != null && PlayerStats.checkpoint != this.gameObject.name)
            {
                Debug.Log("Activated Checkpoint " + transform.position);
                Game.current.playerName = PlayerStats.playerName;
                Game.current.maxHealth = PlayerStats.maxHealth;
                Game.current.currentHealth = PlayerStats.currentHealth;
                Game.current.armorHealth = PlayerStats.armorHealth;
                Game.current.bitsCollected = PlayerStats.bitsCollected;
                Game.current.numberOfDeaths = PlayerStats.numberOfDeaths;
                Debug.Log("Deaths: " + PlayerStats.numberOfDeaths);
                Game.current.totalSecondsOfPlaytime = PlayerStats.totalSecondsOfPlaytime;
                Debug.Log("Time played: " + PlayerStats.totalSecondsOfPlaytime);
                PlayerStats.checkpoint = this.gameObject.name;
                Game.current.checkpoint = this.gameObject.name;
                PlayerStats.deadObjects = PlayerStats.deadObjects.Distinct().ToList<string>();
                Game.current.deadObjects = PlayerStats.deadObjects;
                if(PlayerStats.highestCheckpoint < Int32.Parse(this.gameObject.name.Substring(this.gameObject.name.Length - 1, 1)))
                {
                    PlayerStats.highestCheckpoint = Int32.Parse(this.gameObject.name.Substring(this.gameObject.name.Length - 1, 1));
                }
                Game.current.highestCheckpoint = PlayerStats.highestCheckpoint;
                SaveLoad.Save();
            }

            if(levelManager)
                levelManager.currentCheckpoint = gameObject;
	
		}
	}
}

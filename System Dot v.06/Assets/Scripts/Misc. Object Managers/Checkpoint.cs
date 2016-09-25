using UnityEngine;
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
                Game.current.bitsCollected = PlayerStats.bitsCollected;
                PlayerStats.checkpoint = this.gameObject.name;
                Game.current.checkpoint = this.gameObject.name;
                PlayerStats.deadObjects = PlayerStats.deadObjects.Distinct().ToList<string>();
                Game.current.deadObjects = PlayerStats.deadObjects;
                SaveLoad.Save();
            }

            levelManager.currentCheckpoint = gameObject;
	
		}
	}
}

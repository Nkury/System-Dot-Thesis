using UnityEngine;
using System.Collections;

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
            // checks what checkpoint has been collided and adjusts player stats accordingly
            PlayerStats.checkpoint = this.gameObject.name;

            // autosave feature
            if (Game.current != null)
            {
                Debug.Log("Activated Checkpoint " + transform.position);
                Game.current.playerName = PlayerStats.playerName;
                Game.current.bitsCollected = PlayerStats.bitsCollected;
                Game.current.checkpoint = PlayerStats.checkpoint;
                Game.current.deadObjects = PlayerStats.deadObjects;
                SaveLoad.Save();
            }

            levelManager.currentCheckpoint = gameObject;
	
		}
	}
}

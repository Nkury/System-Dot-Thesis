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
            switch (this.gameObject.name)
            {
                case "Checkpoint1":
                    PlayerStats.firstCheckpoint = true;
                    break;
                case "Checkpoint2":
                    PlayerStats.secondCheckpoint = true;
                    break;
                case "Checkpoint3":
                    PlayerStats.thirdCheckpoint = true;
                    break;
                case "Checkpoint4":
                    PlayerStats.fourthCheckpoint = true;
                    break;
                case "Checkpoint5":
                    PlayerStats.fifthCheckpoint = true;
                    break;
            }

            // autosave feature
            if (Game.current != null)
            {
                Game.current.playerName = PlayerStats.playerName;
                Game.current.bitsCollected = PlayerStats.bitsCollected;
                Game.current.firstCheckpoint = PlayerStats.firstCheckpoint;
                Game.current.secondCheckpoint = PlayerStats.secondCheckpoint;
                Game.current.thirdCheckpoint = PlayerStats.thirdCheckpoint;
                Game.current.fourthCheckpoint = PlayerStats.fourthCheckpoint;
                Game.current.fifthCheckpoint = PlayerStats.fifthCheckpoint;
                Game.current.deadObjects = PlayerStats.deadObjects;
                SaveLoad.Save();
            }

            levelManager.currentCheckpoint = gameObject;
			Debug.Log("Activated Checkpoint " + transform.position);
		}
	}
}

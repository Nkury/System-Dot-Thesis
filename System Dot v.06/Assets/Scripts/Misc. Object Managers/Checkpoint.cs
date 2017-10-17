using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Checkpoint : MonoBehaviour {

	public LevelHandler levelManager;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelHandler> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "PlayerBody") 
		{
            // if we enter a new level
            if (PlayerStats.levelName != SceneManager.GetActiveScene().name)
            {
                PlayerStats.levelName = SceneManager.GetActiveScene().name;
                PlayerStats.highestCheckpoint = 1;            
                PlayerStats.deadObjects.Clear();
                PlayerStats.terminalStrings.Clear();
            }
            

            // autosave feature
            if ((Game.current != null && PlayerStats.checkpoint != this.gameObject.name && PlayerStats.highestCheckpoint <= Int32.Parse(this.gameObject.name.Split('t')[1]))
                || PlayerStats.checkpoint == "Checkpoint1")
            {
                LogToFile.WriteToFile("HIT-" + this.gameObject.name, "PLAYER-" + PlayerStats.playerName);
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
                if (PlayerStats.highestCheckpoint < Int32.Parse(this.gameObject.name.Split('t')[1]))
                {
                    PlayerStats.highestCheckpoint = Int32.Parse(this.gameObject.name.Split('t')[1]);
                }
                Game.current.highestCheckpoint = PlayerStats.highestCheckpoint;
                Game.current.levelName = PlayerStats.levelName;
                SaveTerminalStrings();
                SaveLoad.Save();
            }    

            if (levelManager)
                levelManager.currentCheckpoint = gameObject;
	
		}
	}

    // saves terminal strings and whether the enemy has been seen
    public void SaveTerminalStrings()
    {
        EnemyTerminal[] enemies = FindObjectsOfType<EnemyTerminal>();
        foreach (EnemyTerminal e in enemies)
        {
            List<string> terminalString;
            if (PlayerStats.terminalStrings.TryGetValue(e.gameObject.name, out terminalString))
            {
                if (!terminalString.SequenceEqual(e.terminalString))
                {
                    PlayerStats.terminalStrings[e.gameObject.name] = e.terminalString.ToList();
                }
            }
            else
            {
                PlayerStats.terminalStrings.Add(e.gameObject.name, e.terminalString.ToList());
            }

            bool seen;
            if (PlayerStats.enemySeen.TryGetValue(e.gameObject.name, out seen))
            {
                if (seen != e.seen)
                {
                    PlayerStats.enemySeen[e.gameObject.name] = e.seen;
                }
            }
            else
            {
                PlayerStats.enemySeen.Add(e.gameObject.name, e.seen);
            }
        }

        Game.current.terminalStrings = PlayerStats.terminalStrings;
        Game.current.enemySeen = PlayerStats.enemySeen;
    }
}

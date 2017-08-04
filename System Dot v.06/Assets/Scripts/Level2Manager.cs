using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : LevelHandler {

    [Header("Miscellaneous")]
    public GameObject intelliSense;

    public override void LoadLevel()
    {
        // set it to false so we don't have to go through the tutorial
        base.loadedIn = true;
        switch (PlayerStats.checkpoint)
        {
            case "Checkpoint1":                
                intelliSense.SetActive(true);
                break;    
        }

        currentCheckpoint = GameObject.Find(PlayerStats.checkpoint);
    }
}

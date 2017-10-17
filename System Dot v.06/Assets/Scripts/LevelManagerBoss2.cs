using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerBoss2 : LevelHandler {

    [Header("Miscellaneous")]
    public GameObject intelliSense;

    // Update is called once per frame
    public override void LoadLevel() { 
        // set it to false so we don't have to go through the tutorial
        base.loadedIn = true;
        intelliSense.SetActive(true);
        currentCheckpoint = GameObject.Find(PlayerStats.checkpoint);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Manager : LevelHandler
{

    [Header("Miscellaneous")]
    public GameObject intelliSense;

    public override void LoadLevel()
    {
        // set it to false so we don't have to go through the tutorial
        base.loadedIn = true;
        intelliSense.SetActive(true);
        switch (PlayerStats.checkpoint)
        {
           
        }

        currentCheckpoint = GameObject.Find(PlayerStats.checkpoint);
    }
}

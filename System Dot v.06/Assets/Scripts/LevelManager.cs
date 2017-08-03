 using UnityEngine;
using System.Collections;

public class LevelManager : LevelHandler {

    [Header("Level 1 Checkpoints")]
    public GameObject checkpoint1;
    public GameObject checkpoint2;
    public GameObject checkpoint3;
    public GameObject checkpoint4;
    public GameObject checkpoint5;

    [Header("Level 1 Boss Checkpoint")]
    public GameObject checkpoint6;

    [Header("Miscellaneous")]
    public GameObject intelliSense;
    public GameObject APIButton;
    public GameObject directionHelpButton;
    public GameObject chestHelpButton;
    public GameObject DebugButton;

    public static bool canPressTab = true;

    public override void LoadLevel()
    {
        // set it to false so we don't have to go through the tutorial
        base.loadedIn = true;
        switch (PlayerStats.checkpoint)
        {
            case "Checkpoint1":
                currentCheckpoint = checkpoint1;
                intelliSense.SetActive(true);
                APIButton.SetActive(false);
                DebugButton.SetActive(false);
                directionHelpButton.SetActive(false);
                chestHelpButton.SetActive(false);
                IntelliSenseTest.clickOnce = false;
                APISystem.clicked = false;
                canPressTab = false; // cannot press tab to switch boots to serve the tutorial
                break;
            case "Checkpoint2":
                APIButton.SetActive(false);
                DebugButton.SetActive(false);
                IntelliSenseTest.clickOnce = true;
                directionHelpButton.SetActive(false);
                chestHelpButton.SetActive(false);
                APISystem.clicked = false;
                currentCheckpoint = checkpoint2;
                break;
            case "Checkpoint3":
                IntelliSenseTest.clickOnce = true;
                directionHelpButton.SetActive(false);
                currentCheckpoint = checkpoint3;
                break;
            case "Checkpoint4":
                IntelliSenseTest.clickOnce = false;
                currentCheckpoint = checkpoint4;
                break;
            case "Checkpoint5":
                currentCheckpoint = checkpoint5;
                break;
            case "Checkpoint6":
                currentCheckpoint = checkpoint6;
                break;
        }
    }
}

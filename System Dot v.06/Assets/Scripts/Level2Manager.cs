using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : LevelHandler {

    [Header("Miscellaneous")]
    public GameObject intelliSense;
    public GameObject checkZone;

    public override void LoadLevel()
    {
        // set it to false so we don't have to go through the tutorial
        base.loadedIn = true;
        intelliSense.SetActive(true);
        switch (PlayerStats.checkpoint)
        {
            case "Checkpoint6":
                GameObject.Find("Main HUD").GetComponent<TerminalWindowUI>().setVariabullCode("int flint = 5;");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                break;
            case "Checkpoint7":
                checkZone.GetComponent<CheckZone>().numRescued = 3;
                checkZone.GetComponent<CheckZone>().listOfVariabulls.Add("int flint = 5; ");
                checkZone.GetComponent<CheckZone>().AddParameters();
                IntelliSenseLevel2.clickOnce = true;
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                break;
            case "Checkpoint8":
                checkZone.GetComponent<CheckZone>().numRescued = 3;
                checkZone.GetComponent<CheckZone>().listOfVariabulls.Add("int flint = 5; ");
                checkZone.GetComponent<CheckZone>().AddParameters();
                IntelliSenseLevel2.clickOnce = true;
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                break;
            case "Checkpoint9":
                checkZone.GetComponent<CheckZone>().numRescued = 3;
                checkZone.GetComponent<CheckZone>().listOfVariabulls.Add("int flint = 5; ");
                checkZone.GetComponent<CheckZone>().AddParameters();
                IntelliSenseLevel2.clickOnce = true;
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                break;
            case "Checkpoint10":
                checkZone.GetComponent<CheckZone>().numRescued = 2;
                checkZone.GetComponent<CheckZone>().listOfVariabulls.Add("int flint = 5; double dec = 0.25; ");
                checkZone.GetComponent<CheckZone>().AddParameters();
                IntelliSenseLevel2.clickOnce = true;
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("DoubleAPI");
                break;
            case "Checkpoint11":
                checkZone.GetComponent<CheckZone>().numRescued = 2;
                checkZone.GetComponent<CheckZone>().listOfVariabulls.Add("int flint = 5; double dec = 0.25; ");
                checkZone.GetComponent<CheckZone>().AddParameters();
                IntelliSenseLevel2.clickOnce = true;
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("DoubleAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("SmashAPI");
                break;
            case "Checkpoint12":
                checkZone.GetComponent<CheckZone>().numRescued = 2;
                checkZone.GetComponent<CheckZone>().listOfVariabulls.Add("int flint = 5; double dec = 0.25; ");
                checkZone.GetComponent<CheckZone>().AddParameters();
                IntelliSenseLevel2.clickOnce = true;
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("DoubleAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("SmashAPI");
                break;
            case "Checkpoint13":
                checkZone.GetComponent<CheckZone>().numRescued = 2;
                checkZone.GetComponent<CheckZone>().listOfVariabulls.Add("int flint = 5; double dec = 0.25; ");
                checkZone.GetComponent<CheckZone>().AddParameters();
                IntelliSenseLevel2.clickOnce = false;
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("DoubleAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("SmashAPI");
                break;
            case "Checkpoint14":
                GameObject.Find("Main HUD").GetComponent<TerminalWindowUI>().setVariabullCode("string word = \"sentence\";");
                checkZone.GetComponent<CheckZone>().numRescued = 2;
                checkZone.GetComponent<CheckZone>().listOfVariabulls.Add("int flint = 5; double dec = 0.25; ");
                checkZone.GetComponent<CheckZone>().AddParameters();
                IntelliSenseLevel2.clickOnce = false;
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("DoubleAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("SmashAPI");
                break;
            case "Checkpoint15":
                checkZone.GetComponent<CheckZone>().numRescued = 1;
                checkZone.GetComponent<CheckZone>().listOfVariabulls.Add("int flint = 5; double dec = 0.25; string word = \"sentence\"; ");
                checkZone.GetComponent<CheckZone>().AddParameters();
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("DoubleAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("StringAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("SmashAPI");
                break;
            case "Checkpoint16":
                checkZone.GetComponent<CheckZone>().numRescued = 1;
                checkZone.GetComponent<CheckZone>().listOfVariabulls.Add("int flint = 5; double dec = 0.25; string word = \"sentence\"; ");
                checkZone.GetComponent<CheckZone>().AddParameters();
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("DoubleAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("StringAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("SmashAPI");
                break;
            case "Checkpoint17":  
                GameObject.Find("Main HUD").GetComponent<TerminalWindowUI>().setVariabullCode("boolean bool = true;");
                checkZone.GetComponent<CheckZone>().numRescued = 1;
                checkZone.GetComponent<CheckZone>().listOfVariabulls.Add("int flint = 5; double dec = 0.25; string word = \"sentence\"; ");
                checkZone.GetComponent<CheckZone>().AddParameters();
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("DoubleAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("StringAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("BooleanAPI");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("SmashAPI");
                break;
        }

        currentCheckpoint = GameObject.Find(PlayerStats.checkpoint);
    }
}

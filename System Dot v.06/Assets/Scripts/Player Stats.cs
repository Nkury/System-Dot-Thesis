using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats {

    /**** PLAYER ATTRIBUTES ************************************************************************/
    public static int maxHealth;
    public static int currentHealth;
    public static int armorHealth;
    public static string playerName;
    public static int bitsCollected;
    public static int numberOfDeaths;
    public static float totalSecondsOfPlaytime;

    public static int numHealthPotions;
    public static string hat;

    /**** PLAYER PROGRESS *************************************************************************/
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  
    /**** TUTORIAL LEVEL *********************************/
    public static List<string> deadObjects = new List<string>();
    public static string checkpoint = "Checkpoint1";
    public static int highestCheckpoint = 1;

}

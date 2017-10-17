using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats {
    /*** ADAPTIVITY STATISTICS *********************************************************************/
    public static int timeToDebugThreshhold = 5;
    public static Dictionary<string, int> log_numAPIOpen = new Dictionary<string, int>();
    public static Dictionary<string, int> log_numSyntaxErrors = new Dictionary<string, int>();
    public static Dictionary<string, int> log_numPerfectEdits = new Dictionary<string, int>();
    public static Dictionary<string, int> log_numOfF5 = new Dictionary<string, int>();
    public static Dictionary<string, int> log_numLegacyCodeViewed = new Dictionary<string, int>();
    public static Dictionary<string, int> log_codeSeen = new Dictionary<string, int>();
    public static Dictionary<string, int> log_numQuickDebug = new Dictionary<string, int>();
    public static Dictionary<string, int> log_totalNumDebugs = new Dictionary<string, int>();
    public static Dictionary<string, int> log_totalNumberOfModifiedEdits = new Dictionary<string, int>();
    public static Dictionary<string, int> log_totalNumberOfLegacyOnly = new Dictionary<string, int>();
    public static Dictionary<string, int> log_totalNumberOfObjects = new Dictionary<string, int>();

    /**** USER PROFILE APTITUDE STATS **************************************************************/
    public static float averageTimeOnEditing;
    public static float longestTimeOnEditing;
    public static float averageNumberofMouseClicks;
    public static float mostNumberofMouseClicks;
    public static int mostNumberofAttempts;
    public static int numberOfPerfectEdits;
    public static int mostNumberOfBackspaces;
    public static int averageNumberOfBackspaces;
    public static int mostNumberOfDeletes;
    public static int averageNumberOfDeletes;
    public static float averageTimeOfMouseInactivity;
    public static float mostTimeofMouseInactivity;
    public static int numOfAPIUses;
    public static int numOfEdits;

    /**** PLAYER ATTRIBUTES ************************************************************************/
    public static int maxHealth;
    public static int currentHealth;
    public static int armorHealth;
    public static string playerName;
    public static int bitsCollected;
    public static int numberOfDeaths;
    public static float totalSecondsOfPlaytime;

    public static int numRevivePotions;
    public static string hat;

    /**** PLAYER PROGRESS *************************************************************************/
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * /
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
    public static string levelName;
    public static string checkpoint = "Checkpoint1";
    public static int highestCheckpoint = 1;
    public static List<string> deadObjects = new List<string>();
    public static Dictionary<string, List<string>> terminalStrings = new Dictionary<string, List<string>>();
    public static Dictionary<string, bool> enemySeen = new Dictionary<string, bool>();
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Game
{
    public static Game current;
    /*** ADAPTIVITY STATISTICS *********************************************************************/
    public Dictionary<string,int> log_numAPIOpen = new Dictionary<string, int>();
    public Dictionary<string,int> log_numSyntaxErrors = new Dictionary<string, int>();
    public Dictionary<string,int> log_numPerfectEdits = new Dictionary<string, int>();
    public Dictionary<string,int> log_numOfF5 = new Dictionary<string, int>();
    public Dictionary<string,int> log_numLegacyCodeViewed = new Dictionary<string, int>();
    public Dictionary<string,int> log_codeSeen = new Dictionary<string, int>();
    public Dictionary<string,int> log_numQuickDebug = new Dictionary<string, int>();
    public Dictionary<string,int> log_totalNumDebugs = new Dictionary<string, int>();
    public Dictionary<string,int> log_totalNumberOfModifiedEdits = new Dictionary<string, int>();
    public Dictionary<string,int> log_totalNumberOfEdits = new Dictionary<string, int>();
    public Dictionary<string, int> log_totalNumberOfObjects = new Dictionary<string, int>();

    /**** USER PROFILE APTITUDE STATS **************************************************************/
    public float averageTimeOnEditing;
    public float longestTimeOnEditing;
    public float averageNumberofMouseClicks;
    public float mostNumberofMouseClicks;
    public int mostNumberofAttempts;
    public int numberOfPerfectEdits;
    public int mostNumberOfBackspaces;
    public int averageNumberOfBackspaces;
    public int mostNumberOfDeletes;
    public int averageNumberOfDeletes;
    public float averageTimeOfMouseInactivity;
    public float mostTimeofMouseInactivity;
    public int numOfAPIUses;

    public int numOfEdits;

    /**** PLAYER ATTRIBUTES ************************************************************************/
    public int maxHealth;
    public int currentHealth;
    public int armorHealth;
    public string playerName;
    public int bitsCollected;
    public Dictionary<string, int> numberOfDeaths = new Dictionary<string, int>();
    public Dictionary<string, float> totalSecondsOfPlaytime = new Dictionary<string, float>();
    public int numRevivePotions;

    /**** PLAYER PROGRESS *************************************************************************/
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
    public List<string> deadObjects = new List<string>();
    public Dictionary<string, List<string>> terminalStrings = new Dictionary<string, List<string>>();
    public Dictionary<string, bool> enemySeen = new Dictionary<string, bool>();
    public string levelName;
    public bool firstCheckpoint = true;
    public string checkpoint;
    public int highestCheckpoint;
}

public static class SaveLoad
{
    public static List<Game> savedGames = new List<Game>();

    public static void Save()
    {
        LogSaved();   
        savedGames.Insert(0, Game.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            SaveLoad.savedGames = (List<Game>)bf.Deserialize(file);
            file.Close();
        }
    }

    public static void EraseAt(int index)
    {
        savedGames.RemoveAt(index);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    public static void EraseAll()
    {
        savedGames.Clear();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    public static void LogSaved()
    {
        // ADAPTIVE STATS
        Game.current.log_numAPIOpen = PlayerStats.log_numAPIOpen;
        Game.current.log_numLegacyCodeViewed = PlayerStats.log_numLegacyCodeViewed;
        Game.current.log_codeSeen = PlayerStats.log_codeSeen;
        Game.current.log_numOfF5 = PlayerStats.log_numOfF5;
        Game.current.log_numPerfectEdits = PlayerStats.log_numPerfectEdits;
        Game.current.log_numQuickDebug = PlayerStats.log_numQuickDebug;
        Game.current.log_totalNumDebugs = PlayerStats.log_totalNumDebugs;
        Game.current.log_numSyntaxErrors = PlayerStats.log_numSyntaxErrors;
        Game.current.log_totalNumberOfEdits = PlayerStats.log_totalNumberOfEdits;
        Game.current.log_totalNumberOfModifiedEdits = PlayerStats.log_totalNumberOfModifiedEdits;
        Game.current.log_totalNumberOfObjects = PlayerStats.log_totalNumberOfObjects;        

        // APTITUDE STATS
        Game.current.averageTimeOnEditing = PlayerStats.averageTimeOnEditing;
        Game.current.longestTimeOnEditing = PlayerStats.longestTimeOnEditing;
        Game.current.averageNumberofMouseClicks = PlayerStats.averageNumberofMouseClicks;
        Game.current.mostNumberofMouseClicks = PlayerStats.mostNumberofMouseClicks;
        Game.current.mostNumberofAttempts = PlayerStats.mostNumberofAttempts;
        Game.current.numberOfPerfectEdits = PlayerStats.numberOfPerfectEdits;
        Game.current.mostNumberOfBackspaces = PlayerStats.mostNumberOfBackspaces;
        Game.current.averageNumberOfBackspaces = PlayerStats.averageNumberOfBackspaces;
        Game.current.mostNumberOfDeletes = PlayerStats.mostNumberOfDeletes;
        Game.current.averageNumberOfDeletes = PlayerStats.averageNumberOfDeletes;
        Game.current.averageTimeOfMouseInactivity = PlayerStats.averageTimeOfMouseInactivity;
        Game.current.mostTimeofMouseInactivity = PlayerStats.mostTimeofMouseInactivity;
        Game.current.numOfAPIUses = PlayerStats.numOfAPIUses;
        Game.current.numOfEdits = PlayerStats.numOfEdits;
    }
}

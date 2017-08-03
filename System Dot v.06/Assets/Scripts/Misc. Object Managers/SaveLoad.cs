using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Game
{
    public static Game current;

    /**** USER PROFILE APTITUDE STATS **************************************************************/
    public float typingSpeed;
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
    public int numOfF5;
    public int numOfEdits;

    /**** PLAYER ATTRIBUTES ************************************************************************/
    public int maxHealth;
    public int currentHealth;
    public int armorHealth;
    public string playerName;
    public int bitsCollected;
    public int numberOfDeaths;
    public float totalSecondsOfPlaytime;
    public int numHealthPotions;

    /**** PLAYER PROGRESS *************************************************************************/
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
    public List<string> deadObjects = new List<string>();
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
        Game.current.typingSpeed = PlayerStats.typingSpeed;
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
        Game.current.numOfF5 = PlayerStats.numOfF5;
        Game.current.numOfEdits = PlayerStats.numOfEdits;
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
}

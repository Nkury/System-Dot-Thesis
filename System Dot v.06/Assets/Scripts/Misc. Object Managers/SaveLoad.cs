﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Game
{
    public static Game current;

    /**** PLAYER ATTRIBUTES ************************************************************************/
    public int maxHealth;
    public int currentHealth;
    public string playerName;
    public int bitsCollected;
    public int numberOfDeaths;
    public float totalSecondsOfPlaytime;

    /**** PLAYER PROGRESS *************************************************************************/
    /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *  
    /**** TUTORIAL LEVEL *********************************/
    public List<string> deadObjects = new List<string>();
    public bool firstCheckpoint = true;
    public string checkpoint;
    public int highestCheckpoint;
}

public static class SaveLoad
{
    public static List<Game> savedGames = new List<Game>();

    public static void Save()
    {
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

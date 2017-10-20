using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogToFile {

    public static void WriteToFile(string action, string type, string levelName = "")
    {
        string path = "Assets/Logs/" + (levelName == "" ? SceneManager.GetActiveScene().name : levelName) + "_log.txt";

        // Write text to file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(DateTime.Now + "  " + action + "  " + type);
        writer.Close();

        // Re-import file to update editor's copy
        //AssetDatabase.ImportAsset(path);   

    }
}

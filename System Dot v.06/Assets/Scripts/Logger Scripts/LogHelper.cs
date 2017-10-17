using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogHelper {

    public static void SetDictionaryValue(Dictionary<string, int> dict, int val)
    {
        int value;
        if (dict.TryGetValue(SceneManager.GetActiveScene().name, out value))
        {
            dict[SceneManager.GetActiveScene().name] = val;
        }
        else
        {
            dict.Add(SceneManager.GetActiveScene().name, val);
        }
    }

    public static int GetDictionaryValue(Dictionary<string, int> dict) {
        int val;
        if(dict.TryGetValue(SceneManager.GetActiveScene().name, out val)){
            return val;
        }
        else
        {
            return 0;
        }
    }

    public static int AccumulateDataFromAllLevels(Dictionary<string, int> dict)
    {
        int total = 0;

        foreach (KeyValuePair<string, int> entry in dict)
        {
            total += entry.Value;    
        }

        return total;
    }	

    public static double RoundStat(double valueToRound)
    {
        return Math.Round(valueToRound * 100, 2);
    }
}

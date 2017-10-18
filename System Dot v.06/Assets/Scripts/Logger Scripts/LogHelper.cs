using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogHelper {

    public static void SetDictionaryValue<T>(Dictionary<string, T> dict, T val)
    {
        T value;
        if (dict.TryGetValue(SceneManager.GetActiveScene().name, out value))
        {
            dict[SceneManager.GetActiveScene().name] = val;
        }
        else
        {
            dict.Add(SceneManager.GetActiveScene().name, val);
        }
    }

    public static T GetDictionaryValue<T>(Dictionary<string, T> dict) {
        T val;
        if(dict.TryGetValue(SceneManager.GetActiveScene().name, out val)){
            return val;
        }
        else
        {
            return default(T);
        }
    }

    public static int AccumulateDataFromAllLevels(Dictionary<string, int> dict)
    {
        int total = 0;

        foreach (KeyValuePair<string, int> entry in dict)
        {
            if (entry.Key.Contains("LVL"))
            {
                total += entry.Value;
            }
        }

        return total;
    }	

    public static double RoundStat(double valueToRound)
    {
        return Math.Round(valueToRound * 100, 2);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsLog {
    static int numAPIOpen;
    static int numSyntaxErrors; 
    static int numPerfectEdits; 
    static int numOfF5;
    static int numLegacyCodeViewed;
    static int codeSeen;
    static int numQuickDebug;
    static int totalNumDebugs;
    static int totalNumberOfModifiedEdits;
    static int totalNumberOfLegacyOnly;
    static int totalCodeObjects;

    public static void WriteToFile()
    {
        string path = "Logs/Player-" + PlayerStats.playerName + "-Statistics.txt";
        GetValues(); // set the variables to accumulated values

        // Write text to file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("\n");
        writer.WriteLine("-----------> " + SceneManager.GetActiveScene().name + " <-----------");
        writer.WriteLine("--------------------ADAPTIVE STATISTICS--------------------");

        writer.WriteLine("\n");
        writer.WriteLine("PROCESSING:");
        writer.WriteLine("Quick Debugging: " + numQuickDebug + " / " + totalNumDebugs
            + " = " + LogHelper.RoundStat((float)numQuickDebug / totalNumDebugs) + "%");
        writer.WriteLine("Number of Code Viewed: " + codeSeen + " / " + totalCodeObjects
            + " = " + LogHelper.RoundStat((float)codeSeen  / totalCodeObjects) + "%");
        BayesianNetwork.BNode TimeToClickDebugNode = new BayesianNetwork.BNode(null, "TimeToClickDebug", 0, new List<double>() { (float)numQuickDebug / totalNumDebugs });
        BayesianNetwork.BNode ViewCodeNode = new BayesianNetwork.BNode(null, "ViewCode", 1, new List<double>() { (float)codeSeen / totalCodeObjects });
        BayesianNetwork.BNode ProcessingNode = new BayesianNetwork.BNode(new List<BayesianNetwork.BNode>() { TimeToClickDebugNode, ViewCodeNode }, "Processing", 2, new List<double>() { 0, .25, .75, 1.00 });
        writer.WriteLine("--PROCESSING STAT-> Active: " + LogHelper.RoundStat(ProcessingNode.CalculateProbability(1)) + "% | Reflexive: " + LogHelper.RoundStat(ProcessingNode.CalculateProbability(0)) + "%");

        writer.WriteLine("\n");
        writer.WriteLine("PERCEPTION:");
        writer.WriteLine("Use API to Code: " + numAPIOpen + " / " + totalNumberOfModifiedEdits
          + " = " + LogHelper.RoundStat((float)numAPIOpen / totalNumberOfModifiedEdits) + "%");
        writer.WriteLine("F5 Key Hit: " + numOfF5 + " / " + totalNumDebugs
            + " = " + LogHelper.RoundStat((float)numOfF5 / totalNumDebugs) + "%");
        BayesianNetwork.BNode UseAPINode = new BayesianNetwork.BNode(null, "UseAPI", 0, new List<double>() { (float)numAPIOpen / totalNumberOfModifiedEdits });
        BayesianNetwork.BNode UseF5 = new BayesianNetwork.BNode(null, "UseF5", 1, new List<double>() { (float)numOfF5 / totalNumDebugs });
        BayesianNetwork.BNode PerceptionNode = new BayesianNetwork.BNode(new List<BayesianNetwork.BNode>() { UseAPINode, UseF5}, "Perception", 2, new List<double>() { 1, .8, .2, 0 });
        writer.WriteLine("--PERCEPTION STAT-> Sensing: " + LogHelper.RoundStat(PerceptionNode.CalculateProbability(1)) + "% | Intuitive: " + LogHelper.RoundStat(PerceptionNode.CalculateProbability(0)) + "%");

        writer.WriteLine("\n");
        writer.WriteLine("INPUT:");
        writer.WriteLine("Use API to Code: " + numAPIOpen + " / " + totalNumberOfModifiedEdits
          + " = " + LogHelper.RoundStat((float)numAPIOpen / totalNumberOfModifiedEdits) + "%");
        writer.WriteLine("Number of Code Viewed: " + codeSeen + " / " + totalCodeObjects
             + " = " + LogHelper.RoundStat((float)codeSeen / totalCodeObjects) + "%");
        BayesianNetwork.BNode InputNode = new BayesianNetwork.BNode(new List<BayesianNetwork.BNode>() { UseAPINode, ViewCodeNode }, "Perception", 2, new List<double>() { 0, .25, .75, 1 });
        writer.WriteLine("--INPUT STAT-> Visual: " + LogHelper.RoundStat(InputNode.CalculateProbability(1)) + "% | Verbal: " + LogHelper.RoundStat(InputNode.CalculateProbability(0)) + "%");


        writer.WriteLine("\n");
        writer.WriteLine("MISC:");
        writer.WriteLine("Syntax Errors: " + numSyntaxErrors + " / " + totalNumberOfModifiedEdits
            + " = " + LogHelper.RoundStat((float)numSyntaxErrors /totalNumberOfModifiedEdits) + "%");
        writer.WriteLine("Perfect Edits: " + numPerfectEdits+ " / " + totalNumberOfModifiedEdits
            + " = " + LogHelper.RoundStat((float)numPerfectEdits / totalNumberOfModifiedEdits) + "%");

        writer.WriteLine("\n");
        writer.WriteLine("--------------------PLAYER STATISTICS--------------------");
        int seconds = (int)LogHelper.GetDictionaryValue(PlayerStats.totalSecondsOfPlaytime) % 60;
        int minutes = (int)LogHelper.GetDictionaryValue(PlayerStats.totalSecondsOfPlaytime) / 60;

        writer.WriteLine("Total Time Played In Level: " + string.Format("{0:00}:{1:00}", minutes, seconds));
        writer.WriteLine("Total Deaths in Level: " + LogHelper.GetDictionaryValue(PlayerStats.numberOfDeaths));
        writer.WriteLine("Bits at the end of level: " + PlayerStats.bitsCollected);
        writer.WriteLine("Health: " + PlayerStats.currentHealth + " / " + PlayerStats.maxHealth);
        writer.WriteLine("Armor: " + PlayerStats.armorHealth);
        writer.WriteLine("Revive Potions: " + PlayerStats.numRevivePotions);
        
        writer.Close();

        // Re-import file to update editor's copy
       // AssetDatabase.ImportAsset(path);

    }

    //public double CreateNetwork(double[] individualProbs, double[] CPT)
    //{
    //    BayesianNetwork.BNode ProcessingNode = new BayesianNetwork.BNode(null, "Processing", 2, new List<double>(CPT));

    //    for(int i = 0; i < individualProbs.Length; i++)
    //    {
    //        BayesianNetwork.BNode node = new BayesianNetwork.BNode(null)
    //    }
    //    foreach(double db in individualProbs)
    //    {

    //    }
    //    // Processing Network
    //    BayesianNetwork.BNode TimeToClickDebugNode = new BayesianNetwork.BNode(null, "TimeToClickDebug", 0, new List<double>() { .43 });
    //    BayesianNetwork.BNode ViewLegacyCodeNode = new BayesianNetwork.BNode(null, "ViewLegacyCode", 1, new List<double>() { .72 });
    //    BayesianNetwork.BNode ProcessingNode = new BayesianNetwork.BNode(new List<BayesianNetwork.BNode>() { TimeToClickDebugNode, ViewLegacyCodeNode }, "Processing", 2, new List<double>() { 1.00, .8, .2, 0 });
    //    BayesianNetwork.BNetwork ProcessingNetwork = new BayesianNetwork.BNetwork("ProcessingNetwork", new List<BayesianNetwork.BNode>() { TimeToClickDebugNode, ViewLegacyCodeNode, ProcessingNode });

    //    // Perception Network
    //    BayesianNetwork.BNode UseAPINode = new BayesianNetwork.BNode(null, "UseAPI", 1, new List<double>() { .333 });
    //    BayesianNetwork.BNode NumberOfSyntaxErrors = new BayesianNetwork.BNode(null, "NumberOfSyntaxErrors", 2, new List<double>() { .333 });
    //    BayesianNetwork.BNode NumberTimesF5KeyHit = new BayesianNetwork.BNode(null, "NumberOfF5KeyPressed", 3, new List<double>() { .333 });
    //    BayesianNetwork.BNode PerceptionNode = new BayesianNetwork.BNode(new List<BayesianNetwork.BNode>() { UseAPINode, NumberOfSyntaxErrors, NumberTimesF5KeyHit },
    //        "Perception", 4, new List<double>() { .25, 0, .33, .17, .75, .67, 1, .83 });
    //    BayesianNetwork.BNetwork PerceptionNetwork = new BayesianNetwork.BNetwork("PerceptionNetwork",
    //        new List<BayesianNetwork.BNode>() { UseAPINode, NumberOfSyntaxErrors, NumberTimesF5KeyHit, PerceptionNode });

    //    // Understanding Network
    //    BayesianNetwork.BNode NumberOfPerfectEdits = new BayesianNetwork.BNode(null, "NumberOfPerfectEdits", 0, new List<double>() { .5 });
    //    BayesianNetwork.BNode UnderstandingNode = new BayesianNetwork.BNode(new List<BayesianNetwork.BNode>() { NumberOfPerfectEdits, NumberOfSyntaxErrors }, "Understanding", 1,
    //        new List<double>() { .5, 1, 0, .5 });
    //    BayesianNetwork.BNetwork UnderstandingNetwork = new BayesianNetwork.BNetwork("UnderstandingNetwork", new List<BayesianNetwork.BNode>() { NumberOfPerfectEdits, NumberOfSyntaxErrors, UnderstandingNode });

    //    Debug.Log("Processing: " + ProcessingNode.CalculateProbability(1));
    //    Debug.Log("Perception: " + PerceptionNode.CalculateProbability(1));
    //    Debug.Log("Understanding: " + UnderstandingNode.CalculateProbability(1));

    //}

    public static void GetValues()
    {
        numAPIOpen                     = LogHelper.AccumulateDataFromAllLevels(PlayerStats.log_numAPIOpen                   );
        numSyntaxErrors                = LogHelper.AccumulateDataFromAllLevels(PlayerStats.log_numSyntaxErrors              );
        numPerfectEdits                = LogHelper.AccumulateDataFromAllLevels(PlayerStats.log_numPerfectEdits              );
        numOfF5                        = LogHelper.AccumulateDataFromAllLevels(PlayerStats.log_numOfF5                      );
        numLegacyCodeViewed            = LogHelper.AccumulateDataFromAllLevels(PlayerStats.log_numLegacyCodeViewed          );
        codeSeen                       = LogHelper.AccumulateDataFromAllLevels(PlayerStats.log_codeSeen                     );
        numQuickDebug                  = LogHelper.AccumulateDataFromAllLevels(PlayerStats.log_numQuickDebug                );
        totalNumDebugs                 = LogHelper.AccumulateDataFromAllLevels(PlayerStats.log_totalNumDebugs               );
        totalNumberOfModifiedEdits     = LogHelper.AccumulateDataFromAllLevels(PlayerStats.log_totalNumberOfModifiedEdits   );
        totalNumberOfLegacyOnly        = LogHelper.AccumulateDataFromAllLevels(PlayerStats.log_totalNumberOfEdits      );
        totalCodeObjects               = LogHelper.AccumulateDataFromAllLevels(PlayerStats.log_totalNumberOfObjects         );
    }

}


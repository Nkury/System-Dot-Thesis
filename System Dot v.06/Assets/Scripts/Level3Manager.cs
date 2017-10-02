using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Level3Manager : LevelHandler
{

    [Header("Miscellaneous")]
    public GameObject intelliSense;

    private Text virusCount;
    int oldVirusCount;
    public void Start()
    {
        virusCount = GameObject.Find("VirusCount").transform.FindChild("Count").GetComponent<Text>();
        base.Start();
        oldVirusCount = int.Parse(virusCount.text);
    }

    public void Update()
    {
      
    }

    public override void LoadLevel()
    {
        // set it to false so we don't have to go through the tutorial
        base.loadedIn = true;
        intelliSense.SetActive(true);

        switch (PlayerStats.checkpoint)
        {
           
        }

        currentCheckpoint = GameObject.Find(PlayerStats.checkpoint);
    }
}

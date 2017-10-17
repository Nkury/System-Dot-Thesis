using UnityEngine;
using System.Collections;

public class TutorialZones : MonoBehaviour
{
    public GameObject intelliSense;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (this.gameObject.name == "FirstTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseTest>().SetDialogue("catchUp1");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if (this.gameObject.name == "SecondTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseTest>().SetDialogue("catchUp2");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "ThirdTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseTest>().SetDialogue("catchUp3");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "FourthTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseTest>().SetDialogue("preHack");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "FifthTutorialObjective")
            {
                intelliSense.SetActive(true);
                intelliSense.GetComponent<IntelliSenseTest>().SetDialogue("meetBlackVBot");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "SixthTutorialObjective")
            {
                intelliSense.SetActive(true);
                intelliSense.GetComponent<IntelliSenseTest>().SetDialogue("startChest");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "SeventhTutorialObjective")
            {
                intelliSense.SetActive(true);
                intelliSense.GetComponent<IntelliSenseTest>().SetDialogue("startDebugStation");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "BossTrigger")
            {
                intelliSense.SetActive(true);
                
                // set up boss dialogue
                intelliSense.SendMessage("StartBoss");
                AudioSource[] aSources = Camera.main.gameObject.GetComponents<AudioSource>();
                aSources[1].mute = true;
                aSources[0].Play();                          

                Destroy(this.gameObject);
            } else if(this.gameObject.name == "gauntlet")
            {
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "GauntletTrigger")
            {
                intelliSense.SetActive(true);

                // set up boss dialogue
                intelliSense.SendMessage("invGlove");

                Destroy(this.gameObject);
            }
        }
    }
}
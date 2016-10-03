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
                intelliSense.SendMessage("StartSecondTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if (this.gameObject.name == "SecondTutorialObjective")
            {
                intelliSense.SendMessage("StartThirdTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "ThirdTutorialObjective")
            {
                intelliSense.SendMessage("StartFourthTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "FourthTutorialObjective")
            {
                intelliSense.SendMessage("StartFifthTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "FifthTutorialObjective")
            {
                intelliSense.SetActive(true);
                intelliSense.SendMessage("StartSixthTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "SixthTutorialObjective")
            {
                intelliSense.SetActive(true);
                intelliSense.SendMessage("StartChestTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "SeventhTutorialObjective")
            {
                intelliSense.SetActive(true);
                intelliSense.SendMessage("StartMovingPlatformTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            }
        }
    }
}
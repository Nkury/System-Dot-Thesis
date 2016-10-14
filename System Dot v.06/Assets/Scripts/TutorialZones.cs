using UnityEngine;
using System.Collections;

public class TutorialZones : MonoBehaviour
{
    public GameObject intelliSense;

    [Header("Level 1 Boss Objects")]
    public GameObject ground1;
    public GameObject ground2;
    public GameObject hallway;

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
            } else if(this.gameObject.name == "BossTrigger")
            {
                intelliSense.SetActive(true);
                // intelliSense.SendMessage("BossStart");

                // set up the boss room
                ground1.SetActive(true);
                ground2.SetActive(true);
                hallway.SetActive(false);

                // set up the boss camera perspective
                Camera.main.orthographicSize = 11;
                Camera.main.GetComponent<CameraController>().yOffset = 9.55f;
                Camera.main.GetComponent<CameraController>().xOffset = 4.5f;
                Camera.main.transform.position = new Vector3(30.36603f, 10.42f, -10);
                Camera.main.GetComponent<CameraController>().isFollowing = false;

                Destroy(this.gameObject);
            }
        }
    }
}
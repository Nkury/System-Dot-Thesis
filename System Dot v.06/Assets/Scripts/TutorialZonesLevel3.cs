using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialZonesLevel3 : MonoBehaviour
{

    public GameObject intelliSense;

    private Text virusCount;
    private bool dontDestroy = false;
    private bool isColliding = false;

    public void Start()
    {
        virusCount = GameObject.Find("VirusCount").transform.FindChild("Count").GetComponent<Text>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (this.gameObject.name == "FirstTutorialObjective")
            {
                if (int.Parse(virusCount.text) <= 16)
                {
                    intelliSense.GetComponent<IntelliSenseLevel3>().SetDialogue("successfulCleansing");
                    dontDestroy = false;
                }
                else
                {
                    intelliSense.GetComponent<IntelliSenseLevel3>().SetDialogue("stillNeedCleansing");
                    dontDestroy = true;
                }
            }
            else if (this.gameObject.name == "SecondTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel3>().SetDialogue("distancePlatform");
            }

            if (!dontDestroy)
            {
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            }
        }
    }
}

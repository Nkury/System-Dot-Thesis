using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialZonesLevel4 : MonoBehaviour
{

    public GameObject intelliSense;

    private bool isColliding = false;

    public void Start()
    {
      
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (this.gameObject.name == "FirstTutorialObjective")
            {              
                intelliSense.GetComponent<IntelliSenseLevel4>().SetDialogue("findIntelliSense");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            }            

        
        } else if(other.tag == "Escort")
        {
            if(this.gameObject.name == "SecondTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel4>().SetDialogue("killedIntelliSense");
            } else if(this.gameObject.name == "ThirdTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel4>().SetDialogue("IntelliSenseMoving");
            } else if(this.gameObject.name == "FourthTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel4>().SetDialogue("IntelliSenseDeathInPit");
            } else if(this.gameObject.name == "FifthTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel4>().SetDialogue("IntelliSenseCharging");
            }

            PlayerStats.deadObjects.Add(this.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;

public class TutorialZones : MonoBehaviour
{
    public GameObject IntelliSense;

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
                IntelliSense.SendMessage("StartSecondTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if (this.gameObject.name == "SecondTutorialObjective")
            {
                IntelliSense.SendMessage("StartThirdTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "ThirdTutorialObjective")
            {
                IntelliSense.SendMessage("StartFourthTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "FourthTutorialObjective")
            {
                IntelliSense.SendMessage("StartFifthTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            } else if(this.gameObject.name == "FifthTutorialObjective")
            {
                IntelliSense.SendMessage("StartSixthTutorial");
                PlayerStats.deadObjects.Add(this.gameObject.name);
                Destroy(this.gameObject);
            }
        }
    }
}
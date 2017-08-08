using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialZonesLevel2 : MonoBehaviour {
    public GameObject intelliSense;  

    void Start()
    {
   
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (this.gameObject.name == "FirstTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("meetFlint");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("IntAPI");       
            }
            else if (this.gameObject.name == "ThirdTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("FlintLeaves");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("FlintLeaves");
                GameObject.Find("CollectZone").GetComponent<CheckZone>().AddParameters();
            }
            else if (this.gameObject.name == "FourthTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("newKernel");            
            }
            else if (this.gameObject.name == "FifthTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("SeeDecInDanger");       
            }
            else if (this.gameObject.name == "SixthTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("MeetDec");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("DoubleAPI");
            }
            else if (this.gameObject.name == "SeventhTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("DecLeaves");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("DecLeaves");
                GameObject.Find("CollectZone").GetComponent<CheckZone>().AddParameters();
            } 
            else if (this.gameObject.name == "EighthTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("meetWord");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("StringAPI");
            }
            else if (this.gameObject.name == "NinthTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("WordLeaves");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("WordLeaves");
                GameObject.Find("CollectZone").GetComponent<CheckZone>().AddParameters();
            }
            else if (this.gameObject.name == "TenthTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("seeBool");
            }
            else if (this.gameObject.name == "EleventhTutorialObjective")
            {
                intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("meetBool");
                intelliSense.GetComponent<IntelliSenseLevel2>().initialEvent("BooleanAPI");
            }

            PlayerStats.deadObjects.Add(this.gameObject.name);
            Destroy(this.gameObject);
        }
    }
}

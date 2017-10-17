using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckZone : MonoBehaviour
{
    public GameObject intelliSense;
    public List<string> listOfVariabulls = new List<string>();
    public int numRescued = 4;

    private GameObject terminalWindow;
    
    // Use this for initialization
    void Start()
    {
        Transform[] trs = GameObject.Find("Main HUD").GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "Terminal Window")
            {
                terminalWindow = t.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        string variabullText = "";

        if (terminalWindow.transform.parent.GetComponent<TerminalWindowUI>().variabullRef.activeSelf)
        {
            variabullText = terminalWindow.transform.parent.GetComponent<TerminalWindowUI>().variaCode.GetComponent<Text>().text;
        }

        switch (variabullText)
        {
            case "int flint = 5;":
                if (numRescued >= 4)
                {
                    numRescued--;
                    listOfVariabulls.Add("int flint = 5; ");                   
                    intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("ActivateFlint");
                }
                break;
            case "double dec = 0.25;":
                if (numRescued >= 3)
                {
                    numRescued--;
                    listOfVariabulls.Add("double dec = 0.25; ");                   
                    intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("DeliverDec");
                }
                break;
            case "string word = \"sentence\";":
                if (numRescued >= 2)
                {
                    numRescued--;
                    listOfVariabulls.Add("string word = \"sentence\"; ");                    
                    intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("deliverWord");
                }
                break;
            case "boolean bool = true;":
                if (numRescued >= 1)
                {
                    numRescued--;                  
                    intelliSense.GetComponent<IntelliSenseLevel2>().SetDialogue("deliverBool");
                }
                break;
        }

        if (numRescued <= 0)
        {
            PlayerStats.deadObjects.Add(this.gameObject.name);
            Destroy(this.gameObject);
        }
    }

    public void AddParameters()
    {
        EnemyTerminal[] enemies = FindObjectsOfType<EnemyTerminal>();
        foreach (EnemyTerminal e in enemies)
        {
            if (!e.gameObject.GetComponent<Gate>())
            {
                string listOfVar = "";
                foreach (string variabull in listOfVariabulls)
                {
                    listOfVar += variabull;
                }
                e.parameters = listOfVar;
            }
        }
    }
}

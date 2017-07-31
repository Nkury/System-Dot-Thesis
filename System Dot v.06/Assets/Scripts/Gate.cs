using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    public List<GameObject> powerSources = new List<GameObject>();
    public GameObject powerDestination;
    public List<string> parameterNames = new List<string>();
    public GameObject connectedTo;

    // Use this for initialization
    void Start() {     
    }
	
	// Update is called once per frame
	void Update () {

        string parameters = "";

        for(int i = 0; i < parameterNames.Count; i++)
        {
            if (powerSources[i].transform.FindChild("StartSource").GetComponent<SpriteRenderer>().color == new Color(1, 1, 0))
            {
                parameters += "boolean " + parameterNames[i] + " = true; ";
            }
            else
            {
                parameters += "boolean " + parameterNames[i] + " = false; ";
            }
        } 

        this.GetComponent<EnemyTerminal>().parameters = parameters;
    }

    public void ActivateDestination(bool isActive)
    {       
        if (isActive)
        {
            powerDestination.transform.FindChild("EndPath").GetComponent<SpriteRenderer>().color = Color.yellow;
            powerDestination.transform.FindChild("EndSource").GetComponent<SpriteRenderer>().color = Color.yellow;
            connectedTo.GetComponent<Activation>().activate();
        }
        else
        {
            powerDestination.transform.FindChild("EndPath").GetComponent<SpriteRenderer>().color = Color.magenta;
            powerDestination.transform.FindChild("EndSource").GetComponent<SpriteRenderer>().color = Color.magenta;
            connectedTo.GetComponent<Activation>().deactivate();
        }           
    }
}

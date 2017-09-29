using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Gate : MonoBehaviour {

    public List<GameObject> powerSources = new List<GameObject>();
    public GameObject powerDestination;
    public List<string> parameterNames = new List<string>();
    public GameObject connectedTo;
    public bool mandatory; // denotes whether or not all parameters must be used

    // Use this for initialization
    void Start() {     
    }
	
	// Update is called once per frame
	void Update () {

        string parameters = "";

        for(int i = 0; i < parameterNames.Count; i++)
        {
            if (powerSources.Count > 0)
            {
                if (powerSources[i].gameObject.name.Contains("PowerSource"))
                {
                    if (powerSources[i].transform.FindChild("StartSource").GetComponent<SpriteRenderer>().color == Color.yellow)
                    {
                        parameters += "boolean " + parameterNames[i] + " = true; ";
                    }
                    else
                    {
                        parameters += "boolean " + parameterNames[i] + " = false; ";
                    }
                }
                else if (powerSources[i].gameObject.name.Contains("PowerDestination"))
                {
                    if (powerSources[i].transform.FindChild("EndSource").GetComponent<SpriteRenderer>().color == Color.yellow)
                    {
                        parameters += "boolean " + parameterNames[i] + " = true; ";
                    }
                    else
                    {
                        parameters += "boolean " + parameterNames[i] + " = false; ";
                    }
                }
            }
        } 

        this.GetComponent<EnemyTerminal>().parameters = parameters;
    }

    public void ActivateDestination(bool isActive)
    {
        bool canActivate = true;

        if (mandatory && isActive)
        {
            foreach (string pname in parameterNames)
            {
                bool isInThere = false;

                foreach (string s in this.GetComponent<EnemyTerminal>().terminalString)
                {
                    if (s.Contains("System.output(" + pname + ")"))
                    {
                        isInThere = true;
                    }
                    else
                    {
                        string[] stringParts = s.Split(' ');
                        foreach (string subString in stringParts)
                        {
                            string tempString;
                            Regex rgx = new Regex("[^a-zA-Z0-9]");
                            tempString = rgx.Replace(subString, "");
                            if (tempString == pname)
                            {
                                isInThere = true;
                            }
                        }
                    }
                }

                if (!isInThere)
                {
                    canActivate = false;
                }
            }
        }

        if (canActivate)
        {
            foreach (Transform child in powerDestination.transform)
            {
                if (child.gameObject.name.Contains("EndPath") || child.gameObject.name.Contains("EndSource"))
                {
                    if (isActive)
                    {
                        child.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                        child.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                        if (connectedTo != null)
                        {
                            connectedTo.GetComponent<Activation>().activate();
                        }
                    }
                    else
                    {
                        child.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                        child.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
                        if (connectedTo != null)
                        {
                            connectedTo.GetComponent<Activation>().deactivate();
                        }
                    }
                }
            }
        }
    }
}

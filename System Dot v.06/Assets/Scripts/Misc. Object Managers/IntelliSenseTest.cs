using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class IntelliSenseTest : MonoBehaviour {

    XmlTextReader reader;
    Dictionary<string, List<string>> dialogue = new Dictionary<string, List<string>>();

	// Use this for initialization
	void Start () {
        reader = new XmlTextReader("TutorialDialogue.xml");
        reader.WhitespaceHandling = WhitespaceHandling.None;

        List<string> tempDialogue = new List<string>();
        string keyName = "";

        while (reader.Read())
        {
         
            Debug.Log(reader.NodeType + " " + reader.Name + " " + reader.Value);

            if(reader.AttributeCount > 0)
            {
                // adds a new entry into the dictionary with attribute as the key
                while (reader.MoveToNextAttribute())
                {
                    keyName = reader.Name;
                    Debug.Log("ATTRIBUTE " + reader.NodeType + " " + reader.Name + " " + reader.Value);
                }
            }

            if (reader.NodeType == XmlNodeType.Element && reader.Name == "message")
            {
                tempDialogue = new List<string>();
                keyName = "";
            }

            if(reader.Name == "say")
            {
                Debug.Log("VALUE IS " + reader.Value);
                tempDialogue.Add(reader.Value);
            }

            if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "message")
            {
                dialogue.Add(keyName, tempDialogue);
            }
        }

        Debug.Log(dialogue);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

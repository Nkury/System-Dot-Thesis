using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Destroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    foreach(string name in PlayerStats.deadObjects)
        {
            Destroy(GameObject.Find(name));
        }

        EnemyTerminal[] enemies = FindObjectsOfType<EnemyTerminal>();
        foreach (EnemyTerminal e in enemies)
        {
            List<string> terminalString = new List<string>();
            if(PlayerStats.terminalStrings.TryGetValue(e.gameObject.name, out terminalString))
            {
                if (!terminalString.SequenceEqual(e.terminalString))
                {
                    e.terminalString = terminalString.ToArray();
                    e.checkTerminalString();
                    StartCoroutine(e.evaluateActions());
                }               
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

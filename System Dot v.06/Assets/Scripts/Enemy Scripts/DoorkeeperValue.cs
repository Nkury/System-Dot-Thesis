using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DoorkeeperValue : MonoBehaviour {

    public InputField answer;
    public Doorkeeper[] doorkeepers;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        doorkeepers = GameObject.FindObjectsOfType<Doorkeeper>();
    }

    public void sayPressed()
    {
        foreach (Doorkeeper dk in doorkeepers)
        {
            if (dk.showBox)
            {
                try
                {
                    dk.inputValue = Int32.Parse(answer.text);
                }
                catch (FormatException)
                {
                    dk.inputValue = -1;
                }
            }
        }
    }

}

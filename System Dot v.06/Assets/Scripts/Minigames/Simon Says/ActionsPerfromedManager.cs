using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActionsPerfromedManager : MonoBehaviour {

    private int actions;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    public void addAction()
    {
        actions++;
    }

    public void resetActions()
    {
        actions = 0;
    }

    public int getActions()
    {
        return actions;
    }
}

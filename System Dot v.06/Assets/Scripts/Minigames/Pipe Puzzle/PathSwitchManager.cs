using UnityEngine;
using System.Collections;

public class PathSwitchManager : MonoBehaviour {

    public PipeManager pipeManager;

    public bool pathSwitched;

	// Use this for initialization
	void Start () {
        pathSwitched = false;
        pipeManager = FindObjectOfType<PipeManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Debris")
        {
            pathSwitched = true;
        }
    }


    public bool truePipeChosen()
    {
        return pipeManager.getPipe();
    }
}

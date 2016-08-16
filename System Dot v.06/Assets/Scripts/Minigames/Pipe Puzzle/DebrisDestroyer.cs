using UnityEngine;
using System.Collections;

public class DebrisDestroyer : MonoBehaviour {
    
    public PathSwitchManager pathSwitch;
    public PipeScoreManager scoreManager;

    public bool destroyed;

	// Use this for initialization
	void Start () {
	    pathSwitch = FindObjectOfType<PathSwitchManager>();
        scoreManager = FindObjectOfType<PipeScoreManager>();
        destroyed = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Debris")
        {
            Destroy(other.gameObject);
            destroyed = true;
            pathSwitch.pathSwitched = false;
            scoreManager.scoreCheck();


        }
    }
}

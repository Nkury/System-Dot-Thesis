using UnityEngine;
using System.Collections;

public class DebrisMovement : MonoBehaviour {

    public int spawnSpeed;
    public int quickSpeed;

    private int currentSpeed;

    public PathSwitchManager pathSwitch;

	// Use this for initialization
	void Start () {
        pathSwitch = FindObjectOfType<PathSwitchManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!pathSwitch.pathSwitched)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - (currentSpeed * Time.deltaTime));
        }
        else if(pathSwitch.truePipeChosen())
        {
            transform.position = new Vector2(transform.position.x - (currentSpeed * Time.deltaTime), transform.position.y - (currentSpeed * Time.deltaTime));
        }
        else
        {
            transform.position = new Vector2(transform.position.x + (currentSpeed * Time.deltaTime), transform.position.y - (currentSpeed * Time.deltaTime));
        }

        if(Input.GetKey(KeyCode.Return))
        {
            currentSpeed = quickSpeed;
        }
        else
        {
            currentSpeed = spawnSpeed;
        }

       
	}

 
    
}

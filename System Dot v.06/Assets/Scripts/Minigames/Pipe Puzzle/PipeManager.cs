using UnityEngine;
using System.Collections;

public class PipeManager : MonoBehaviour {

    public GameObject truePipe;
    public GameObject falsePipe;

    public PathSwitchManager pathSwitch;
    

    public int count;
    private bool truePipeActive;

    public AudioSource audio;

	// Use this for initialization
	void Start () {
        count = 0;
        truePipeActive = true;

        pathSwitch = FindObjectOfType<PathSwitchManager>();
        audio = GetComponent<AudioSource>();

       
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Space) && !pathSwitch.pathSwitched)
        {
            audio.Play();
            if (count % 2 == 0)
            {
                truePipe.SetActive(false);
                falsePipe.SetActive(true);
                truePipeActive = false;
            }
            else
            {
                truePipe.SetActive(true);
                falsePipe.SetActive(false);
                truePipeActive = true;
            }
            count++;
        }

	}

    public bool getPipe()
    {
        return truePipeActive;
    }
}

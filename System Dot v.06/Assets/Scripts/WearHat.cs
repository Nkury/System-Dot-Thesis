using UnityEngine;
using System.Collections;

public class WearHat : MonoBehaviour {

    public Sprite cowboy;
    SoundController sound;
	// Use this for initialization
	void Start () {
        sound = FindObjectOfType<SoundController>();
	}
	
	// Update is called once per frame
	void Update () {
        string currentHat = PlayerStats.hat;
	    if(PlayerStats.hat != null && PlayerStats.hat.Equals("cowboy"))
        {
            this.GetComponent<SpriteRenderer>().sprite = cowboy;
        }
	}
}

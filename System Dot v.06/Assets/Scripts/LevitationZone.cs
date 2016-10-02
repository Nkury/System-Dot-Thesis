using UnityEngine;
using System.Collections;

public class LevitationZone : MonoBehaviour {

    public float flightSpeed = 1.1f;
    public PlayerController player;
    private bool flying;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        flying = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(flying)
        {
            player.transform.position  = new Vector2(transform.position.x, transform.position.y - (flightSpeed * Time.deltaTime));
        }
	}

    void onTriggerEnter2d(Collider2D other)
    {
        if(other.tag == "Player")
        {
            flying = true;
        }
    }

    void onTriggerExit2d(Collider2D other)
    {
        flying = false;
    }
}

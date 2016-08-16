using UnityEngine;
using System.Collections;

public class JumpOverEnemyCheck : MonoBehaviour {

    private PlayerController player;
    private SimonController simon;
    private SimonTimer timer;

    private SpawnController spawner;
    private ActionsPerfromedManager actionManager;

    public GameObject deathEffect;

   

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
        simon = FindObjectOfType<SimonController>();
        spawner = FindObjectOfType<SpawnController>();
        timer = FindObjectOfType<SimonTimer>();
        actionManager = FindObjectOfType<ActionsPerfromedManager>();
        
	}
	
	// Update is called once per frame
	void Update () {
	    if(timer.isTimeOver())
        {
            Destroy(transform.parent.gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Player")
        {
            if(simon.getTask() == SimonController.JUMP_OVER_ENEMY)
            {
                actionManager.addAction();
                Instantiate(deathEffect, transform.parent.position, transform.parent.rotation);
                Destroy(transform.parent.gameObject);

                if (!(timer.isTimeOver()))
                {
                    spawner.spawnRandomEnemy();
                }
            }
            
            

        }

       

    }
}

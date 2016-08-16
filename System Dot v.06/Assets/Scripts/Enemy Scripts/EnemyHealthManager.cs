using UnityEngine;
using System.Collections;

public class EnemyHealthManager : MonoBehaviour {

    public int enemyHealth;

    public GameObject deathEffect;

    public int pointsOnDeath;

    private SimonController simon;
    private ActionsPerfromedManager actions;
    private SpawnController spawner;
    private SimonTimer timer;

	// Use this for initialization
	void Start () {
        simon = FindObjectOfType<SimonController>();
        actions = FindObjectOfType<ActionsPerfromedManager>();
        spawner = FindObjectOfType<SpawnController>();
        timer = FindObjectOfType<SimonTimer>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(enemyHealth <= 0)
        {
            if(simon != null && simon.getTask() == SimonController.KILL_ENEMY)
            {
                actions.addAction();

                if (!(timer.isTimeOver()))
                {
                    spawner.spawnRandomEnemy();
                }
            }
            Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
	}

    public void giveDamage(int damageToGive)
    {
        enemyHealth -= damageToGive;
        GetComponent<AudioSource>().Play();
    }
}

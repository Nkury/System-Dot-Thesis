using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealthManager : MonoBehaviour {

    public int enemyHealth;

    public GameObject deathEffect;
    public GameObject bit;

    public int bitsOnDeath;


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

            if(this.gameObject.tag == "Centipede Body")
            {
                CentipedeHead.lives--;
                GameObject.Find("Boss Health").GetComponentInChildren<Slider>().value -=1;
            }

            Instantiate(deathEffect, transform.position, transform.rotation);
            for(int i = 0; i < bitsOnDeath; i++)
            {
                Instantiate(bit, transform.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f)), transform.rotation);
            }

            PlayerStats.deadObjects.Add(this.gameObject.name);
            Destroy(gameObject);
        }
	}

    public void giveDamage(int damageToGive)
    {
        enemyHealth -= damageToGive;
        GetComponent<AudioSource>().Play();
    }
}

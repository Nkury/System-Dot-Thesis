using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level3BossBehavior : MonoBehaviour {

    public GameObject[] pipeExits = new GameObject[4];
    public GameObject bossAttack;
    public GameObject numberBall;
    public ParticleSystem spawnNumberBall;
    public Slider bossHealth;

    public float moveSpeed;
    public float amplitude;
    public float baseX;
    public float spawnYDown;

    public int health;

    public float intervalToSpike;
    public float intervalToSpawn;


    private float time = 0;
    private float spawnTime = 0;

    private float midpoint1;
    private float midpoint2;
    private float midpoint3;
    private float chosenMidpoint;

    private bool isAttacking;
    private bool isSpawning;

    // Use this for initialization
    void Start () {
        midpoint1 = (pipeExits[1].transform.position.x + pipeExits[0].transform.position.x) / 2;
        midpoint2 = (pipeExits[2].transform.position.x + pipeExits[1].transform.position.x) / 2;
        midpoint3 = (pipeExits[3].transform.position.x + pipeExits[2].transform.position.x) / 2;
    }
	
	// Update is called once per frame
	void Update () {
      
        transform.position = new Vector2(baseX + amplitude * Mathf.Sin(moveSpeed * Time.time), transform.position.y);

        if (isSpawning && this.transform.position.x <= closestDistance() + .1f && this.transform.position.x >= closestDistance() - .1f)
        {
            transform.position = new Vector2(closestDistance(), this.transform.position.y);
        }
        
        if (health != this.GetComponent<EnemyHealthManager>().enemyHealth)
        {
            health = this.GetComponent<EnemyHealthManager>().enemyHealth;
            if (health <= 4)
            {
                //SetUp();
                //StartCoroutine(AttackPhase(health));
            }
        }
        
        bossHealth.value = health;
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        spawnTime += Time.deltaTime;

        if (time >= intervalToSpike)
        {
            Spike();
        }

        if (spawnTime >= intervalToSpawn)
        {
            isSpawning = true;
            if (spawnTime >= intervalToSpawn + 3)
            {
                SpawnNumberBall();
            }
        }

    }

    public float closestDistance()
    {
        float[] values =
        {
            Mathf.Abs(this.transform.position.x - midpoint1),
            Mathf.Abs(this.transform.position.x - midpoint2),
            Mathf.Abs(this.transform.position.x - midpoint3)
        };

        float[] midpoints =
        {
            midpoint1,
            midpoint2,
            midpoint3
        };

        float min = Mathf.Min(values);
        int indexOfMidpoint = Array.IndexOf(values, min);
        chosenMidpoint = midpoints[indexOfMidpoint];
        return midpoints[indexOfMidpoint];
    }

    public void SpawnNumberBall()
    {
        GameObject particleSys = Instantiate(spawnNumberBall.gameObject, this.transform.position, spawnNumberBall.gameObject.transform.rotation);
        GameObject numBall = Instantiate(numberBall, this.transform.position - new Vector3(0, spawnYDown), numberBall.transform.rotation);
        spawnTime = 0;
        isSpawning = false;
    }
    
    public void Spike()
    {
        GameObject obj = Instantiate(bossAttack, this.transform.position, bossAttack.transform.rotation);    
        time = 0;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<KillPlayer>() && other.gameObject.tag == "PipeObject")
        {
            this.GetComponent<EnemyHealthManager>().giveDamage(1);    
        }
    }


}

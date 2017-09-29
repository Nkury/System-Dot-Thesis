using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level3BossBehavior : MonoBehaviour {

    public GameObject[] pipeExits = new GameObject[4];
    public GameObject bossAttack;
    public ParticleSystem spawnNumberBall;
    public Slider bossHealth;

    public float moveSpeed;
    public float amplitude;
    public float baseX;

    public int health;
    public float intervalToSpike;


    private float time = 0;

    private float midpoint1;
    private float midpoint2;
    private float midpoint3;

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
        if (time >= intervalToSpike)
        {
            Spike();
        }
        
    }

    public void SpawnNumberBall()
    {
        isSpawning = true;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level3BossBehavior : MonoBehaviour {

    public GameObject[] pipeExits = new GameObject[4];
    public GameObject bossAttack;
    public GameObject numberBall;
    public GameObject bossPlatform;
    public ParticleSystem spawnNumberBall;
    public Slider bossHealth;
    

    public float moveSpeed;
    public float amplitude;
    public float baseX;
    public float spawnYDown;

    public int health;

    public float intervalToSpike;
    public float intervalToSpawn;
    public float intervalToDropNumBall;

    private GameObject numBall;
    private int randomPipeExit;

    private float time = 0;
    private float spawnTime = 0;
    private float dropTime = 0;

    private float midpoint;
    private float chosenMidpoint;

    private bool isAttacking;
    private bool isSpawning;
    private bool isSpiking;

    // Use this for initialization
    void Start()
    {
        midpoint = (pipeExits[2].transform.position.x + pipeExits[1].transform.position.x) / 2;
    }
	
	// Update is called once per frame
	void Update () {

        if (isAttacking)
        {

        }
        else if (numBall)
        {
            transform.position = Vector2.MoveTowards(this.transform.position,
                new Vector2(pipeExits[randomPipeExit].transform.position.x, this.transform.position.y), 
                moveSpeed * Time.time);            
        }
        else if (isSpawning)
        {
            transform.position = Vector2.Lerp(this.transform.position, new Vector2(midpoint, this.transform.position.y), 2* moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector2(baseX + amplitude * Mathf.Sin(moveSpeed * Time.time), transform.position.y);
        }

        if (health != this.GetComponent<EnemyHealthManager>().enemyHealth)
        {
            health = this.GetComponent<EnemyHealthManager>().enemyHealth;
            if (health <= 4)
            {
                isAttacking = true;
                //SetUp();
                StartCoroutine(AttackPhase(health));
            }
        }
        
        bossHealth.value = health;
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        spawnTime += Time.deltaTime;
        dropTime += Time.deltaTime;


        if (time >= intervalToSpike)
        {
            Spike();
        }

        if (!isAttacking && !isSpawning && !isSpiking)
        {
            isSpiking = true;
            StartCoroutine(SpikePattern(health));
        }

        if (spawnTime >= intervalToSpawn && !isSpawning && !isAttacking && numBall == null)
        {
            isSpawning = true;          
            StartCoroutine(SpawnNumberBall());
        }

        if(dropTime >= intervalToDropNumBall && numBall != null)
        {
            numBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

    }

    //public float closestDistance()
    //{
    //    float[] values =
    //    {
    //        Mathf.Abs(this.transform.position.x - midpoint1),
    //        Mathf.Abs(this.transform.position.x - midpoint2),
    //        Mathf.Abs(this.transform.position.x - midpoint3)
    //    };

    //    float[] midpoints =
    //    {
    //        midpoint1,
    //        midpoint2,
    //        midpoint3
    //    };

    //    float min = Mathf.Min(values);
    //    int indexOfMidpoint = Array.IndexOf(values, min);
    //    chosenMidpoint = midpoints[indexOfMidpoint];
    //    return midpoints[indexOfMidpoint];
    //}
    public IEnumerator AttackPhase(int health)
    {
        switch (health)
        {
            case 4:
                bossPlatform.AddComponent<Rotater>();
                Rotate(true, 60);
                yield return new WaitForSeconds(5);
                Rotate(false, 60);
                yield return new WaitForSeconds(5);
                Destroy(bossPlatform.GetComponent<Rotater>());
                bossPlatform.transform.rotation = new Quaternion(0, 0, 0, 0);
                break;
            case 3:
                break;
            case 2:
                break;
            case 1:
                break;
        }

        yield return new WaitForSeconds(1);
        spawnTime = 0;        
    }

    public void Rotate(bool goLeft, float speed)
    {
        bossPlatform.GetComponent<Rotater>().goLeft = goLeft;
        bossPlatform.GetComponent<Rotater>().maxRotation = goLeft ? 1000000 : -100000;
        bossPlatform.GetComponent<Rotater>().rotationSpeed = speed;
        bossPlatform.GetComponent<Rotater>().pause = false;
    }

    public IEnumerator SpikePattern(int health)
    {

        switch (health)
        {
            case 5:
                StartCoroutine(MegaAttack(2, 1, .5f, 3));
                yield return new WaitForSeconds(9);
                break;
            case 4:
                break;
            case 3:
                break;
            case 2:
                break;
            case 1:
                break;
        }

        yield return new WaitForSeconds(.2f);
        isSpiking = false;

    }

    public IEnumerator MegaAttack(float on, float off, float offTime, int numTimes)
    {
        for (int i = 0; i < numTimes; i++)
        {
            intervalToSpike = 0;
            yield return new WaitForSeconds(on);
            intervalToSpike = offTime;
            yield return new WaitForSeconds(off);
        }
    }

    public IEnumerator SpawnNumberBall()
    {
        yield return new WaitForSeconds(1);
        GameObject particleSys = Instantiate(spawnNumberBall.gameObject, this.transform.position, spawnNumberBall.gameObject.transform.rotation);
        yield return new WaitForSeconds(2);
        numBall = Instantiate(numberBall, this.transform.position - new Vector3(0, spawnYDown), numberBall.transform.rotation);
        numBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        dropTime = 0;
        if(health == 5)
        {
            randomPipeExit = UnityEngine.Random.Range(1, 3);
        }
        else
        {
            randomPipeExit = UnityEngine.Random.Range(0, 4);
        }
        yield return new WaitForSeconds(3);
        Destroy(particleSys);
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

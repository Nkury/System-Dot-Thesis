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
    public float upSpeed;
    public float amplitude;
    public float baseX;
    public float spawnYDown;
    public float restingY;

    public float restingYLower;
    public float restingYMid;
    public float restingYTop;
    public float bossPlatformYLower;
    public float bossPlatformYMid;
    public float bossPlatformYTop;

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
    private float currRestingY;

    private bool isAttacking;
    private bool isSpawning;
    private bool isSpiking;
    private bool isWaiting;

    private TiltPlatform[] tiltPlatforms;

    // Use this for initialization
    void Start()
    {
        midpoint = (pipeExits[2].transform.position.x + pipeExits[1].transform.position.x) / 2;
        tiltPlatforms = GameObject.FindObjectsOfType<TiltPlatform>();
        currRestingY = restingYLower;
    }
	
	// Update is called once per frame
	void Update () {

        if (isAttacking)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, 
                new Vector2(midpoint, restingY),
                upSpeed * Time.deltaTime);
        }
        else if (numBall)
        {
            transform.position = Vector2.MoveTowards(this.transform.position,
                new Vector2(pipeExits[randomPipeExit].transform.position.x, this.transform.position.y), 
                moveSpeed * Time.deltaTime);            
        }
        else if (isSpawning)
        {
            transform.position = Vector2.Lerp(this.transform.position, new Vector2(midpoint, this.transform.position.y), 2* moveSpeed * Time.deltaTime);
        }
        else
        {
            isWaiting = false;
            transform.position = new Vector2(baseX + amplitude * Mathf.Sin(moveSpeed * Time.time), currRestingY);
        }

        if (health != this.GetComponent<EnemyHealthManager>().enemyHealth)
        {
            health = this.GetComponent<EnemyHealthManager>().enemyHealth;
            if (health <= 4)
            {
                isAttacking = true;
                SetUp();
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

        if (!isAttacking && !isSpawning && !isSpiking && !isWaiting)
        {
            isSpiking = true;
            StartCoroutine(SpikePattern(health));
        }

        if (spawnTime >= intervalToSpawn && !isSpawning && !isAttacking && numBall == null)
        {
            isSpawning = true;          
            StartCoroutine(SpawnNumberBall());
        }

        if(dropTime >= intervalToDropNumBall && numBall != null && !isWaiting)
        {
            numBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            isWaiting = true;
            dropTime = 0;            
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

    public void SetUp()
    {
        foreach(TiltPlatform tp in tiltPlatforms)
        {
            tp.ClearCondition();
            tp.GetComponent<SpriteRenderer>().enabled = true;
            tp.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public IEnumerator AttackPhase(int health)
    {
        bossPlatform.AddComponent<Rotater>();
        switch (health)
        {
            case 4:                
                Rotate(true, 40);
                yield return new WaitForSeconds(5);
                Rotate(false, 40);
                yield return new WaitForSeconds(5);
                bossPlatform.transform.position = new Vector2(bossPlatform.transform.position.x, bossPlatformYMid);
                currRestingY = restingYMid;
                break;
            case 3:
                Rotate(true, 40);
                yield return new WaitForSeconds(5);
                Rotate(false, 40);
                yield return new WaitForSeconds(5);
                bossPlatform.transform.position = new Vector2(bossPlatform.transform.position.x, bossPlatformYMid);
                currRestingY = restingYMid;
                break;
            case 2:
                Rotate(true, 40);
                yield return new WaitForSeconds(5);
                Rotate(false, 40);
                yield return new WaitForSeconds(5);
                bossPlatform.transform.position = new Vector2(bossPlatform.transform.position.x, bossPlatformYTop);
                currRestingY = restingYTop;
                break;
            case 1:
                Rotate(true, 40);
                yield return new WaitForSeconds(5);
                Rotate(false, 40);
                yield return new WaitForSeconds(5);
                bossPlatform.transform.position = new Vector2(bossPlatform.transform.position.x, bossPlatformYTop);
                currRestingY = restingYTop;
                break;
        }
        Destroy(bossPlatform.GetComponent<Rotater>());
        bossPlatform.transform.rotation = new Quaternion(0, 0, 0, 0);
        bossPlatform.SetActive(false);
        ClearTiltPlatforms();
        SetTiltPlatforms();
        yield return new WaitForSeconds(3);        
        bossPlatform.SetActive(true);
        spawnTime = 0;
        isAttacking = false;
    }

    public void ClearTiltPlatforms()
    {
        foreach(TiltPlatform tp in tiltPlatforms)
        {
            tp.GetComponent<SpriteRenderer>().enabled = false;
            tp.GetComponent<BoxCollider2D>().enabled = false;
            tp.GetComponent<EnemyTerminal>().numOfLegacy[0] = true;
            tp.GetComponent<EnemyTerminal>().terminalString[0] = "";
        }
    }

    public void SetTiltPlatforms()
    {
        foreach (TiltPlatform tp in tiltPlatforms)
        {
            string name = tp.gameObject.name;
            switch (health)
            {
                case 4:
                    if(name.Contains("LL"))
                    {
                        tp.GetComponent<SpriteRenderer>().enabled = true;
                        tp.GetComponent<BoxCollider2D>().enabled = true;
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\"< 4\");";
                        tp.condition = "< 4";
                    } else if (name.Contains("LM"))
                    {
                        tp.GetComponent<SpriteRenderer>().enabled = true;
                        tp.GetComponent<BoxCollider2D>().enabled = true;
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\"<= 3\");";
                        tp.condition = "<= 3";
                    } else if (name.Contains("ML"))
                    {
                        tp.GetComponent<SpriteRenderer>().enabled = true;
                        tp.GetComponent<BoxCollider2D>().enabled = true;
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\">= 2\");";
                        tp.condition = ">= 2";
                    }
                    break;
                case 3:
                    if (name.Contains("LR"))
                    {
                        tp.GetComponent<SpriteRenderer>().enabled = true;
                        tp.GetComponent<BoxCollider2D>().enabled = true;
                        tp.GetComponent<EnemyTerminal>().numOfLegacy[0] = false;
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "";
                        tp.condition = "";
                    }
                    else if (name.Contains("LM"))
                    {
                        tp.GetComponent<SpriteRenderer>().enabled = true;
                        tp.GetComponent<BoxCollider2D>().enabled = true;
                        tp.GetComponent<EnemyTerminal>().numOfLegacy[0] = false;
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "";
                        tp.condition = "";
                    }
                    else if (name.Contains("MR"))
                    {
                        tp.GetComponent<SpriteRenderer>().enabled = true;
                        tp.GetComponent<BoxCollider2D>().enabled = true;
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\"> 0\");";
                        tp.condition = "> 0";
                    }
                    break;
                case 2:
                    tp.GetComponent<SpriteRenderer>().enabled = true;
                    tp.GetComponent<BoxCollider2D>().enabled = true;
                    if (name.Contains("LL"))
                    {
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\"> 7\");";
                        tp.condition = "> 7";
                    } else if (name.Contains("LM"))
                    {
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\">= 3\");";
                        tp.condition = ">= 3";
                    } else if (name.Contains("LR"))
                    {
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\"< -3\");";
                        tp.condition = "< -3";
                    } else if (name.Contains("ML"))
                    {
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\"< 4\");";
                        tp.condition = "< 4";
                    } else if (name.Contains("MR"))
                    {
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\"> -5\");";
                        tp.condition = "> -5";
                    } else if (name.Contains("TM"))
                    {
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\"< 0\");";
                        tp.condition = "< 0";
                    }
                    break;
                case 1:
                    tp.GetComponent<SpriteRenderer>().enabled = true;
                    tp.GetComponent<BoxCollider2D>().enabled = true;
                    if (name.Contains("LL"))
                    {
                        tp.GetComponent<EnemyTerminal>().numOfLegacy[0] = false;
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "";
                        tp.condition = "";
                    }
                    else if (name.Contains("LM"))
                    {
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\"< 0\");";
                        tp.condition = "< 0";
                    }
                    else if (name.Contains("LR"))
                    {
                        tp.GetComponent<EnemyTerminal>().numOfLegacy[0] = false;
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "";
                        tp.condition = "";
                    }
                    else if (name.Contains("ML"))
                    {
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\">= -5\");";
                        tp.condition = ">= -5";
                    }
                    else if (name.Contains("MR"))
                    {
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(\"< 5\");";
                        tp.condition = "< 5";
                    }
                    else if (name.Contains("TM"))
                    {
                        tp.GetComponent<EnemyTerminal>().numOfLegacy[0] = false;
                        tp.GetComponent<EnemyTerminal>().terminalString[0] = "";
                        tp.condition = "";
                    }
                    break;
            }            
        }
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
            case 4:
                break;
            case 3:
                break;
            case 2:
                break;
            case 1:
                StartCoroutine(MegaAttack(2, 1, .5f, 3));
                yield return new WaitForSeconds(9);
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
        if(health == 4 )
        {
            midpoint = pipeExits[1].transform.position.x;
        } else if(health == 3)
        {
            midpoint = pipeExits[2].transform.position.x;
        }
        else
        {
            midpoint = (pipeExits[2].transform.position.x + pipeExits[1].transform.position.x) / 2;
        }
        yield return new WaitForSeconds(1);
        GameObject particleSys = Instantiate(spawnNumberBall.gameObject, this.transform.position, spawnNumberBall.gameObject.transform.rotation);
        yield return new WaitForSeconds(2);
        numBall = Instantiate(numberBall, this.transform.position - new Vector3(0, spawnYDown), numberBall.transform.rotation);
        numBall.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        dropTime = 0;
        switch (health)
        {
            case 5:
                randomPipeExit = UnityEngine.Random.Range(1, 3);
                numBall.GetComponent<NumberBall>().character = UnityEngine.Random.Range(0, 7).ToString();
                break;
            case 4:
                randomPipeExit = UnityEngine.Random.Range(0, 3);
                numBall.GetComponent<NumberBall>().character = UnityEngine.Random.Range(0, 6).ToString();
                break;
            case 3:
                randomPipeExit = UnityEngine.Random.Range(1, 4);
                numBall.GetComponent<NumberBall>().character = UnityEngine.Random.Range(-5, 6).ToString();
                numBall.GetComponent<NumberBall>().canClick = false;
                break;
            case 2:
                randomPipeExit = UnityEngine.Random.Range(0, 4);
                numBall.GetComponent<NumberBall>().character = UnityEngine.Random.Range(-5, 5).ToString();
                break;
            case 1:
                randomPipeExit = UnityEngine.Random.Range(0, 4);
                numBall.GetComponent<NumberBall>().character = UnityEngine.Random.Range(-10, 11).ToString();
                numBall.GetComponent<NumberBall>().canClick = false;
                break;
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
        if (other.GetComponent<KillPlayer>() && other.transform.parent != null && other.transform.parent.gameObject.tag == "PipeObject")
        {
            Destroy(other.transform.parent.gameObject);
            this.GetComponent<EnemyHealthManager>().giveDamage(1);
            spawnTime = 0;   
        }
    }
}

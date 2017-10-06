using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {
    
    public List<GameObject> enemiesSpawned = new List<GameObject>();

    public int totalEnemiesSpawned = 0;
    public int numOfEnemiesToSpawn = 0;
    public int numEnemiesKilled = 0;

    public float minSpeed;
    public float maxSpeed;

    public GameObject spawnArea;
    public GameObject enemyToSpawn;
    public GameObject enemyCount;

    public float timeBetweenSpawn;


    private string[] enemyColors = { "BLUE", "GREEN", "RED" };
    private bool startSpawn = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        for(int i = 0; i < enemiesSpawned.Count; i++)
        {
            if(enemiesSpawned[i] == null)
            {
                enemiesSpawned.RemoveAt(i);
                numEnemiesKilled++;
                enemyCount.GetComponent<TextMesh>().text = (totalEnemiesSpawned - numEnemiesKilled).ToString();
            }

            if(numEnemiesKilled == totalEnemiesSpawned)
            {
                this.GetComponent<EnemyHealthManager>().enemyHealth = 0;
            }
        }


        if (startSpawn)
        {
            if (numOfEnemiesToSpawn > 0)
            {
                StartCoroutine(SpawnEnemy());
            }
        }


	}

    public IEnumerator SpawnEnemy()
    {
        startSpawn = false;
        enemyToSpawn.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(Color." + enemyColors[Random.Range(0, 3)] + ");";

        enemyToSpawn.GetComponent<EnemyTerminal>().terminalString[1] = "System.move(Direction." + 
            (spawnArea.transform.position.x < transform.position.x ? "LEFT" : "RIGHT") + ");";

        enemyToSpawn.GetComponent<EnemyTerminal>().moveSpeed = Random.Range(minSpeed, maxSpeed);

        enemiesSpawned.Add(Instantiate(enemyToSpawn, spawnArea.transform.position, enemyToSpawn.transform.rotation));

        yield return new WaitForSeconds(timeBetweenSpawn);

        numOfEnemiesToSpawn--;
        startSpawn = true;

    }

    public void SetNumEnemies(int numToSet)
    {
        totalEnemiesSpawned += numToSet;
        numOfEnemiesToSpawn += numToSet;
        enemyCount.GetComponent<TextMesh>().text = numOfEnemiesToSpawn.ToString();
    }
}

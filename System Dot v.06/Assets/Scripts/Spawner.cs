using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    List<GameObject> enemiesSpawned = new List<GameObject>();

    public int numOfEnemiesToSpawn;

    public GameObject spawnArea;
    public GameObject enemyToSpawn;
    public float timeBetweenSpawn;

    private string[] enemyColors = { "BLUE", "GREEN", "RED" };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(timeBetweenSpawn);
        enemyToSpawn.GetComponent<EnemyTerminal>().terminalString[0] = "System.body(Color." + enemyColors[Random.Range(0, 3)] + ");";

        enemyToSpawn.GetComponent<EnemyTerminal>().terminalString[1] = "System.move(Direction." + 
            (spawnArea.transform.position.x < transform.position.x ? "LEFT" : "RIGHT") + ");";

        enemiesSpawned.Add(Instantiate(enemyToSpawn, spawnArea.transform.position, enemyToSpawn.transform.rotation));
        
    }
}

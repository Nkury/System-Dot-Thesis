using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {

    public GameObject idleEnemy;
    public GameObject roamingEnemy;
    public GameObject coin;
    public GameObject spawn0;
    public GameObject spawn1;
    public GameObject spawn2;

    private GameObject spawnZone;
    private int lastSpawn;


    System.Random rng;


	// Use this for initialization
	void Start () {
        rng = new System.Random();
        lastSpawn = -1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject randomSpawn()
    {
        GameObject chosenSpawn = null;
        switch(rng.Next(0,3))
        {
            case 0:
                {
                    if (lastSpawn != 0)
                    {
                        chosenSpawn = spawn0;
                        lastSpawn = 0;
                    }
                    else
                    {
                        chosenSpawn = spawn1;
                        lastSpawn = 1;
                    }
                    break;
                }
            case 1:
                {
                    if (lastSpawn != 1)
                    {
                        chosenSpawn = spawn1;
                        lastSpawn = 1;
                    }
                    else
                    {
                        chosenSpawn = spawn2;
                        lastSpawn = 2;
                    }
                    break;
                }
            case 2:
                {
                    if (lastSpawn != 2)
                    {
                        chosenSpawn = spawn2;
                        lastSpawn = 2;
                    }
                    else
                    {
                        chosenSpawn = spawn0;
                        lastSpawn = 0;
                    }
                    break;
                }
            default:
                break;
        }

        return chosenSpawn;
    }
    

    public void spawnIdleEnemy()
    {
        spawnZone = randomSpawn();
        Instantiate(idleEnemy, spawnZone.transform.position, spawnZone.transform.rotation);
    }

    public void spawnRoamingEnemy()
    {
        spawnZone = randomSpawn();
        Instantiate(roamingEnemy, spawnZone.transform.position, spawnZone.transform.rotation);
    }

    public void spawnRandomEnemy()
    {
        switch(rng.Next(0,2))
        {
            case 0:
                spawnRoamingEnemy();
                break;
            case 1:
                spawnIdleEnemy();
                break;
            default:
                break;

        }
    }

    public void spawnCoin()
    {
        GameObject spawnZone = randomSpawn();
        Instantiate(coin, spawnZone.transform.position, spawnZone.transform.rotation);
    }
    
     
}

using UnityEngine;
using System.Collections;

public class EnemyConditonalRespawn : MonoBehaviour {


    public triggerZoneScript trigger;
    public GameObject enemy;
    Transform enemyLocation;
	// Use this for initialization
	void Start () {
        enemyLocation = enemy.transform;
	}
	
	// Update is called once per frame
	void Update () {

        if(enemy == null)
        {
            if (!trigger.getEnemyCount())
            {
                Instantiate(enemy, enemyLocation.position, enemyLocation.rotation);
            }
        }
	
	}
}

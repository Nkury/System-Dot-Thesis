using UnityEngine;
using System.Collections;

public class triggerZoneScript : MonoBehaviour {

    public GameObject door;
    private int enemyCount;
    public int enemyThreshold;

	// Use this for initialization
	void Start () {
	    enemyCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    if(enemyCount >= enemyThreshold)
        {
            Destroy(door);
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            enemyCount++;
        }
    }

    public bool getEnemyCount()
    {
        if(enemyCount >= enemyThreshold)
        {
            return true;
        }

        return false;
    }
}

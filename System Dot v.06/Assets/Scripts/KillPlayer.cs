﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class KillPlayer : MonoBehaviour {

	public LevelManager levelManager;
    public int damageToGive;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
        if (other.name == "Player" && this.gameObject.tag != "Falling Spike")
        {
            levelManager.RespawnPlayer();
        }
        else if (other.name == "Player" && this.gameObject.tag == "Falling Spike")
        {
            HealthManager.HurtPlayer(1);
            Destroy(this.gameObject);
        } else if(other.tag == "Ground" && this.gameObject.tag == "Falling Spike")
        {
            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy" || (other.tag == "Centipede Body" && other.name != "Centipede Head") && this.gameObject.tag != "Falling Spike")
        {
            other.GetComponent<EnemyHealthManager>().giveDamage(damageToGive);
        }
	}
}

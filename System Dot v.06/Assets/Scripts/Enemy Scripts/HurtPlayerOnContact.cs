﻿using UnityEngine;
using System.Collections;

public class HurtPlayerOnContact : MonoBehaviour
{

    public int damageToGive;
    public HurtEnemyOnContact.colorState enemyState = HurtEnemyOnContact.colorState.RED;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player" && this.gameObject.tag != "Ground")
        {
            GameObject.Find("Sound Controller").GetComponent<SoundController>().play("player");
            HealthManager.HurtPlayer(damageToGive);

            var player = other.GetComponent<PlayerController>();
            player.knockbackCount = player.knockbackLength;

            if (other.transform.position.x < transform.position.x)
            {
                player.knockFromRight = true;
            }
            else
            {
                player.knockFromRight = false;
            }
        } 
    }
}

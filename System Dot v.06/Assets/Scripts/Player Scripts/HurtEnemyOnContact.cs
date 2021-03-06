﻿using UnityEngine;
using System.Collections;

public class HurtEnemyOnContact : MonoBehaviour
{

    public enum colorState { BLUE, RED, GREEN, BLACK };

    public int damageToGive;
    public float bounceOnEnemy;

    public colorState state = colorState.RED;

    public GameObject leftFoot;
    public GameObject rightFoot;
    public Sprite blueBoot;
    public Sprite redBoot;
    public Sprite greenBoot;

    private Rigidbody2D myrigidbody2D;


    // Use this for initialization
    void Start()
    {
        myrigidbody2D = transform.parent.parent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.canPressTab && Input.GetKeyDown(KeyCode.Q))
        {
            if ((int)state >= 2)
                state = colorState.BLUE;
            else {
                state++;
            }
        }

        // changes boot color sprite
        if(state == colorState.RED)
        {
            leftFoot.GetComponent<SpriteRenderer>().sprite = redBoot;
            rightFoot.GetComponent<SpriteRenderer>().sprite = redBoot;
            if(GameObject.Find("display shoe right"))
            {
                GameObject.Find("display shoe right").GetComponent<SpriteRenderer>().sprite = redBoot;
            }
        } else if(state == colorState.BLUE)
        {
            leftFoot.GetComponent<SpriteRenderer>().sprite = blueBoot;
            rightFoot.GetComponent<SpriteRenderer>().sprite = blueBoot;
            if (GameObject.Find("display shoe right"))
            {
                GameObject.Find("display shoe right").GetComponent<SpriteRenderer>().sprite = blueBoot;
            }
        } else if(state == colorState.GREEN)
        {
            leftFoot.GetComponent<SpriteRenderer>().sprite = greenBoot;
            rightFoot.GetComponent<SpriteRenderer>().sprite = greenBoot;
            if (GameObject.Find("display shoe right"))
            {
                GameObject.Find("display shoe right").GetComponent<SpriteRenderer>().sprite = greenBoot;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // kill enemies of the same color
        if ((other.tag == "Enemy" || other.tag == "Enemy1" || other.tag == "Centipede Body" || other.tag.Contains("Boss"))
            && other.GetComponent<HurtPlayerOnContact>() && other.GetComponent<HurtPlayerOnContact>().enemyState == state)
        {
            
            GameObject.Find("Sound Controller").GetComponent<SoundController>().play("enemy");
            other.GetComponent<EnemyHealthManager>().giveDamage(damageToGive);
            myrigidbody2D.velocity = new Vector2(myrigidbody2D.velocity.x, bounceOnEnemy);
            if(other.name == "TutorialEnemy")
            {
                GameObject.Find("Intellisense").GetComponent<IntelliSenseTest>().SetDialogue("killedTutorialEnemy");
            }
            // commented out because it causes lag
            // LogToFile.WriteToFile("JUMPED-ON-" + other.gameObject.name, "PLAYER");
        } else if ((other.tag == "Ground") && other.GetComponent<HurtPlayerOnContact>() != null && other.GetComponent<HurtPlayerOnContact>().enemyState == state)
        {
            GameObject.Find("Sound Controller").GetComponent<SoundController>().play("destroy block");
            Destroy(other.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.tag == "Ground") && other.GetComponent<HurtPlayerOnContact>() != null && other.GetComponent<HurtPlayerOnContact>().enemyState == state)
        {
            Physics2D.IgnoreCollision(this.transform.parent.GetComponent<Collider2D>(), other);
        }
        else if ((other.tag == "Ground") && other.GetComponent<HurtPlayerOnContact>() != null && other.GetComponent<HurtPlayerOnContact>().enemyState != state)
        {
           // Destroy(other.gameObject);
            Physics2D.IgnoreCollision(this.transform.parent.GetComponent<Collider2D>(), other, false);
        }
    }
}

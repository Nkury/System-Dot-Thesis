using UnityEngine;
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
        myrigidbody2D = transform.parent.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if ((int)state >= 2)
                state = colorState.BLUE;
            else {
                state++;
            }
        }

        if(state == colorState.RED)
        {
            leftFoot.GetComponent<SpriteRenderer>().sprite = redBoot;
            rightFoot.GetComponent<SpriteRenderer>().sprite = redBoot;
        } else if(state == colorState.BLUE)
        {
            leftFoot.GetComponent<SpriteRenderer>().sprite = blueBoot;
            rightFoot.GetComponent<SpriteRenderer>().sprite = blueBoot;
        } else if(state == colorState.GREEN)
        {
            leftFoot.GetComponent<SpriteRenderer>().sprite = greenBoot;
            rightFoot.GetComponent<SpriteRenderer>().sprite = greenBoot;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Enemy" || other.tag == "Enemy1") && other.GetComponent<HurtPlayerOnContact>().enemyState == state)
        {
            other.GetComponent<EnemyHealthManager>().giveDamage(damageToGive);
            myrigidbody2D.velocity = new Vector2(myrigidbody2D.velocity.x, bounceOnEnemy);
            if(other.name == "TutorialEnemy")
            {
                GameObject.Find("Intellisense").SendMessage("botKilled");
            }
        } else if ((other.tag == "Ground") && other.GetComponent<HurtPlayerOnContact>() != null && other.GetComponent<HurtPlayerOnContact>().enemyState == state)
        {
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

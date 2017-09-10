using UnityEngine;
using System.Collections;

public class HurtPlayerOnContact : MonoBehaviour
{

    public int damageToGive;
    public HurtEnemyOnContact.colorState enemyState = HurtEnemyOnContact.colorState.RED;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() && this.gameObject.tag != "Ground")
        {
            GameObject.Find("Sound Controller").GetComponent<SoundController>().play("player");
            HealthManager.HurtPlayer(damageToGive);

            var player = other.gameObject.GetComponent<PlayerController>();
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

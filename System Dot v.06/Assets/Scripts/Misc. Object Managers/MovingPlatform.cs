using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{

    public AudioSource moving;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            this.GetComponent<EnemyTerminal>().actions.Clear();
        }
    }

    public void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            this.GetComponent<EnemyTerminal>().actions.Clear();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            this.GetComponent<EnemyTerminal>().actions.Clear();
        }
    }

    //public void OnTriggerStay2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Ground")
    //    {
    //        this.GetComponent<EnemyTerminal>().actions.Clear();
    //    }
    //}
}

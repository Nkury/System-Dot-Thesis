using UnityEngine;
using System.Collections;

public class Bit : MonoBehaviour {

    public Sprite zero;
    public Sprite one;


	// Use this for initialization
	void Start () {
        int choice = Random.Range(0, 2);
        if (choice == 0)
            this.GetComponent<SpriteRenderer>().sprite = zero;
        else
            this.GetComponent<SpriteRenderer>().sprite = one;
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "absorb bits")
        {
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, 5* Time.deltaTime);
        }
    }

    // if we collide with a bit, we collect it. If it's an environment bit,
    // then we add it to the list of items that need to be destroyed when we
    // load the level
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            // play the collect bit sound effect
            GameObject.Find("Player").GetComponents<AudioSource>()[0].Play();

            PlayerStats.bitsCollected++;
            if (this.gameObject.name.Contains("enviroBit"))
            {
                PlayerStats.deadObjects.Add(this.gameObject.name);
            }
            Destroy(this.gameObject);
        }
    }

}

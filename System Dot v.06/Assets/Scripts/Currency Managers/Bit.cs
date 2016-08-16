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

}

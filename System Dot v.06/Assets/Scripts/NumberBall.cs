using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBall : MonoBehaviour {

    public char character;
    public TextMesh text;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        text.text = character.ToString();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<KillPlayer>() && other.gameObject.tag != "Boss2Attack")
        {
            Destroy(this.gameObject);
        }
    }
}

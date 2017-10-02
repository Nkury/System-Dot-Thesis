using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBall : MonoBehaviour {

    public string character;
    public TextMesh text;
    public bool canClick = true;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        text.text = character.ToString();
        if (!canClick)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 128f/255, 230f/255, 1);
        }

    }

    public void OnMouseOver()
    {
        if (canClick)
            gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                                    gameObject.GetComponent<SpriteRenderer>().material.color.b, .5f);
    }

    public void OnMouseExit()
    {
        if (canClick)
            gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                                    gameObject.GetComponent<SpriteRenderer>().material.color.b, 1);
    }

    public void OnMouseDown()
    {
        if (canClick)
        {
            int newVal;
            if (int.TryParse(character, out newVal))
            {
                character = (newVal + 1).ToString();
            }
            else
            {
                char incChar = character[0];
                character = incChar++ + character.Substring(1, character.Length - 1);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<KillPlayer>() && other.gameObject.tag != "Boss2Attack")
        {
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortIntelliSense : MonoBehaviour {

    public GameObject playButton;
    public GameObject pauseButton;
    public GameObject deathEffect;

    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIsWall;

    public float moveSpeed;
    public float accelerationRate = .001f;
    public float accelerationLimit = .03f;
    public float acceleration;

    public bool isCharging = false;

    private GameObject currentCheckpoint;
    private bool hittingWall;
    private Color formerColor;

	// Use this for initialization
	void Start () {
        formerColor = this.GetComponent<SpriteRenderer>().color;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (pauseButton.activeSelf)
        {
            hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);

            if (!hittingWall)
            {
                if (acceleration < accelerationLimit)
                {
                    acceleration += accelerationRate;
                }

                this.gameObject.transform.position += Vector3.right * acceleration;
            }
        }

        if (isCharging)
        {
            this.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 1));
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = formerColor;
        }
    }

    public void OnMouseDown()
    {
        if (playButton.activeSelf)
        {
            playButton.SetActive(false);
            pauseButton.SetActive(true);
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            acceleration = moveSpeed;
        }
        else
        {
            playButton.SetActive(true);
            pauseButton.SetActive(false);
        }
    }

    public void OnMouseOver()
    {
        if (playButton.activeSelf)
        {
            playButton.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                                        gameObject.GetComponent<SpriteRenderer>().material.color.b, .5f);
        }
        else
        {
            pauseButton.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                                  gameObject.GetComponent<SpriteRenderer>().material.color.b, .5f);
        }
    }

    public void OnMouseExit()
    {
        if (playButton.activeSelf)
        {
            playButton.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                                        gameObject.GetComponent<SpriteRenderer>().material.color.b, 1);
        }
        else
        {
            pauseButton.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                                  gameObject.GetComponent<SpriteRenderer>().material.color.b, .5f);
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<HurtPlayerOnContact>() || other.gameObject.GetComponent<KillPlayer>()) {
            StartCoroutine(KillIntelliSense());
        }

        if (other.gameObject.tag == "movingPlatform")
        {
            this.transform.parent = new GameObject().transform;
            this.transform.parent.parent = other.transform;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "movingPlatform")
        {
            transform.parent = null;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "IntelliSenseCheckpoint")
        {
            currentCheckpoint = other.gameObject;
        } else if(other.gameObject.GetComponent<HurtPlayerOnContact>() || other.gameObject.GetComponent<KillPlayer>())
        {
            StartCoroutine(KillIntelliSense());
        }
    }

    public IEnumerator KillIntelliSense()
    {
        playButton.SetActive(true);
        playButton.GetComponent<SpriteRenderer>().enabled = false;
        pauseButton.SetActive(false);
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        this.GetComponent<Collider2D>().enabled = false;
        this.GetComponent<Blinking>().enabled = true;
        Instantiate(deathEffect, transform.position, transform.rotation);
        this.transform.position = currentCheckpoint.transform.position;
        Instantiate(deathEffect, transform.position, transform.rotation);
        yield return new WaitForSeconds(3);
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        playButton.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<SpriteRenderer>().enabled = true;
        this.GetComponent<Collider2D>().enabled = true;
        this.GetComponent<Blinking>().enabled = false;
    }
}

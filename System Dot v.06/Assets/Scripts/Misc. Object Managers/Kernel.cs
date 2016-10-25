using UnityEngine;
using System.Collections;

public class Kernel : MonoBehaviour {

    public AudioSource beep;

    public GameObject player;
    private bool canPressE = false;
    private bool inDebugMode = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (EnemyTerminal.globalTerminalMode < 2)
        {
            if (Input.GetKeyDown(KeyCode.E) && canPressE && !inDebugMode)
            {
                inDebugMode = true;
                player.SetActive(false);
                Camera.main.orthographicSize = 7.07f;
                if(PlayerStats.highestCheckpoint == 3 && this.gameObject.name == "TutorialKernel")
                {
                    GameObject.Find("Intellisense").SendMessage("InDebugStation");
                    this.gameObject.name = "TutorialKernel1";
                }
            }
            else if (Input.GetKeyDown(KeyCode.E) && inDebugMode)
            {
                inDebugMode = false;
                player.SetActive(true);
                Camera.main.orthographicSize = 3.675071f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            beep.Play();
            canPressE = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                                      gameObject.GetComponent<SpriteRenderer>().material.color.b, .5f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            canPressE = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(gameObject.GetComponent<SpriteRenderer>().material.color.r, gameObject.GetComponent<SpriteRenderer>().material.color.g,
                                                              gameObject.GetComponent<SpriteRenderer>().material.color.b, 1);

        }
    }
}

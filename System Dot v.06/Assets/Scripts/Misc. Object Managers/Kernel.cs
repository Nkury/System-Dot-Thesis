using UnityEngine;
using System.Collections;

public class Kernel : MonoBehaviour {

    public AudioSource beep;

    public GameObject player;
    public GameObject intelliSense;

    public static string kernelNameCurrentlyIn;

    public float zoomOutSize = 7.07f; // default is 7.07
    private bool canPressE = false;
    private bool inDebugMode = false;


	
	// Update is called once per frame
	void Update () {
        if (EnemyTerminal.globalTerminalMode < 2)
        {
            if (Input.GetKeyDown(KeyCode.E) && canPressE && !inDebugMode)
            {
                inDebugMode = true;
                kernelNameCurrentlyIn = this.gameObject.name;
                player.SetActive(false);
                Camera.main.orthographicSize = zoomOutSize;
                intelliSense.GetComponent<Dialogue>().initialEvent(this.gameObject.name);           
            }
            else if (Input.GetKeyDown(KeyCode.E) && inDebugMode)
            {
                inDebugMode = false;
                kernelNameCurrentlyIn = string.Empty;
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

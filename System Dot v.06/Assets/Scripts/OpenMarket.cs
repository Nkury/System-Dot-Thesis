using UnityEngine;
using System.Collections;

public class OpenMarket : MonoBehaviour {


    public GameObject marketCanvas;

    private bool openMarket;

    private bool inMarket;

	// Use this for initialization
	void Start () {
        openMarket = false;
        inMarket = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W) && inMarket)
        {
                openMarket = true;
        }

        if(openMarket)
        {
            marketCanvas.SetActive(true);
            Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && openMarket)
        {
            openMarket = false;
            marketCanvas.SetActive(false);
            Time.timeScale = 1f;
        }
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            inMarket = true;
        }
    }

    void OnTriggerExit2d(Collider2D other)
    {
        
            inMarket = false;
        
    }


}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpenMarket : MonoBehaviour {
    
    public GameObject marketCanvas;

    private bool openMarket;

    private bool inMarket;

    public GameObject openShopTip;

	// Use this for initialization
	void Start () {
        PlayerStats.bitsCollected = 2000;
        openMarket = false;
        inMarket = false;
	}
	
	// Update is called once per frame
	void Update () {
      
        if (Input.GetKeyDown(KeyCode.W) && inMarket)
        {
            openShopTip.SetActive(openMarket);
            openMarket = !openMarket;            
        }

        if (Input.GetKeyDown(KeyCode.Escape) && openMarket)
        {
            openShopTip.SetActive(openMarket);
            openMarket = !openMarket;
           
        }
        if(openMarket)
        {
            marketCanvas.SetActive(true);
            Time.timeScale = 0f;
            
        }
        else
        {
            marketCanvas.SetActive(false);
            Time.timeScale = 1f;
        }

	}

    void OnTriggerEnter2D(Collider2D other)
    {       
        if (other.tag == "Player")
        {
            inMarket = true;
            openShopTip.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {        
            inMarket = false;
            openShopTip.SetActive(false);        
    }


}

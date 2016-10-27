using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    private PauseMenu pause;
    private HealthManager healthManager;

    //public GameObject gameOverScreen;
    //public PlayerController player;


	// Use this for initialization
	void Start () {
        pause = FindObjectOfType<PauseMenu>();
        //player = FindObjectOfType<PlayerController>();
        ////healthManager = FindObjectOfType<HealthManager>();
        //countingTime = startingTime;
	}
	
	// Update is called once per frame
	void Update () {
        if(pause != null && pause.isPaused)
        {
            return;
        }

        PlayerStats.totalSecondsOfPlaytime += Time.deltaTime;
        //text.text = "" + Mathf.Round(countingTime);

        //if (countingTime <= .5)
        //{
        //   //gameOverScreen.SetActive(true);
        //   //player.gameObject.SetActive(false);
        //    healthManager.KillPlayer();

        //}
       
	}

    //public void resetTime()
    //{
    //    countingTime = startingTime;
    //}
}

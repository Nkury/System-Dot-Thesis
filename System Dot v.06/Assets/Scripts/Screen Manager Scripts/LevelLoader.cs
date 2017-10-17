using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour {


	private bool playerInZone;

	public string levelToLoad;

	// Use this for initialization
	void Start () {
		playerInZone = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxisRaw ("Vertical") > 0 && playerInZone) 
		{
            if(SceneManager.GetActiveScene().name.Contains("LVL"))
            {
                StatsLog.WriteToFile();
            }

			Application.LoadLevel(levelToLoad);

		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			playerInZone = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			playerInZone = false;
		}
	}
}

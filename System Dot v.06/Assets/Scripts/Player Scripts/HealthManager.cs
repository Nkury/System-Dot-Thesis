using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

	public int maxPlayerHealth;

	public static int playerHealth;
	
	Text text;

	private LevelManager levelManager;

	public bool isDead;

  private LifeManager lifeSystem;

  private TimeManager timeManager;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();	
		//playerHealth = maxPlayerHealth;
		playerHealth = PlayerPrefs.GetInt ("PlayerCurrentHealth");
		levelManager = FindObjectOfType<LevelManager> ();
    timeManager = FindObjectOfType<TimeManager>();
		lifeSystem = FindObjectOfType<LifeManager> ();
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(playerHealth <= 0 && !isDead && SceneManager.GetActiveScene().name == "StartingScene")
        {
            playerHealth = 0;
            levelManager.RespawnPlayer();
            lifeSystem.TakeLife();
            isDead = true;
            //timeManager.resetTime();
        } else if(playerHealth <= 0 && !isDead && SceneManager.GetActiveScene().name == "Level1 BOSS")
        {
            FullHealth();
            SceneManager.LoadScene("Level1 BOSS");
        }

		text.text = "" + playerHealth;
	}

	public static void HurtPlayer(int damageToGive)
	{
		playerHealth -= damageToGive;
      if (playerHealth < 0)
        {
            playerHealth = 0;
        }
		PlayerPrefs.SetInt ("PlayerCurrentHealth", playerHealth);
	}

	public void FullHealth()
	{
		playerHealth = PlayerPrefs.GetInt ("PlayerMaxHealth");
		PlayerPrefs.SetInt ("PlayerCurrentHealth", playerHealth);
	}

  public void KillPlayer()
    {
        playerHealth = 0;
    }
}

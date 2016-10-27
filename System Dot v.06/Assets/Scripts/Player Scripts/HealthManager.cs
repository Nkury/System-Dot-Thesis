using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

	
	Text text;

	private LevelManager levelManager;

	public bool isDead;

   private LifeManager lifeSystem;

   private TimeManager timeManager;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();	
        this.GetComponent<Slider>().maxValue = PlayerStats.maxHealth;
        levelManager = FindObjectOfType<LevelManager> ();
        timeManager = FindObjectOfType<TimeManager>();
		lifeSystem = FindObjectOfType<LifeManager> ();
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(PlayerStats.currentHealth <= 0 && !isDead && SceneManager.GetActiveScene().name == "StartingScene")
        {
            PlayerStats.currentHealth = 0;
            levelManager.RespawnPlayer();
            PlayerStats.numberOfDeaths++;
            isDead = true;
            EnemyTerminal.globalTerminalMode = 0;
            //timeManager.resetTime();
        } else if(PlayerStats.currentHealth <= 0 && !isDead && SceneManager.GetActiveScene().name == "Level1 BOSS")
        {
            FullHealth();
            PlayerStats.numberOfDeaths++;
            CentipedeHead.lives = 18;
            CentipedeHead.life = 1;
            EnemyTerminal.globalTerminalMode = 0;
            Application.LoadLevel(Application.loadedLevel);
        }

        this.GetComponent<Slider>().value = PlayerStats.currentHealth;
	}

	public static void HurtPlayer(int damageToGive)
	{
		PlayerStats.currentHealth -= damageToGive;
      if (PlayerStats.currentHealth < 0)
        {
            PlayerStats.currentHealth = 0;
        }
	}

	public void FullHealth()
	{
        PlayerStats.currentHealth = PlayerStats.maxHealth;
	}

  public void KillPlayer()
    {
        PlayerStats.currentHealth = 0;
    }
}

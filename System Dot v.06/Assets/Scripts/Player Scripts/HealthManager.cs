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
    void Start() {
        text = GetComponent<Text>();
        if (this.gameObject.name == "Health Bar")
        {
            this.GetComponent<Slider>().maxValue = PlayerStats.maxHealth;
        } else if (this.gameObject.name == "Armor Bar")
        {
            this.GetComponent<Slider>().maxValue = PlayerStats.armorHealth;
        }
        levelManager = FindObjectOfType<LevelManager> ();
        timeManager = FindObjectOfType<TimeManager>();
		lifeSystem = FindObjectOfType<LifeManager> ();
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(PlayerStats.currentHealth <= 0 && !isDead && SceneManager.GetActiveScene().name == "StartingScene")
        {
            GameObject.Find("Sound Controller").GetComponent<SoundController>().play("death");
            PlayerStats.currentHealth = 0;
            levelManager.RespawnPlayer();
            PlayerStats.numberOfDeaths++;
            isDead = true;
            EnemyTerminal.globalTerminalMode = 0;
            //timeManager.resetTime();
        } else if(PlayerStats.currentHealth <= 0 && !isDead && SceneManager.GetActiveScene().name == "Level1 BOSS")
        {
            GameObject.Find("Sound Controller").GetComponent<SoundController>().play("death");
            FullHealth();
            BossIntellisense.startBoss = false;
            PlayerStats.numberOfDeaths++;
            CentipedeHead.lives = 18;
            CentipedeHead.life = 1;
            EnemyTerminal.globalTerminalMode = 0;
            Application.LoadLevel(Application.loadedLevel);
        }

        if (this.gameObject.name == "Health Bar")
        {
            this.GetComponent<Slider>().value = PlayerStats.currentHealth;
        } else if(this.gameObject.name == "Armor Bar")
        {
            this.GetComponent<Slider>().value = PlayerStats.armorHealth;
        }

        
	}

	public static void HurtPlayer(int damageToGive)
	{
        if (!Invincibility.invincibility)
        {
            if (PlayerStats.armorHealth <= 0)
            {
                PlayerStats.currentHealth -= damageToGive;
                Invincibility.invincibility = true;
                Invincibility.invincibilityOnce = true;
            }
            else
            {
                PlayerStats.armorHealth -= damageToGive;
                Invincibility.invincibility = true;
                Invincibility.invincibilityOnce = true;
            }

            if (PlayerStats.currentHealth < 0)
            {
                PlayerStats.currentHealth = 0;
            }

            if (PlayerStats.armorHealth < 0)
            {
                PlayerStats.armorHealth = 0;
            }
        }
	}

	public static void FullHealth()
	{
        GameObject.Find("Sound Controller").GetComponent<SoundController>().play("health");
        PlayerStats.currentHealth = PlayerStats.maxHealth;
	}

  public void KillPlayer()
    {
        PlayerStats.currentHealth = 0;
    }

    public void SetMaxArmor()
    {
        if(this.gameObject.name == "Armor Bar")
        {
            this.GetComponent<Slider>().maxValue = 10;
        }
    }
}

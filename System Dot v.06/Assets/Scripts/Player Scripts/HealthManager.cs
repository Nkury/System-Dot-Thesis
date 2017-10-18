using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

	
	Text text;

	private LevelHandler levelManager;

	public static bool isDead;

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
        levelManager = FindObjectOfType<LevelHandler> ();
        timeManager = FindObjectOfType<TimeManager>();
		lifeSystem = FindObjectOfType<LifeManager> ();
		isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(PlayerStats.currentHealth <= 0 && !isDead)
        {
            if (PlayerStats.numRevivePotions > 0)
            {
                PlayerStats.numRevivePotions--;
                HealthManager.FullHealth();
            }
            else
            {
                GameObject.Find("Sound Controller").GetComponent<SoundController>().play("death");
                LogHelper.SetDictionaryValue(PlayerStats.numberOfDeaths, LogHelper.GetDictionaryValue(PlayerStats.numberOfDeaths) + 1);
                PlayerStats.currentHealth = 0;

                if (SceneManager.GetActiveScene().name == "Level1 BOSS")
                {
                    FullHealth();
                    BossIntellisense.startBoss = false;
                    CentipedeHead.lives = 18;
                    CentipedeHead.life = 1;
                    SceneManager.LoadScene(PlayerStats.levelName);
                }
                else if(levelManager)
                {
                    levelManager.RespawnPlayer();
                    isDead = true;
                }
            }

            EnemyTerminal.globalTerminalMode = 0;            
        }	   

        if (this.gameObject.name == "Health Bar")
        {
            this.GetComponent<Slider>().maxValue = PlayerStats.maxHealth;
            this.GetComponent<Slider>().value = PlayerStats.currentHealth;
        } else if(this.gameObject.name == "Armor Bar")
        {
            this.GetComponent<Slider>().value = PlayerStats.armorHealth;
            SetMaxArmor();
        }

        
	}

	public static void HurtPlayer(int damageToGive)
	{

        if (!Invincibility.invincibility)
        {
            LogToFile.WriteToFile("PLAYER-TAKES-" + damageToGive + "-DAMAGE", "PLAYER-" + PlayerStats.playerName);
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
        //GameObject.Find("Sound Controller").GetComponent<SoundController>().play("health");
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

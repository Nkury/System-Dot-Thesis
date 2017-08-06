using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public string startLevel;

	public string levelSelect;

	public int playerLives;
	public int playerHealth;

    public Button newGame;
    public Button continueGame;

    public void Start()
    {
        // uncomment below to erase all saved files when running title screen
        // SaveLoad.EraseAll();
        SaveLoad.Load(); // load the game

    }

    public void Update()
    {
        if (SaveLoad.savedGames.Count != 0)
        {
            continueGame.gameObject.SetActive(true);
            newGame.gameObject.SetActive(false);
        }
        else
        {
            continueGame.gameObject.SetActive(false);
            newGame.GetComponent<RectTransform>().anchoredPosition = new Vector2(3, -29);
        }
    }

    public void ResetButton()
    {
        newGame.gameObject.SetActive(true);
        SaveLoad.EraseAll();
    }

	public void NewGame()
	{
        Game.current = new Game();
        PlayerStats.maxHealth = playerHealth;
        PlayerStats.currentHealth = playerHealth;
        PlayerStats.numberOfDeaths = 0;
        PlayerStats.totalSecondsOfPlaytime = 0;

        SceneManager.LoadScene(startLevel);

		PlayerPrefs.SetInt ("PlayerCurrentLives", playerLives);

		PlayerPrefs.SetInt ("CurrentScore", 0);  

		PlayerPrefs.SetInt ("PlayerCurrentHealth", playerHealth);
		PlayerPrefs.SetInt ("PlayerMaxHealth", playerHealth);
	}

    public void ContinueGame()
    {
        Game.current = SaveLoad.savedGames[0];
        PlayerStats.maxHealth = SaveLoad.savedGames[0].maxHealth;
        PlayerStats.currentHealth = SaveLoad.savedGames[0].currentHealth;
        PlayerStats.armorHealth = SaveLoad.savedGames[0].armorHealth;
        PlayerStats.bitsCollected = SaveLoad.savedGames[0].bitsCollected;
        PlayerStats.numberOfDeaths = SaveLoad.savedGames[0].numberOfDeaths;
        PlayerStats.totalSecondsOfPlaytime = SaveLoad.savedGames[0].totalSecondsOfPlaytime;
        PlayerStats.playerName = SaveLoad.savedGames[0].playerName;
        PlayerStats.checkpoint = SaveLoad.savedGames[0].checkpoint;
        PlayerStats.deadObjects = SaveLoad.savedGames[0].deadObjects;
        PlayerStats.terminalStrings = SaveLoad.savedGames[0].terminalStrings;
        PlayerStats.highestCheckpoint = SaveLoad.savedGames[0].highestCheckpoint;
        PlayerStats.numHealthPotions = SaveLoad.savedGames[0].numHealthPotions;
        PlayerStats.typingSpeed = SaveLoad.savedGames[0].typingSpeed;
        PlayerStats.averageTimeOnEditing = SaveLoad.savedGames[0].averageTimeOnEditing;
        PlayerStats.longestTimeOnEditing = SaveLoad.savedGames[0].longestTimeOnEditing;
        PlayerStats.averageNumberofMouseClicks = SaveLoad.savedGames[0].averageNumberofMouseClicks;
        PlayerStats.mostNumberofMouseClicks = SaveLoad.savedGames[0].mostNumberofMouseClicks;
        PlayerStats.mostNumberofAttempts = SaveLoad.savedGames[0].mostNumberofAttempts;
        PlayerStats.numberOfPerfectEdits = SaveLoad.savedGames[0].numberOfPerfectEdits;
        PlayerStats.mostNumberOfBackspaces = SaveLoad.savedGames[0].mostNumberOfBackspaces;
        PlayerStats.averageNumberOfBackspaces = SaveLoad.savedGames[0].averageNumberOfBackspaces;
        PlayerStats.mostNumberOfDeletes = SaveLoad.savedGames[0].mostNumberOfDeletes;
        PlayerStats.averageNumberOfDeletes = SaveLoad.savedGames[0].averageNumberOfDeletes;
        PlayerStats.averageTimeOfMouseInactivity = SaveLoad.savedGames[0].averageTimeOfMouseInactivity;
        PlayerStats.mostTimeofMouseInactivity = SaveLoad.savedGames[0].mostTimeofMouseInactivity;
        PlayerStats.numOfAPIUses = SaveLoad.savedGames[0].numOfAPIUses;
        PlayerStats.numOfF5 = SaveLoad.savedGames[0].numOfF5;
        PlayerStats.numOfEdits = SaveLoad.savedGames[0].numOfEdits;
        PlayerStats.levelName = SaveLoad.savedGames[0].levelName;
      
        SceneManager.LoadScene(PlayerStats.levelName);
   
    }

	public void LevelSelect()
	{
		PlayerPrefs.SetInt ("PlayerCurrentLives", playerLives);

		PlayerPrefs.SetInt ("CurrentScore", 0);

		PlayerPrefs.SetInt ("PlayerCurrentHealth", playerHealth);
		PlayerPrefs.SetInt ("PlayerMaxHealth", playerHealth);

		SceneManager.LoadScene (levelSelect);
	}

	public void QuitGame()
	{
		Application.Quit ();
	}
}

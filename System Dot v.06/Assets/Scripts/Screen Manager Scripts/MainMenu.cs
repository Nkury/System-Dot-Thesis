using UnityEngine;
using UnityEngine.UI;
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
            newGame.GetComponent<RectTransform>().anchoredPosition = new Vector2(3, 7);
        }
        else
        {
            continueGame.gameObject.SetActive(false);
            newGame.GetComponent<RectTransform>().anchoredPosition = new Vector2(3, -29);
        }
    }

    public void ResetButton()
    {
        SaveLoad.EraseAll();
    }

	public void NewGame()
	{
        Game.current = new Game();

		Application.LoadLevel (startLevel);

		PlayerPrefs.SetInt ("PlayerCurrentLives", playerLives);

		PlayerPrefs.SetInt ("CurrentScore", 0);  

		PlayerPrefs.SetInt ("PlayerCurrentHealth", playerHealth);
		PlayerPrefs.SetInt ("PlayerMaxHealth", playerHealth);
	}

    public void ContinueGame()
    {
        Game.current = SaveLoad.savedGames[0];
        PlayerStats.bitsCollected = SaveLoad.savedGames[0].bitsCollected;
        PlayerStats.playerName = SaveLoad.savedGames[0].playerName;
        PlayerStats.checkpoint = SaveLoad.savedGames[0].checkpoint;
        PlayerStats.deadObjects = SaveLoad.savedGames[0].deadObjects;
        PlayerStats.highestCheckpoint = SaveLoad.savedGames[0].highestCheckpoint;

        if (PlayerStats.checkpoint == "Checkpoint6")
        {
            Application.LoadLevel("Level1 BOSS");
        }
        else
        {
            Application.LoadLevel(startLevel);
        }
    }

	public void LevelSelect()
	{
		PlayerPrefs.SetInt ("PlayerCurrentLives", playerLives);

		PlayerPrefs.SetInt ("CurrentScore", 0);

		PlayerPrefs.SetInt ("PlayerCurrentHealth", playerHealth);
		PlayerPrefs.SetInt ("PlayerMaxHealth", playerHealth);

		Application.LoadLevel (levelSelect);
	}

	public void QuitGame()
	{
		Application.Quit ();
	}
}

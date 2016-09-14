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
        if(SaveLoad.savedGames.Count != 0)
        {
            continueGame.gameObject.SetActive(true);
            newGame.GetComponent<RectTransform>().anchoredPosition = new Vector2(3, 7);
        }
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
        PlayerStats.bitsCollected = SaveLoad.savedGames[0].bitsCollected;
        PlayerStats.playerName = SaveLoad.savedGames[0].playerName;
        PlayerStats.firstCheckpoint = SaveLoad.savedGames[0].firstCheckpoint;
        PlayerStats.secondCheckpoint = SaveLoad.savedGames[0].secondCheckpoint;
        PlayerStats.thirdCheckpoint = SaveLoad.savedGames[0].thirdCheckpoint;
        PlayerStats.fourthCheckpoint = SaveLoad.savedGames[0].fourthCheckpoint;
        PlayerStats.fifthCheckpoint = SaveLoad.savedGames[0].fifthCheckpoint;
        PlayerStats.deadObjects = SaveLoad.savedGames[0].deadObjects;

        Application.LoadLevel(startLevel);
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

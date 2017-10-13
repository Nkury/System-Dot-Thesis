using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
        LogToFile.WriteToFile("\nSTARTING-NEW-GAME", "GAME\n");
        Game.current = new Game();
        PlayerStats.maxHealth = playerHealth;
        PlayerStats.currentHealth = playerHealth;
        PlayerStats.numberOfDeaths = 0;
        PlayerStats.totalSecondsOfPlaytime = 0;

        CreateNetwork();

        SceneManager.LoadScene(startLevel);

		PlayerPrefs.SetInt ("PlayerCurrentLives", playerLives);

		PlayerPrefs.SetInt ("CurrentScore", 0);  

		PlayerPrefs.SetInt ("PlayerCurrentHealth", playerHealth);
		PlayerPrefs.SetInt ("PlayerMaxHealth", playerHealth);
	}

    public void ContinueGame()
    {
        Game.current = SaveLoad.savedGames[0];

        // ADAPTIVE LOG SECTION
        PlayerStats.log_numAPIOpen = SaveLoad.savedGames[0].log_numAPIOpen;
        PlayerStats.log_numLegacyCodeViewed = SaveLoad.savedGames[0].log_numLegacyCodeViewed;
        PlayerStats.log_numOfF5 = SaveLoad.savedGames[0].log_numOfF5;
        PlayerStats.log_numPerfectEdits = SaveLoad.savedGames[0].log_numPerfectEdits;
        PlayerStats.log_numSyntaxErrors = SaveLoad.savedGames[0].log_numSyntaxErrors;
        PlayerStats.log_numQuickDebug = SaveLoad.savedGames[0].log_numQuickDebug;
        PlayerStats.log_totalNumDebugs = SaveLoad.savedGames[0].log_totalNumDebugs;
        PlayerStats.log_totalNumberOfLegacyOnly = SaveLoad.savedGames[0].log_totalNumberOfLegacyOnly;
        PlayerStats.log_totalNumberOfModifiedEdits = SaveLoad.savedGames[0].log_totalNumberOfModifiedEdits;
        
        // GAME STATS
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
        PlayerStats.numRevivePotions = SaveLoad.savedGames[0].numRevivePotions;
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
        PlayerStats.numOfEdits = SaveLoad.savedGames[0].numOfEdits;
        PlayerStats.levelName = SaveLoad.savedGames[0].levelName;

        LogToFile.WriteToFile("\nCONTINUE-GAME-" + PlayerStats.levelName, "GAME\n");
        LogToFile.WriteToFile("CONTINUED-GAME-" + PlayerStats.levelName, "GAME\n", PlayerStats.levelName);
        SceneManager.LoadScene(PlayerStats.levelName);
   
    }

    public void CreateNetwork()
    {
        // Processing Network
        BayesianNetwork.BNode TimeToClickDebugNode = new BayesianNetwork.BNode(null, "TimeToClickDebug", 0, new List<double>() { .5 });
        BayesianNetwork.BNode ViewLegacyCodeNode = new BayesianNetwork.BNode(null, "ViewLegacyCode", 1, new List<double>() { .5 });
        BayesianNetwork.BNode ProcessingNode = new BayesianNetwork.BNode(new List<BayesianNetwork.BNode>() { TimeToClickDebugNode, ViewLegacyCodeNode }, "Processing", 2, new List<double>() { 0, .5, .5, 1});
        BayesianNetwork.BNetwork ProcessingNetwork = new BayesianNetwork.BNetwork("ProcessingNetwork", new List<BayesianNetwork.BNode>() { TimeToClickDebugNode, ViewLegacyCodeNode, ProcessingNode });

        // Perception Network
        BayesianNetwork.BNode UseAPINode = new BayesianNetwork.BNode(null, "UseAPI", 1, new List<double>() { .333 });
        BayesianNetwork.BNode NumberOfSyntaxErrors = new BayesianNetwork.BNode(null, "NumberOfSyntaxErrors", 2, new List<double>() { .333 });
        BayesianNetwork.BNode NumberTimesF5KeyHit = new BayesianNetwork.BNode(null, "NumberOfF5KeyPressed", 3, new List<double>() { .333 });
        BayesianNetwork.BNode PerceptionNode = new BayesianNetwork.BNode(new List<BayesianNetwork.BNode>() { UseAPINode, NumberOfSyntaxErrors, NumberTimesF5KeyHit},
            "Perception", 4, new List<double>() { .25, 0, .33, .17, .75, .67, 1, .83 });
        BayesianNetwork.BNetwork PerceptionNetwork = new BayesianNetwork.BNetwork("PerceptionNetwork",
            new List<BayesianNetwork.BNode>() { UseAPINode, NumberOfSyntaxErrors, NumberTimesF5KeyHit, PerceptionNode});

        // Understanding Network
        BayesianNetwork.BNode NumberOfPerfectEdits = new BayesianNetwork.BNode(null, "NumberOfPerfectEdits", 0, new List<double>() { .5 });
        BayesianNetwork.BNode UnderstandingNode = new BayesianNetwork.BNode(new List<BayesianNetwork.BNode>() { NumberOfPerfectEdits, NumberOfSyntaxErrors}, "Understanding", 1,
            new List<double>() { .5, 1, 0, .5 });
        BayesianNetwork.BNetwork UnderstandingNetwork = new BayesianNetwork.BNetwork("UnderstandingNetwork", new List<BayesianNetwork.BNode>() { NumberOfPerfectEdits, NumberOfSyntaxErrors, UnderstandingNode});

        Debug.Log("Processing: " + ProcessingNode.CalculateProbability(1));
        Debug.Log("Perception: " + PerceptionNode.CalculateProbability(1));
        Debug.Log("Understanding: " + UnderstandingNode.CalculateProbability(1));

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
        LogToFile.WriteToFile("\nQUIT-GAME", "GAME\n");
        Application.Quit ();
	}
}

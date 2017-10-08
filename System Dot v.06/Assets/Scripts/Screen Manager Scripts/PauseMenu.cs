using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{

    public string mainMenu;

    public string levelSelect;

    public bool isPaused;

    public GameObject pauseMenuCanvas;

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        else {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LogToFile.WriteToFile("CLOSE-TERMINAL-WINDOW", "TERMINAL-WINDOW");
            EnemyTerminal.globalTerminalMode--;
            if (EnemyTerminal.globalTerminalMode <= 0)
            {
                if (isPaused)
                {
                    LogToFile.WriteToFile("UNPAUSE-GAME", "GAME");
                }
                else
                {
                    LogToFile.WriteToFile("PAUSE-GAME", "GAME");
                }

                isPaused = !isPaused;
            }
        }
    }

    public void Resume()
    {
        isPaused = false;
    }

    public void LevelSelect()
    {
        isPaused = false;
        Application.LoadLevel(levelSelect);
    }

    public void Quit()
    {
        isPaused = false;
        Application.LoadLevel(mainMenu);
    }
}

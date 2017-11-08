using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public string mainMenu;

    public bool isPaused;

    public GameObject pauseMenuCanvas;
    public Dropdown levelSelect;
    private TerminalWindowUI terminalWindow;

    public void Start()
    {
        terminalWindow = GameObject.FindObjectOfType<TerminalWindowUI>();
    }

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
            terminalWindow.exitClicked();
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
        if (levelSelect.value != 4)
        {
            Application.LoadLevel("LVL" + (levelSelect.value + 1));
        }
        else
        {
            Application.LoadLevel("CPU");
        }
    }

    public void Quit()
    {
        isPaused = false;
        Application.LoadLevel("TitleMenu");
    }
}

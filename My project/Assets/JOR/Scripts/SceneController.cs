using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    public static SceneController Instance => instance;

    private Scene activeScene;

    [Header("Pause Menu")]
    public GameObject pauseMenuScreen;
    public GameObject settingsScreen;
    public GameObject pauseMenuPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    void Update()
    {
        activeScene = SceneManager.GetActiveScene();
    }

    public void NextScene()
    {
        SceneManager.LoadScene(activeScene.buildIndex + 1);
    }

    public void Pause()
    {
        pauseMenuScreen.SetActive(true);
    }
    public void Resume()
    {
        pauseMenuScreen.SetActive(false);
    }

    public void Settings()
    {
        pauseMenuPanel.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseSettings()
    {
        settingsScreen.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }

}

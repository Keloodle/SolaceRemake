using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverMenu;


    public void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += EnableGameOverMenu;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= EnableGameOverMenu;
    }

    public void EnableGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level01");
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMeny");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //}
    }

    public void openSettings()
    {
        print("Whoops! Forgot to add");
    }

    public void OnButtonPress()
    {

    }

    public void PlayGame()
    {
        Invoke("delayGame", 0.5f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void returnMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        Destroy(this.gameObject);
    }

    public void delayGame()
    {
        SceneManager.LoadScene(1);
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.transform.name == "player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    */
}

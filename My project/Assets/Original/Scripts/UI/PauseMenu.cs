using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public bool paused = false;
    public bool inventoryUp = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject inventoryMenu;

    public void pause(InputAction.CallbackContext context)
    {
        if (context.canceled && !paused)
        {
            Time.timeScale = 0;
            paused = true;
            inventoryUp = false;
        } else if(context.canceled && paused || context.started && inventoryUp)
        {
            Time.timeScale = 1;
            paused = false;
        }
    }
    

    public void openInv(InputAction.CallbackContext context)
    {
        if (context.canceled && !inventoryUp)
        {
            Time.timeScale = 0;
            inventoryUp = true;
            paused = false;
        }
        else if (context.canceled && inventoryUp)
        {
            Time.timeScale = 1;
            inventoryUp = false;
        }
    }

    public void continueButton()
    {
        Time.timeScale = 1;
        paused = false;
        inventoryUp = false;
    }

    private void Update()
    {
        pauseMenu.SetActive(paused);
        inventoryMenu.SetActive(inventoryUp);
    }

    public void invFromMenu()
    {
        if (!inventoryUp)
        {
            Time.timeScale = 0;
            inventoryUp = true;
            paused = false;
        }
        else if (inventoryUp)
        {
            Time.timeScale = 1;
            inventoryUp = false;
        }
    }

    /*
    void Update()
    {
        if (!paused && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            paused = true;
        }
        else if (paused && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            paused = false;
        }
    }
    */
}

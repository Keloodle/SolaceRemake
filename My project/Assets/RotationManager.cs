using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RotationManager : MonoBehaviour
{
    private RotationManager instance;
    public RotationManager Instance => instance;


    private Scene activeScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }


    void Update()
    {
        activeScene = SceneManager.GetActiveScene();


        if (activeScene.buildIndex == 0)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else if (activeScene.buildIndex == 1)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }


}

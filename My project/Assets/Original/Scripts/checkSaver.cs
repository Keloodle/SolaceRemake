using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class checkSaver : MonoBehaviour
{
    private static checkSaver instance;
    public Vector2 lastCheckPoint = new Vector2(0,0);
    public int lastCheckScene = 1;

    public bool[] loreFound;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } else
        {
            Destroy(this);
        }
    }
}

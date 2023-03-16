using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeMainMenu : MonoBehaviour
{
    public void fade()
    {
        GetComponent<Animator>().SetTrigger("fade");
    }
}

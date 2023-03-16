using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class controllerTypes : MonoBehaviour
{
    public bool Xbox_One_Controller = false;
    public bool PS4_Controller = false;
    public bool keyboard = false;
    void Update()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            // print(names[x].Length);
            if (names[x].Length == 19)
            {
                // print("PS4 CONTROLLER IS CONNECTED");
                PS4_Controller = true;
                Xbox_One_Controller = false;
            }
            if (names[x].Length == 33)
            {
                // print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4_Controller = true;
                Xbox_One_Controller = false;

            }
        }


        if (!Xbox_One_Controller && !PS4_Controller)
        {
            keyboard = true;
        } else
        {
            keyboard = false;
        }

        //print("xbox " + Xbox_One_Controller);
        //print("ps4 " +  PS4_Controller);
        //print("keyboard " +  keyboard);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private UnityEngine.InputSystem.Gyroscope gyro;
    // Start is called before the first frame update
    void Start()
    {
        InputSystem.EnableDevice(gyro);
    }

    // Update is called once per frame
    void Update()
    {
		if (gyro.enabled)
		{
            Debug.Log("Gyroing");
		}
    }
}

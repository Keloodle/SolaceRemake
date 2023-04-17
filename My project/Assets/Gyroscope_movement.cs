using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope_movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    void Start()
    {
        Input.gyro.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(-Input.gyro.attitude.x * moveSpeed, -Input.gyro.attitude.y * moveSpeed);
    }
}

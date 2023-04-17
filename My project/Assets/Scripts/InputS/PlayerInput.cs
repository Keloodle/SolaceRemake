using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerControls controls;
    float direction = 0;
    public Rigidbody2D rb;

    public float movementSpeed = 5;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();

        controls.Ground.Move.performed += ctx =>
        {
            direction = ctx.ReadValue<float>();
        };
    }
    void Update()
    {
        rb.velocity = new Vector2(direction * movementSpeed * Time.deltaTime, rb.velocity.y);
    }
}

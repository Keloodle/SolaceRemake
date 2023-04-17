using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [Header("Movement")]
    PlayerControls controls;
    float direction = 0;
    float jumpDirection = 0;
    public Rigidbody2D rb;

    public float movementSpeed = 500;
   

    [Header("Ground Check")]
    public bool isGrounded = false;
    public LayerMask groundLayer;
    public float groundedHeight = 0.51f;
    public float heightOffset = 0.25f;

    [Header("Jumping")]
    private float startHeight = 10000;

    public float jumpSpeed = 250;


    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();

        controls.Ground.Move.performed += ctx =>
        {
            direction = ctx.ReadValue<float>();
        };

        controls.Ground.Jump.performed += ctx =>
        {
            jumpDirection = ctx.ReadValue<float>();
        };
    }
    void Update()
    {
        GroundCheck();
        rb.velocity = new Vector2(direction * movementSpeed, rb.velocity.y);
        if (jumpDirection > 0 && isGrounded)
        {
            startHeight = this.transform.position.y;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        //print("jump");
        // Jumping
        if (context.performed && isGrounded)
        {
            startHeight = this.transform.position.y;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + heightOffset), groundedHeight, groundLayer);
    }
}

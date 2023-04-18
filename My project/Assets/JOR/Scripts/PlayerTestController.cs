using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTestController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Vector2 movementInput;

    void Move(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector2 movement = movementInput * moveSpeed * Time.fixedDeltaTime;
        transform.position += new Vector3(movement.x, 0f, 0f);
    }
}

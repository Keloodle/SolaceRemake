using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private float height;

    public bool isGrounded;
    private bool groundCheck = true;

    public Animator camAnim;
    private bool keyDown = false;

    void Start()
    {
        height = 2.001f;
    }

    public void lookDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            keyDown = true;
        }
        if (context.canceled)
        {
            keyDown = false;
        }
    }


    void Update()
    {
        // Looking down
        if (keyDown)
        {
            if(height >= -2)
            {
                height -= 7.5f * Time.deltaTime;
            }
        } else if(height <= 2)
        {
            height += 15f * Time.deltaTime;
        } else if(height >= 2)
        {
            height = 2.001f;
        }

        transform.localPosition = new Vector3(0, height, 0);


        // Landing code logic
        isGrounded = FindObjectOfType<character>().grounded;
        if(isGrounded == true && groundCheck == true)
        {
            camAnim.SetTrigger("landed");
            groundCheck = false;
        }
        if(isGrounded == false)
        {
            groundCheck = true;
        }
    }

    public void gotHit()
    {
        camAnim.SetTrigger("hitShake");
    }

    public void dealDamage()
    {
        camAnim.SetTrigger("dealDmg");
    }

}

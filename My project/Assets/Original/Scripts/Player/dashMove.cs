using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class dashMove : MonoBehaviour
{

    private Rigidbody2D rb;

    public float dashSpeed;
    public float dashTime;
    public float startDashTime;
    private bool canDash = true;
    private bool wallDash = false;
    float timer = 0;
    [SerializeField] private float coolDownTime = 0.5f;
    public SpriteRenderer dashMeter;

    private int direction;
    private float playerLooking;
    private float playerEnergy;

    [SerializeField] private GameObject dashParticle;

    public Animator guardianAnim;
    public Animator dashAnim;
    public bool isDashing = false;

    private bool toDash = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start()
    {
        timer = coolDownTime;
        dashTime = startDashTime;
    }

    public void dash(InputAction.CallbackContext context)
    {
        if (context.started && timer > coolDownTime && GetComponent<character>().isSitting == false && GetComponent<shockShield>().shieldOn == false && FindObjectOfType<PauseMenu>().paused == false && FindObjectOfType<PauseMenu>().inventoryUp == false && FindObjectOfType<DialogueManager>().isUp == false)
        {
            toDash = true;
        }
    }

    void Update()
    {
        // Gets the direction of the player
        playerLooking = FindObjectOfType<character>().looking;

        //Makes it so the player gains dash when grounded
        if(FindObjectOfType<character>().grounded){
            canDash = true;
        }

        // print(timer + " " + coolDownTime);

        if(timer > coolDownTime)
        {
            // Can dash now
            dashAnim.SetBool("dash", false);
        }
        

        if(FindObjectOfType<character>().onWall == true && isDashing == true && !wallDash)
        {
            wallDash = true;
            stopDash();
        }

        if (direction == 0)
        {
            // if (Input.GetKeyDown(KeyCode.F) && canDash && timer > coolDownTime && FindObjectOfType<PauseMenu>().paused == false && !FindObjectOfType<character>().isSitting || Input.GetKeyDown(KeyCode.LeftShift) && canDash && timer > coolDownTime && FindObjectOfType<PauseMenu>().paused == false && !FindObjectOfType<character>().isSitting)
            if(timer > coolDownTime && toDash == true)
            {
                // start Dash
                toDash = false;
                var prefab = Instantiate(dashParticle, this.transform.position, Quaternion.identity);
                Destroy(prefab, 1f);
                direction = 1;
                // FindObjectOfType<energyController>().reduceEnergy(1f);
                canDash = false;
                timer = 0;
                guardianAnim.SetBool("isDashing", true);
                dashAnim.SetBool("dash", true);
                isDashing = true;
                if(FindObjectOfType<character>().onWall == true)
                {
                    wallDash = true;
                }
            }
        } else
        {
            if(dashTime <= 0)
            {
                // stop Dash
                stopDash();
                rb.velocity = Vector2.zero;
            } else
            {
                dashTime -= Time.deltaTime;
            }
            if (direction == 1)
            {
                // During dash?
                rb.velocity = Vector2.right * dashSpeed * playerLooking;
                toDash = false;
            }
        }

        timer += Time.deltaTime;

        // calculate time left

        dashMeter.material.SetFloat("_Health", timer / coolDownTime);

    }

    private void stopDash()
    {
        direction = 0;
        dashTime = startDashTime;
        guardianAnim.SetBool("isDashing", false);
        isDashing = false;
        toDash = false;
        wallDash = false;
    }

    public void enemyCollided()
    {
        dashTime = 0;
        rb.velocity = new Vector2(0, 0);
    }

}

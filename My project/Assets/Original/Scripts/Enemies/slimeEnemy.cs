using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slimeEnemy : MonoBehaviour
{
    [SerializeField] private Vector2 positions = new Vector2(-3, 3);
    private Vector2 patrolPositions;

    [SerializeField] private float moveSpeed = 3;
    private int cDir = 1;
    private int direction = 1;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float checkDistance = 5;
    private Transform player;
    private bool playerClose = false;

    public int enemyBehaviour = 1;

    [SerializeField] private Animator anim;
    private Rigidbody2D rb;
    public bool onGround = true;
    public bool onGround2 = true;
    [SerializeField] private Vector2 jumpLength = new Vector2(4, 4);
    [SerializeField] private Vector2 initJump = new Vector2(4, 4);
    [SerializeField] private Vector2 jumpTimer = new Vector2(1, 1);

    public Vector2 groundedHeight = new Vector2(0,0.51f);
    public Vector2 groundedHeight2 = new Vector2(0, 0.51f);
    public LayerMask groundLayer;
    public float heightOffset = 0.25f;
    public float heightOffset2 = 0.25f;
    private bool groundCheck = true;


    void Start()
    {
        patrolPositions = new Vector2(positions.x + this.transform.position.x, positions.y + this.transform.position.x);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        InvokeRepeating("keepJumping", jumpTimer.x, jumpTimer.y);
        rb.velocity += initJump;
    }


    void Update()
    {
        onGround = Physics2D.OverlapCircle(new Vector2(transform.position.x + groundedHeight.x, transform.position.y + heightOffset), groundedHeight.y, groundLayer);
        onGround2 = Physics2D.OverlapCircle(new Vector2(transform.position.x + groundedHeight2.x, transform.position.y + heightOffset2), groundedHeight2.y, groundLayer);
        this.transform.localScale = new Vector3(cDir, 1, 1);

        // State machine
        switch (enemyBehaviour)
        {
            case 1:
                patrol();
                break;
            case 2:
                follow();
                break;
            case 3:
                // Shoot player
                break;
            case 4:
                // 
                break;
            default:
                break;
        }

        // Landing animation logic
        if (onGround == true && groundCheck == true || onGround2 && groundCheck == true)
        {
            // print("just landed");
            anim.SetBool("Down", true);
            groundCheck = false;
        }
        if (onGround == false || onGround2 == false)
        {
            anim.SetBool("Down", false);
            groundCheck = true;
        }
        anim.SetFloat("Velocity", rb.velocity.y);
    }

    // Patrol script
    private void patrol()
    {

        // Makes the enemy jump between patrol points
        if (transform.position.x < patrolPositions.x)
        {
            // print("Too far left");
            cDir = -1;
        }
        if (patrolPositions.y < transform.position.x)
        {
            // print("Too far right");
            cDir = 1;
        }

    }

    // Simple function that moves it in a direction.
    void moveEnemy()
    {
        if (this.GetComponent<EnemyDamageTaken>().isDead == false)
        {
            // print("up?");
            anim.SetTrigger("Up");
            rb.velocity = new Vector2(jumpLength.x * -cDir, jumpLength.y);
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos, transform.position.y, 0), Time.deltaTime * moveSpeed);
        }
    }

    void keepJumping()
    {
        if(onGround == true || onGround2 == true)
        {
            moveEnemy();
        }
    }

    // Follow and attack player
    private void follow()
    {
        // Decides the direction based on player
        if (player.transform.position.x > transform.position.x)
        {
            cDir = -1;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            cDir = 1;
        }
        /*
        // Checks if the player is in front
        Debug.DrawRay(transform.position, Vector2.right * -cDir * 1.75f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * -cDir, 1.75f, playerLayer);
        if (hit.collider != null)
        {
            // Attacks
            // anim.SetBool("attacking", true);

        }
        else
        {
            // Stops attacking because the player left
            anim.SetBool("attacking", false);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("iceClownRun"))
            {
                moveEnemy();
            }
        }*/
    }

    private void FixedUpdate()
    {
        // Checks for player inside area
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), checkDistance, playerLayer);
        if (enemyColliders.Length > 0)
        {
            playerClose = true;
            player = enemyColliders[0].transform;
            // Attacks
            enemyBehaviour = 2;
        }
        else
        {
            playerClose = false;
            enemyBehaviour = 1;
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0.75f, 1, 0.5f);
        Gizmos.DrawCube(new Vector3(patrolPositions.x, this.transform.position.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawCube(new Vector3(patrolPositions.y, this.transform.position.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawWireCube(new Vector3(positions.x + transform.position.x, this.transform.position.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawWireCube(new Vector3(positions.y + transform.position.x, this.transform.position.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + groundedHeight.x, transform.position.y + heightOffset, transform.position.z), groundedHeight.y);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x + groundedHeight2.x, transform.position.y + heightOffset2, transform.position.z), groundedHeight2.y);
    }

}
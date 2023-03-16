using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trixy2 : MonoBehaviour
{
    [SerializeField] private Vector2 positions = new Vector2(-3, 3);
    private Vector3 patrolPositions1;
    private Vector3 patrolPositions2;

    [SerializeField] private float moveSpeed = 3;
    private int cDir = 1;
    private int yDir = 1;
    private int direction = 1;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float checkDistance = 5;
    [SerializeField] private float shootDistance = 5;
    public GameObject projectile;
    private Transform player;
    private bool playerClose = false;

    public int enemyBehaviour = 1;

    [SerializeField] private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private float moveTimer = 1.4f;
    [SerializeField] private Vector2 moveLength = new Vector2(4, 4);
    [SerializeField] private Transform shotPoint;
    private bool canMove = true;
    private bool isMoving = false;
    private bool canFollow = true;
    private bool canFire = true;



    void Start()
    {
        patrolPositions1 = new Vector3(positions.x + transform.position.x, transform.position.y, transform.position.z);
        patrolPositions2 = new Vector3(positions.y + transform.position.x, transform.position.y, transform.position.z);

        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        this.transform.localScale = new Vector3(cDir, 1, 1);
        switch (enemyBehaviour)
        {
            case 1:
                patrol();
                break;
            case 2:
                follow();
                break;
            case 3:
                shoot();
                break;
            case 4:
                // 
                break;
            default:
                break;
        }

        if (canFire && anim.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Trixy_26" || canFire && anim.gameObject.GetComponent<SpriteRenderer>().sprite.name == "trixyEnd_26")
        {
            StartCoroutine(fire());
        }
    }

    IEnumerator moveEnemy(Vector3 targetPos)
    {
        if(canMove && !isMoving && !anim.GetCurrentAnimatorStateInfo(0).IsName("trixyShoot"))
        {
            rb.velocity = new Vector2(moveLength.x * -cDir, moveLength.y * yDir);
            isMoving = true;
            anim.SetTrigger("dashing");
            yield return new WaitForSeconds(moveTimer);
            isMoving = false;
            yield return StartCoroutine(moveEnemy(targetPos));

        }
    }

    // Simple function that moves it in a direction.
    /*
    void moveEnemy(Vector3 targetPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);

    }
    */

    // Patrol script
    private void patrol()
    {
        // Moves the enemy between patrol points
        if (direction == 1)
        {
            StartCoroutine(moveEnemy(patrolPositions1));
            if (transform.position.x == patrolPositions1.x)
            {
                cDir = -1;
                direction = -1;
            }
            if(transform.position.y > patrolPositions1.y)
            {
                yDir = -1;
            } else if(transform.position.y < patrolPositions1.y)
            {
                yDir = 1;
            }
        }
        else if (direction == -1)
        {
            StartCoroutine(moveEnemy(patrolPositions2));
            if (transform.position.x == patrolPositions2.x)
            {
                cDir = 1;
                direction = 1;
            }
            if (transform.position.y > patrolPositions2.y)
            {
                yDir = -1;
            }
            else if (transform.position.y < patrolPositions2.y)
            {
                yDir = 1;
            }
        }

        // If the enemy is outside of the patrol points, it looks towards the patrol points
        if (transform.position.x < patrolPositions1.x)
        {
            // print("Too far left");
            cDir = -1;
        }
        if (patrolPositions2.x < transform.position.x)
        {
            // print("Too far right");
            cDir = 1;
        }

    }


    // Follow and attack player
    private void follow()
    {

        // Start from other

        // The enemy spots the player
        
        // If player is too close, it stops.

            // shoots
            /*
            if (timeBtwShots <= 0)
            {
                GameObject prefab = Instantiate(projectile, this.transform.position, this.transform.rotation);
                prefab.transform.right = player.transform.position - prefab.transform.position;
                timeBtwShots = startTimeBtwShots;
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }
            */

        // Stop
        

        // Decides the direction based on player
        if (player.transform.position.x > transform.position.x)
        {
            cDir = -1;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            cDir = 1;
        }
        if (transform.position.y > player.transform.position.y)
        {
            yDir = -1;
        }
        else if (transform.position.y < player.transform.position.y)
        {
            yDir = 1;
        }

        StartCoroutine(moveEnemy(player.transform.position));

    }

    private void shoot()
    {
        if (player.transform.position.x > transform.position.x)
        {
            cDir = -1;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            cDir = 1;
        }
        if (canFollow)
        {
            // StartCoroutine(shooting());
            // canFollow = false;
            anim.SetTrigger("shoot");
            canMove = false;
        }   

    }

    IEnumerator fire()
    {
        canFire = false;
        GameObject prefab = Instantiate(projectile, shotPoint.position, this.transform.rotation);
        prefab.transform.right = player.transform.position - prefab.transform.position;
        yield return new WaitForSeconds(0.2f);
        canFire = true;
    }

    /*IEnumerator shooting()
    {
        // First, stop moving.
        canMove = false;

        // Secondly, play animation.
        anim.SetTrigger("shoot");

        // Third, wait until animation stops.
        yield return new WaitForSeconds(2f);

        // Then, send enemy back to patrol points.
        canMove = true;
        moveEnemy(patrolPositions1);
        enemyBehaviour = 1;

        // Lastly, make it so it can shoot again.
        yield return new WaitForSeconds(3f);
        canFollow = true;

    }*/

    private void FixedUpdate()
    {
        // print(enemyBehaviour);
        // Checks for player inside area
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), checkDistance, playerLayer);
        if (enemyColliders.Length > 0)
        {
            playerClose = true;
            player = enemyColliders[0].transform;
            
            // Checks if enemy is within shooting distance
            if (Vector2.Distance(transform.position, player.transform.position) < shootDistance/* && canFollow*/)
            {
                // If it is, start shooting
                enemyBehaviour = 3;
            } else
            {
                // If not, chase the player
                enemyBehaviour = 2;
                canMove = true;
            }

        }
        else
        {
            playerClose = false;
            canMove = true;
            enemyBehaviour = 1;
        }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0.75f, 1, 0.5f);
        Gizmos.DrawCube(new Vector3(patrolPositions1.x, patrolPositions1.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawCube(new Vector3(patrolPositions2.x, patrolPositions2.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawWireCube(new Vector3(positions.x + transform.position.x, this.transform.position.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawWireCube(new Vector3(positions.y + transform.position.x, this.transform.position.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = new Color(1, 0.75f, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, shootDistance);
    }



}

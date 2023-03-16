using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrolEnemy : MonoBehaviour
{
    [SerializeField] private Vector2 positions;
    private Vector2 patrolPositions;
    [SerializeField] private float moveSpeed;
    private int cDir = 1;
    private bool patrolling = true;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float checkDistance = 5;
    [SerializeField] private float seeDistance = 5;
    [SerializeField] private float hitLength = 1.5f;
    private bool playerClose = false;
    private bool playerDetected = false;
    private Transform player;
    private bool playerAir = false;
    private float nextToPlayer;

    private Animator anim;
    private IEnumerator coroutine;


    void Start()
    {
        patrolPositions = new Vector2(positions.x + this.transform.position.x, positions.y + this.transform.position.x);
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        this.transform.localScale = new Vector3(cDir, 1, 1);
        if (patrolling && !playerDetected)
        {
            // Decided what direction it should go and makes it go in that direction.
            if(cDir == 1)
            {
                moveEnemy(patrolPositions.x);
                if(transform.position.x == patrolPositions.x){cDir = -1;};
            } else if (cDir == -1)
            {
                moveEnemy(patrolPositions.y);
                if (transform.position.x == patrolPositions.y){cDir = 1;};
            }
        }

        // Plays when enemy has seen player
        if (playerDetected)
        {
            // Chases player
            if (patrolling)
            {
                moveEnemy(player.transform.position.x);
            }
            if(player.transform.position.x > transform.position.x)
            {
                cDir = -1;
            } else if (player.transform.position.x < transform.position.x)
            {
                cDir = 1;
            }

            // Checks if the player is in the air (So that it can stop and start shooting)
            if (player.GetComponent<character>().grounded == false)
            {
                playerAir = true;
            } else
            {
                playerAir = false;
            }
            //print("Player position: " + player.position.x + ". Enemy position: " + transform.position.x + ". Target area: " + nextToPlayer);
            if(!playerAir)
            {
                if(nextToPlayer < transform.position.x && transform.position.x < player.position.x)
                {
                    patrolling = false;
                    //print("right side");
                    anim.SetBool("isSwinging", true);
                }
                else if(player.position.x < transform.position.x && transform.position.x < nextToPlayer)
                {
                    patrolling = false;
                    //print("left side");
                    anim.SetBool("isSwinging", true);

                }
                coroutine = waitToMove(0.1f);
                StartCoroutine(coroutine);
            }

        }

    }

    //damagePlayer


    private IEnumerator waitToMove(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            if (patrolling == false && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                anim.SetBool("isSwinging", false);
                patrolling = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // Checks for player inside area
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), checkDistance, playerLayer);
        if (enemyColliders.Length > 0)
        {
            playerClose = true;
            player = enemyColliders[0].transform;
        }
        else
        {
            playerClose = false;
            playerDetected = false;
        }
        // Checks for player in front of enemy
        Debug.DrawRay(transform.position, Vector2.right * -cDir * seeDistance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * -cDir, seeDistance, playerLayer);
        if (hit.collider != null)
        {
            playerDetected = true;

            // gets a position right in front of the player (So that it can stop there and attack)
            nextToPlayer = player.transform.position.x + cDir * hitLength;
        }
    }

    // Simple function that moves it in a direction.
    void moveEnemy(float targetPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos, transform.position.y, 0), Time.deltaTime * moveSpeed);
    }

    // Only to show where it walks.
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(0, 0.75f, 1, 0.5f);
        Gizmos.DrawCube(new Vector3(patrolPositions.x, this.transform.position.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawCube(new Vector3(patrolPositions.y, this.transform.position.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawWireCube(new Vector3(positions.x + transform.position.x, this.transform.position.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawWireCube(new Vector3(positions.y + transform.position.x, this.transform.position.y, 0), new Vector3(1, 1, 1));
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(new Vector3(nextToPlayer, this.transform.position.y, 0), new Vector3(0.25f, 0.25f, 0.25f));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyshooting : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public float retreaDistance;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public bool goEnemy = false;


    public Vector3 enemyPos;
    public Transform player;
    public GameObject projectile;

    public Patrol patrol;
    public GameObject trixyEnemy;
    private Trixy trixyScript;
    public bool canShoot = false;

    public float dmgTaken = 0;

    //public GameObject player;
    public float frequency = 2f;


    // Start is called before the first frame update
    void Start()
    {
        trixyScript = trixyEnemy.GetComponent<Trixy>();
        enemyPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        // Simply flips the sprite
        if (!goEnemy)
        {
            if(this.transform.position.x > patrol.currentPos.x)
            {
                trixyEnemy.GetComponent<SpriteRenderer>().flipX = true;
            } else
            {
                trixyEnemy.GetComponent<SpriteRenderer>().flipX = false;
            }
        } else if (goEnemy)
        {
            if (this.transform.position.x > player.position.x)
            {
                trixyEnemy.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                trixyEnemy.GetComponent<SpriteRenderer>().flipX = false;
            }
        }


        // The enemy spots the player
        if (goEnemy)
        {
            // If the player is far enough to follow player
            if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            // I don't know why this is here. It basically says "this position = this position", soo...
            else if (Vector2.Distance(transform.position, player.position) > stoppingDistance && Vector2.Distance(transform.position, player.position) > retreaDistance)
            {
                print("this = this");
                transform.position = this.transform.position;
            }
            // If player is too close, it stops.
            else if (Vector2.Distance(transform.position, player.position) > retreaDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            }

            // shoots
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
        } // The enemy does not see the player. Follows patrol.
        else if(!goEnemy)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrol.currentPos, speed * Time.deltaTime);
        }  
    }

    // Player is close enough to trigger activation

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "player")
        {
            goEnemy = true;
        }
        
    }

    // Player is far enough to deactivate

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "player")
        {
            goEnemy = false;
        }
    }

}
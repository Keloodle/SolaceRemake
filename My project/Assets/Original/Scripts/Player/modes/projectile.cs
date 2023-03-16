using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    private Vector3 enemyPos;
    private Rigidbody2D rb;
    private Transform enemy;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = FindObjectOfType<floatingOrb>().enemy01;
    }
    
    void Update()
    {
        if(enemy != null && FindObjectOfType<follower>() != null)
        {
            enemyPos = enemy.position;
            transform.position = Vector3.MoveTowards(transform.position, enemyPos, Time.deltaTime * FindObjectOfType<follower>().shotSpeed);
        } else if(enemy == null || FindObjectOfType<follower>() == null)
        {
            Destroy(gameObject);
        }

        //rb.MovePosition(rb.)

        if(transform.position == enemyPos)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        {
            if (collision.gameObject.layer == 11)
            {
                Destroy(gameObject);
                //collision.gameObject.GetComponent<enemy>().increaseDmg();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 11)
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<enemy>().increaseDmg();
        } else
        {
            Destroy(gameObject);
        }
    }
    */
}
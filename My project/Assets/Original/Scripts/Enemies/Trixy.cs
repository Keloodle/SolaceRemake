using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trixy : MonoBehaviour
{
    public Enemyshooting enemyScript;
    public bool onWall;
    public float dmgTaken = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 10 /*|| /*collision.gameObject.name != "player"*/)
        {
            enemyScript.goEnemy = false;
            onWall = true;
        }
        if(collision.gameObject.name == "player")
        {
            //print("hi");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.name == "player")
        {
            Invoke("continueAttacking", 1f);
            onWall = false;
        }
    }

    public void continueAttacking()
    {
        enemyScript.goEnemy = true;
    }

    // This hurts the enemy

    public void increaseDmg()
    {
        dmgTaken++;
        if (dmgTaken == 10)
        {
            Destroy(this.gameObject);
        }
        print(dmgTaken);
    }

    private void Update()
    {
        if(this.transform.position != enemyScript.gameObject.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemyScript.gameObject.transform.position, Time.deltaTime * 10);
        }
    }

}
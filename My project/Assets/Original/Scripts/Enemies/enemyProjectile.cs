using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.transform.name == "shieldBox")
        {
            print("hit shield");
        }
        else if(collision.transform.name == "player")
        {
            FindObjectOfType<damagePlayer>().recieveDamage();
        }
        Destroy(gameObject);
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*if (collision.gameObject.transform.name == "player")
        {
            FindObjectOfType<damagePlayer>().recieveDamage();
            Destroy(gameObject);
        }*/
        if (collision.transform.gameObject.layer == 11 || collision.transform.gameObject.layer == 9)
        {
            
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

}

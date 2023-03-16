using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingHeart : MonoBehaviour
{
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            FindObjectOfType<damagePlayer>().recieveHealth();
            Destroy(gameObject);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 12)
        {
            FindObjectOfType<damagePlayer>().recieveHealth();
            destroyMyHeart();
        }
    }

    void destroyMyHeart()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetTrigger("picked");
        Destroy(gameObject, 3f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soulSpring : MonoBehaviour
{
    private bool playerIn = false;
    [SerializeField] private GameObject soul;

    void Start()
    {
        InvokeRepeating("spawnSoul", 0.75f, 0.75f);
    }

    void spawnSoul()
    {
        if(playerIn)
        {
            var prefab = Instantiate(soul, transform.position, transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerIn = true;
        this.GetComponent<Animator>().SetBool("active", true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerIn = false;
        this.GetComponent<Animator>().SetBool("active", false);
    }

}

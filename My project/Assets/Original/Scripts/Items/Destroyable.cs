using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{

    public SpriteRenderer sprite;
    public GameObject destructionEffect;
    public BoxCollider2D triggerCollider;


    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void destroyObject()
    {
        sprite.enabled = false;
        triggerCollider.enabled = false;
        var prefab = Instantiate(destructionEffect, this.transform.position, this.transform.rotation);
        Destroy(prefab, 3f);
    }
}

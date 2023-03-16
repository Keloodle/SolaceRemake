using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingOrb : MonoBehaviour
{

    RaycastHit2D enemiesCheck;
    public float circleDistance = 2f;
    public LayerMask enemyLayer;
    public Transform enemy01;
    public bool enemyClose = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), circleDistance, enemyLayer);
        if (enemyColliders.Length > 0)
        {
            enemyClose = true;
            enemy01 = enemyColliders[0].transform;
        }
        else
        {
            enemyClose = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, circleDistance);
    }
}

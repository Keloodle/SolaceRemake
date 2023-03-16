using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingIcicles : MonoBehaviour
{
    private Vector3 startPos;
    private Rigidbody2D irb;
    [SerializeField] private LayerMask playerLayer;
    private bool isFalling = false;
    [SerializeField] private float fallDistance = 5;

    void Start()
    {
        irb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, fallDistance, playerLayer);
        // If it hits something...
        if (!isFalling && hit.collider != null)
        {
            isFalling = true;
            irb.bodyType = RigidbodyType2D.Dynamic;
            StartCoroutine(resetFall());
        }
    }

    IEnumerator resetFall()
    {
        yield return new WaitForSeconds(2f);
        // Animation to grow back
        GetComponent<Animator>().SetTrigger("grow");
        irb.bodyType = RigidbodyType2D.Static;
        transform.position = startPos;
        yield return new WaitForSeconds(1f);
        // Be able to fall again
        isFalling = false;

    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, -Vector2.up * fallDistance, Color.green);
    }

}

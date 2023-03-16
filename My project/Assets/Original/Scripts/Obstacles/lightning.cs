using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightning : MonoBehaviour
{
    private Transform player;
    private bool inZone = false;

    [SerializeField] private Vector4 leftRightTopBottom;
    [SerializeField] private float floorHeight;
    private Vector3 hitSpot;

    [SerializeField] private GameObject spark;
    [SerializeField] private GameObject lightningEffect;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<character>().transform;
        StartCoroutine(lightningFunction());
    }
    
    void Update()
    {
        if(checkBox(leftRightTopBottom.x + transform.position.x, leftRightTopBottom.y + transform.position.x, leftRightTopBottom.z + transform.position.y, leftRightTopBottom.w + transform.position.y))
        {
            inZone = true;
            hitSpot = new Vector3(player.position.x, floorHeight, 0);
        } else
        {
            inZone = false;
        }
    }

    private IEnumerator lightningFunction()
    {
        if (inZone)
        {
            // Sparks:
            GameObject sparkTemp = Instantiate(spark, hitSpot, transform.rotation);
            Destroy(sparkTemp, 0.5f);
            yield return new WaitForSeconds(1f);
            sparkTemp = Instantiate(spark, hitSpot, transform.rotation);
            Destroy(sparkTemp, 0.5f);

            // Lightning strike:
            yield return new WaitForSeconds(0.75f);
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.25f);
            GameObject lightStrike = Instantiate(lightningEffect, hitSpot, transform.rotation);
            Destroy(lightStrike, 1f);

            yield return new WaitForSeconds(3f);
            StartCoroutine(lightningFunction());


        } else
        {
            // print("No player nearby. Tries again");
            yield return new WaitForSeconds(3f);
            StartCoroutine(lightningFunction());
        }

    }




    bool checkBox(float left, float right, float top, float bottom)
    {
        if (player != null && left < player.position.x && player.position.x < right && bottom < player.position.y && player.position.y < top)
        {
            return true;
        } else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.75f, 1, 0, 0.5f);
        Gizmos.DrawCube(new Vector3(leftRightTopBottom.x, leftRightTopBottom.z, 1) + transform.position, new Vector3(1, 1, 1));
        Gizmos.DrawCube(new Vector3(leftRightTopBottom.x, leftRightTopBottom.w, 1) + transform.position, new Vector3(1, 1, 1));
        Gizmos.DrawCube(new Vector3(leftRightTopBottom.y, leftRightTopBottom.z, 1) + transform.position, new Vector3(1, 1, 1));
        Gizmos.DrawCube(new Vector3(leftRightTopBottom.y, leftRightTopBottom.w, 1) + transform.position, new Vector3(1, 1, 1));
        Gizmos.color = new Color(1f, 0, 0.75f, 0.5f);
        Gizmos.DrawCube(new Vector3(transform.position.x, floorHeight, transform.position.z), new Vector3(0.5f, 0.5f, 0.5f));
    }



}

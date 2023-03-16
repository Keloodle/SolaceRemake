using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySouls : MonoBehaviour
{
    private Vector2 energyBarPos;
    private bool canGive = true;
    
    void Update()
    {
        // Check if energy is full.
        if(FindObjectOfType<energyController>().energy == FindObjectOfType<energyController>().maxEnergy)
        {
            // Play fade out animation
            Destroy(gameObject);
        }
        // Get position of souls from Energy Script
            energyBarPos = FindObjectOfType<energyController>().energyPosition;
        // Send the souls to the energy
        Vector2 thisPos = new Vector2(transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(thisPos, energyBarPos, 17 * Time.deltaTime);
        // Add ewnergy and delete this.
        if(thisPos == energyBarPos && canGive)
        {
            canGive = false;
            FindObjectOfType<energyController>().recieveEnergy();
            Destroy(gameObject);
        }
    }
}

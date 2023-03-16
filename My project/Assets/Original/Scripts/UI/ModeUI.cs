using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModeUI : MonoBehaviour
{
    public Transform[] positions;
    public Transform[] icons;
    public Transform[] selectedIcons;
    Vector3 worldPosition;
    public Transform closestPos;

    public bool canSwap = false;

    /*
    public void controllerSwap(InputAction.CallbackContext context)
    {
        if (context.started)
        {

            var value = context.ReadValue<Vector2>();
            if(value.y == 1)
            {
                // print("top");
                closestPos = positions[2];
            } else if (value.x == 1)
            {
                // print("right");
                closestPos = positions[0];
            }
            else if (value.y == -1)
            {
                // print("bottom");
                closestPos = positions[1];
            }
            else if (value.x == -1)
            {
                // print("left");
                closestPos = positions[3];
            }
        }
    }
    */

    public int currentAbilities = 1;
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        closestPos = GetClosestEnemy(positions);

        if (FindObjectOfType<modeSelector>().psDpadWheel == false)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                selectedIcons[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                if (i+1 <= currentAbilities)
                {
                    // print(positions[i] + " and " + closestPos);
                    if(positions[i] == closestPos)
                    {
                        if (Input.GetKey(KeyCode.Tab))
                        {
                            icons[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                            selectedIcons[i].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        // print(icons[i]);
                        if (Input.GetKeyUp(KeyCode.Tab) && canSwap == true)
                        {
                            selectedIcons[i].gameObject.GetComponent<SpriteRenderer>().enabled = false;
                            GameObject.FindObjectOfType<modeSelector>().combatMode(i+1);
                            canSwap = false;
                        }
                    }
                    if(selectedIcons[i].gameObject.GetComponent<SpriteRenderer>().enabled == false)
                    {
                        icons[i].gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
        }
    }

    // Gets closest thing to mouse
    Transform GetClosestEnemy(Transform[] sides)
        {
            Transform tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = worldPosition;
            foreach (Transform t in sides)
            {
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin;
        }
}

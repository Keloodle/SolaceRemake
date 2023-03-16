using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaBlocker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<BoxCollider2D>().enabled = !FindObjectOfType<modeSelector>().shieldCircle.GetComponent<CircleCollider2D>().enabled;
    }
}

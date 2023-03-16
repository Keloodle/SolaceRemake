using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Vector3 enemyStartPos;
    public Vector3 currentPos;
    public Enemyshooting ES;

    // Start is called before the first frame update
    void Start()
    {
        enemyStartPos = transform.position;
        InvokeRepeating("ChangePosition", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = currentPos;
        if(ES.transform.position == currentPos)
        {
            CancelInvoke("ChangePosition");
            InvokeRepeating("ChangePosition", 0, 2);
        }
    }

    void ChangePosition()
    {
        currentPos = enemyStartPos + new Vector3(Random.Range(-2, 2), Random.Range(-1, 1), 0);
    }
}

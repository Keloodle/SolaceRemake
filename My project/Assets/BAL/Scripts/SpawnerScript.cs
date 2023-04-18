using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject objectToSpawn;


    public GameObject spawnToObject;

    public float timeToSpawn;

    private float currentTimeToSpawn;

    public bool isTimer;

    void Start()
    {
        currentTimeToSpawn = timeToSpawn;
    }

    void Update()
    {
        if (isTimer)
        UpdateTimer();
    }


    private void UpdateTimer()
    {
        if(currentTimeToSpawn > 0)
        {
            currentTimeToSpawn -= Time.deltaTime;
        }

        else
        {
            SpawnObject();
            currentTimeToSpawn = timeToSpawn;
        }
    }


    public void SpawnObject()
    {
        Instantiate(objectToSpawn, transform.position, transform.rotation);
    }
}

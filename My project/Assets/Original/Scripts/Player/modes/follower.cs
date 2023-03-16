using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class follower : MonoBehaviour
{
    [SerializeField] private Transform targetSpot;
    [SerializeField] private float moveSpeed;
    private Vector3 velocity = Vector3.zero;
    public Vector3 offset = new Vector3(2, 2, 0);

    public GameObject shot;
    public float shotSpeed = 10;

    float timer = 0;
    private float coolDownTime = 0.5f;

    private void Update()
    {
        // Tracking:
        Vector3 realPos = targetSpot.position + offset;
        transform.position = Vector3.Lerp(transform.position, realPos, Time.deltaTime * moveSpeed);

        // Shooting:

        /*if (timer > coolDownTime && Input.GetKeyDown(KeyCode.Mouse0) && FindObjectOfType<floatingOrb>().enemyClose && FindObjectOfType<energyController>().energy > 0)
        {
            FindObjectOfType<energyController>().reduceEnergy(0.5f);
            GameObject prefab = Instantiate(shot, this.transform.position, this.transform.rotation);
            timer = 0;
        }
        */

        timer += Time.deltaTime;
    }

    public void shoot(InputAction.CallbackContext context)
    {
        if (timer > coolDownTime && context.started && FindObjectOfType<floatingOrb>().enemyClose && FindObjectOfType<energyController>().energy > 0 && FindObjectOfType<floatingOrb>().enabled)
        {
            FindObjectOfType<energyController>().reduceEnergy(1f);
            GameObject prefab = Instantiate(shot, this.transform.position, this.transform.rotation);
            timer = 0;
        }
    }

} 


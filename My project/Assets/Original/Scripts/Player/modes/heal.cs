using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class heal : MonoBehaviour
{
    private energyController energyScript;
    private damagePlayer healthScript;

    [SerializeField] private Animator camAnim;

    private void Start()
    {
        energyScript = FindObjectOfType<energyController>();
        healthScript = FindObjectOfType<damagePlayer>();

    }
    public void holdButton(InputAction.CallbackContext context)
    {
        if (this.enabled)
        {
            if (context.started && this.enabled == true && energyScript.energy > 3 && healthScript.hp < 5 )
            {
                //print("start");
                StartCoroutine(chargeHeal());
                camAnim.SetBool("charging", true);
            }
            else if (context.canceled)
            {
                //print(context.duration);
                //print("stop");
                StopAllCoroutines();
                camAnim.SetBool("charging", false);
            }
        }
    }

    public void disableCharge()
    {
        StopAllCoroutines();
    }

    private IEnumerator chargeHeal()
    {
        yield return new WaitForSeconds(0.25f);
        energyScript.reduceEnergy(1);
        yield return new WaitForSeconds(0.25f);
        energyScript.reduceEnergy(1);
        yield return new WaitForSeconds(0.25f);
        energyScript.reduceEnergy(1);
        healthScript.recieveHealth();
        camAnim.SetBool("charging", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftLever : MonoBehaviour
{
    [SerializeField] private Lift liftObject;

    [SerializeField] private GameObject onSprite;
    [SerializeField] private GameObject offSprite;

    private bool canHit = false;

    [SerializeField] private AudioSource woodHit;
    [SerializeField] private AudioSource activateLever;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        onSprite.SetActive(canHit);
        offSprite.SetActive(!canHit);

        if(liftObject.moveUp == true && !liftObject.onBottom || liftObject.onTop && !liftObject.onBottom)
        {
            canHit = true;
        }
        else if(liftObject.moveUp == false)
        {
            canHit = false;
        }
    }

    public void trigger()
    {
        woodHit.Play();
        liftObject.moveUp = false;
        liftObject.canMove = true;
        liftObject.isMoving = true;

        if (canHit)
        {
            activateLever.Play();
        }

    }
}

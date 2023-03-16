using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject onSprite;
    [SerializeField] private GameObject offSprite;
    private bool canHit = true;
    [SerializeField] private GameObject door1Open;
    [SerializeField] private GameObject door1Closed;
    [SerializeField] private GameObject door2Open;
    [SerializeField] private GameObject door2Closed;
    [SerializeField] private GameObject door3Open;
    [SerializeField] private GameObject door3Closed;
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
    }

    public void trigger()
    {
        if (canHit)
        {
            canHit = false;
            woodHit.Play();
            activateLever.Play();
            door1Open.SetActive(true);
            door1Closed.SetActive(false);
            door2Open.SetActive(true);
            door2Closed.SetActive(false);
            door3Open.SetActive(true);
            door3Closed.SetActive(false);
        } else
        {
            woodHit.Play();
        }
    }
}

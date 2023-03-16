using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    public bool isTileable = true;


    private void Awake()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Start()
    {
        startpos = transform.position.x;
    }
    
    void Update()
    {
        cam = Camera.main.gameObject;
        float temp = (cam.transform.position.x * (1 - parallaxEffect));

        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (isTileable)
        {
            if (temp > startpos + length) startpos += length;
            else if (temp < startpos - length) startpos -= length;
        }
    }
}

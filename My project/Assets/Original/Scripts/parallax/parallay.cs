using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallay : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;
    public bool isTileable = true;


    private void Awake()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Start()
    {
        startpos = transform.position.y;
    }

    void Update()
    {
        cam = Camera.main.gameObject;
        float temp = (cam.transform.position.y * (1 - parallaxEffect));

        float dist = (cam.transform.position.y * parallaxEffect);

        transform.position = new Vector3(transform.position.x, startpos + dist, transform.position.z);

        if (isTileable)
        {
            if (temp > startpos + length) startpos += length;
            else if (temp < startpos - length) startpos -= length;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour
{
    void Update()
    {
        float verticalHeightSeen = Camera.main.orthographicSize * 2.0f;
        float verticalWidthSeen = verticalHeightSeen * Camera.main.aspect;
        transform.localScale = new Vector3(verticalWidthSeen, verticalHeightSeen, transform.localScale.z);
    }
}

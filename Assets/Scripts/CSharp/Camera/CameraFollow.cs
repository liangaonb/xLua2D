using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    public float smoothSpeed = 0.2f;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (target)
        {
            Vector3 targetPosition = new Vector3(target.position.x + offset.x,  target.position.y + offset.y, transform.position.z);
            //Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            transform.position = targetPosition;
        }
    }
}

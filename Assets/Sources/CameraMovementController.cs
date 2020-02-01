﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    public GameObject target; // player to follow
    public Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.transform.position;
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = targetPosition.x + cameraOffset.x;
        transform.position = cameraPosition;
    }
}

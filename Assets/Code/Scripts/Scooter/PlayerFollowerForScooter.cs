using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerFollowerForScooter : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if(target == null) return;
        
        Quaternion newRotation = target.rotation * Quaternion.Euler(0, -90, 0);
        transform.position = target.position;
        transform.rotation = newRotation;
    }
}
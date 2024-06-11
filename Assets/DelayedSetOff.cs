using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSetOff : MonoBehaviour
{
    private void Start()
    {
        Invoke("SetOff", 0.1f);
    }
    
    private void SetOff()
    {
        gameObject.SetActive(false);
    }
}

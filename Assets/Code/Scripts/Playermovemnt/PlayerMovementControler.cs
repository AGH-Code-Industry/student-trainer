using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovementControler : MonoBehaviour
{

    [Inject] private PlayerMovementService _service;
    private Rigidbody rb;

    private PlayerMovementData _data;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void FixedUpdate()
    {
        rb.velocity = _service.GetSpeed(transform.forward);
    }
}

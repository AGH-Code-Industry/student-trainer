    using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // rotation of character
        float vertical = Input.GetAxisRaw("Vertical"); // force of movement
        transform.Rotate(Vector3.up * horizontal);

        if(vertical != 0)
        {
            Vector3 moveDirection = transform.forward * vertical;
            rb.AddForce(moveDirection.normalized * speed, ForceMode.Force);
            animator.Play("Run");
        }
        else
        {
            animator.Play("Idle");
            rb.velocity = Vector3.zero;
        }


       
    }
}

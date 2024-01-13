using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PlayerArrowsMovement : MonoBehaviour
{
    enum PlayerAnimation { Idle, Run }

    
    NavMeshAgent agent;
    Animator animator;
    Vector3 destination;
    Rigidbody rb;

    [Header("Movement")] [SerializeField] private float speed;

    float lookRotationSpeed = 8f;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        
        
    }

    /*void AssignInputs()
    {
        input.Main.Move.performed += ctx => ArrowsMove();
    }*/

    void ArrowsMove()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Mathf.Abs(input.y) > 0.01f || Mathf.Abs(input.x) > 0.01f)
        {
            Move(input);
        }
        else
        {
            //FaceTarget();
            Rotate(input);
        }
        
        
    }

    void Move(Vector2 input)
    {
        Vector3 destination = transform.position + transform.right * input.x + transform.forward * input.y;
        agent.destination = destination;
    }

    void Rotate(Vector2 input)
    {
        agent.destination = transform.position;
        transform.Rotate(0, input.x * agent.angularSpeed*Time.deltaTime, 0);
    }

    void Update() 
    {
        ArrowsMove();
        /*if (destination != agent.destination)
        {
            destination = agent.destination;
            FaceTarget();
        }*/
        
        SetAnimations();
    }

    void FaceTarget()
    {
        Vector3 direction = (agent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
    }

    void SetAnimations()
    {
        if(rb.velocity == Vector3.zero)
            animator.Play(PlayerAnimation.Idle.ToString());
        else
            animator.Play(PlayerAnimation.Run.ToString());
    }
}

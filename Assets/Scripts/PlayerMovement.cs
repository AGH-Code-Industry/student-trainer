using Ink.Parsed;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using Ink.Runtime;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    enum PlayerAnimation { Idle, Run }

    CustomActions input;

    NavMeshAgent agent;
    Animator animator;
    Vector3 destination;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

    float lookRotationSpeed = 8f;
    
    void Awake() 
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        input = new CustomActions();
        AssignInputs();

    }

    private void Start() {
        Events.DialogTrigger += zdarzenie;
        
    }

    private void zdarzenie (Ink.Runtime.Story  dialogue)
	{
		Debug.Log("PlayerMovement");
        return;
	}


    void AssignInputs()
    {
        input.Main.Move.performed += ctx => ClickToMove();
    }

    void ClickToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers)) 
        {
            agent.destination = hit.point;
            if(clickEffect != null)
                Instantiate(clickEffect, hit.point + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation); 
        }
    }

    void OnEnable() => input.Enable();

    void OnDisable() => input.Disable();

    void Update() 
    {
        if (destination != agent.destination)
        {
            destination = agent.destination;
            FaceTarget();
        }
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
        if(agent.velocity == Vector3.zero)
            animator.Play(PlayerAnimation.Idle.ToString());
        else
            animator.Play(PlayerAnimation.Run.ToString());
    }
}

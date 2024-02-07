using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    enum PlayerAnimation { Idle, Run }

    NavMeshAgent agent;
    Animator animator;
    Vector3 destination;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

    float lookRotationSpeed = 8f;
    
    void Awake() 
    {
        InputManager.Instance.GetInput().Main.Move.performed += input => ClickToMove();
    }

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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

    // private void OnDestroy() 
    // {
    //     InputManager.Instance.GetInput().Main.Move.performed -= input => ClickToMove();
    // }
}

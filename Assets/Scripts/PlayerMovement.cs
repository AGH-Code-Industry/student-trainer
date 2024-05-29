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

    private bool _isRunning = false;


    void Awake()
    {
    }

    private void Start()
    {
        InputManager.Instance.GetInput().Main.Move.performed += input => ClickToMove();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void ClickToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers))
        {
            _isRunning = true;
            agent.destination = hit.point;

            FaceTarget();

            if (clickEffect != null)
                Instantiate(clickEffect, hit.point + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
        }
    }


    void Update()
    {
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
        if (_isRunning)
        {
            if (animator.GetBool("isRidingScooter") == false)
            {
                animator.Play(PlayerAnimation.Run.ToString());
            }
            if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
            {
                animator.Play(PlayerAnimation.Idle.ToString());
                _isRunning = false;
            }
        }
    }

    // private void OnDestroy() 
    // {
    //     InputManager.Instance.GetInput().Main.Move.performed -= input => ClickToMove();
    // }
}
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    enum PlayerAnimation { Idle, Run }
    private GameObject attackRange;
    private bool isAttacking = false;

    NavMeshAgent agent;
    Animator animator;
    Vector3 destination;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

    float lookRotationSpeed = 8f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        attackRange = GameObject.FindGameObjectWithTag("AttackRange");

        // Subscribe to input actions
        InputManager.Instance.GetInput().Main.Move.performed += input => ClickToMove();
        InputManager.Instance.GetInputMain2().RightClick.RightClick.performed += ctx => OnRightClick();
    }

    void ClickToMove()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers))
        {
            // Check if the hit object's tag is not "Player"
            if (hit.transform.tag != "Player")
            {
                agent.destination = hit.point;

                // Instantiate the click effect if it's not null
                if (clickEffect != null)
                    Instantiate(clickEffect, hit.point + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
            }
            /*else if (attackRange != null)
            {
                if (isAttacking == false)
                {
                    isAttacking = !isAttacking;
                    attackRange.SetActive(true);
                }
                else
                {
                    isAttacking = !isAttacking;
                    attackRange.SetActive(false);
                }
            }*/
        }
    }

    void OnRightClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f, clickableLayers))
        {
            if (hit.collider.CompareTag("Player"))
            {
                ToggleAttack();
            }
        }
    }

    void ToggleAttack()
    {
        if (attackRange != null)
        {
            isAttacking = !isAttacking;
            attackRange.SetActive(isAttacking);
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

    private void OnDestroy()
    {
        // Unsubscribe from input actions to avoid memory leaks
        /*InputManager.Instance.GetInput().Main.Move.performed -= input => ClickToMove();*/
        /*InputManager.Instance.GetInputMain2().RightClick.RightClick.performed -= ctx => OnRightClick();*/
        //Generuje błąd: Some objects were not cleaned up when closing the scene.
    }
}
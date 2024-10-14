using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Zenject;

[RequireComponent(typeof(CharacterDataHandler))]
public class PlayerMovement : MonoBehaviour
{
    enum PlayerAnimation { Idle, Run }
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

    [Inject] private BattleService _battleService;

    private CharacterData data;

    private void Awake()
    {
        _battleService.StartBattle += OnStartBattle;
    }

    private void Start()
    {
        if (clickEffect == null)
            Debug.LogError($"clickEffect is not set in {transform.name} Object");

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        data = GetComponent<CharacterDataHandler>().Data;

    }

    private void OnStartBattle(Vector3 position)
    {
        agent.ResetPath();
    }

    void OnMove()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers))
        {
            if (_battleService.IsBattleState)
            {
                var distance = Vector3.Distance(hit.point, transform.position);
                if (data.id == _battleService.actualBattleTurn.id && _battleService.TryUpdateMovePoints(distance))
                {
                    GoToPosition(hit.point);
                }
            }
            else
            {
                GoToPosition(hit.point);
            }

        }
    }

    private void GoToPosition(Vector3 position)
    {
        agent.destination = position;

        FaceTarget();

        Instantiate(clickEffect, position + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
        animator.Play(PlayerAnimation.Run.ToString());
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
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
        {
            animator.Play(PlayerAnimation.Idle.ToString());
        }
    }

    private void OnDestroy()
    {
        _battleService.StartBattle += OnStartBattle;
    }
}
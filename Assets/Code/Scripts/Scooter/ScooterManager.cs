using UnityEngine;
using UnityEngine.AI;

public class ScooterManager : MonoBehaviour
{
    public GameObject scooterPrefab;
    public Transform spawnPoint;
    NavMeshAgent agent;
    Animator animator;

    private GameObject currentScooterInstance;

    private void Start()
    {
        // Assuming the InputManager is accessible as a singleton
        // InputManager.Instance.onScooterSpawnRequested.AddListener(ToggleScooter);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void OnScooter()
    {
        animator.SetBool("isRidingScooter", !animator.GetBool("isRidingScooter"));

        if (currentScooterInstance != null)
        {
            Destroy(currentScooterInstance);
            currentScooterInstance = null;
            agent.speed = 8;
        }
        else
        {
            Quaternion newRotation = spawnPoint.rotation * Quaternion.Euler(0, 90, 0);

            // Instantiate the scooter with the new rotation
            currentScooterInstance = Instantiate(scooterPrefab, spawnPoint.position, newRotation, transform);
            agent.speed = 12;
        }
    }
}
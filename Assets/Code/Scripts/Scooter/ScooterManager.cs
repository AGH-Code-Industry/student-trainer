using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ScooterManager : MonoBehaviour
{
    public GameObject scooterPrefab;
    public Transform spawnPoint;
    NavMeshAgent agent;

    private GameObject currentScooterInstance;

    private void Start()
    {
        // Assuming the InputManager is accessible as a singleton
        InputManager.Instance.onScooterSpawnRequested.AddListener(ToggleScooter);
        agent = GetComponent<NavMeshAgent>();
    }
    
    private void ToggleScooter()
    {
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
            currentScooterInstance = Instantiate(scooterPrefab, spawnPoint.position, newRotation);
            agent.speed = 12;
        }
    }
}
using UnityEngine;
using Zenject;

public class Area : MonoBehaviour
{
    [SerializeField] string areaID;

    [Inject] readonly EventBus eventBus;

    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.CompareTag("Player");

        // Only call event if the player has entered the area
        if (isPlayer)
            eventBus.Publish(new AreaEnteredEvent(areaID));
    }
}

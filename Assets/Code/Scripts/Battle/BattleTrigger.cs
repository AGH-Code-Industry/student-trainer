using UnityEngine;
using Zenject;

public class BattleTrigger : MonoBehaviour
{
    [Inject] private BattleService _battleService;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _battleService.Start(transform.position);
        }
    }
}
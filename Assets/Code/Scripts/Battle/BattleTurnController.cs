using System.Threading.Tasks;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterDataHandler))]
public class BattleTurnController : MonoBehaviour
{
    [Inject] private BattleService _battleService;

    private CharacterData _data;

    private void Awake()
    {
        _battleService.StartBattle += OnStartBattle;
    }

    private void Start()
    {
        _data = GetComponent<CharacterDataHandler>().Data;
    }

    private void OnStartBattle(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        if (distance <= _battleService.DISTANCE_TO_TAKE_FIGHT)
        {
            _battleService.AddParticipant(new BattleTurn(_data));
        }
    }

    private void OnDestroy()
    {
        _battleService.StartBattle -= OnStartBattle;
    }
}
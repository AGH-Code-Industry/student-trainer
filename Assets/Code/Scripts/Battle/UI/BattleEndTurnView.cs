using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Button))]
public class BattleEndTurnView : MonoBehaviour
{
    [Inject] private BattleService _battleService;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _battleService.EndTurn();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClick);
    }
}
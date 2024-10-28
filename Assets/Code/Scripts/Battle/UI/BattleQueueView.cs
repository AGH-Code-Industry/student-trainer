using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BattleQueueView : MonoBehaviour
{
    [SerializeField] private List<BattleTurnView> _battleTurns;

    private Image _image;

    [Inject] private BattleService _battleService;

    private void Awake()
    {
        _battleService.CreateBattleQueue += OnCreateBattleQueue;
        _battleService.StartBattle += OnStartBattle;
        _battleService.EndBattle += OnEndBattle;
    }

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.enabled = false;

        _battleTurns.ForEach(battleTurn => battleTurn.gameObject.SetActive(false));
    }

    private void OnCreateBattleQueue()
    {
        var queueList = _battleService.BattleQueue.ToList();
        var count = Math.Min(_battleTurns.Count, queueList.Count);
        for (int i = 0; i < count; i++)
        {
            _battleTurns[i].gameObject.SetActive(true);
            _battleTurns[i].SetTurnData(queueList[i]);
        }
    }

    private void OnStartBattle(Vector3 position)
    {
        _image.enabled = true;
    }

    private void OnEndBattle()
    {
        _image.enabled = false;
        _battleTurns.ForEach(battleTurn => battleTurn.gameObject.SetActive(false));
    }

    private void OnDestroy()
    {
        _battleService.CreateBattleQueue -= OnCreateBattleQueue;
        _battleService.StartBattle -= OnStartBattle;
        _battleService.EndBattle -= OnEndBattle;
    }

}
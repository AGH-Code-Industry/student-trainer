using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BattleService : IInitializable
{
    public readonly float DISTANCE_TO_TAKE_FIGHT = 10;
    public readonly float MAX_MOVE_POINTS = 10;
    public readonly int MIN_REQUIRED_PARTICIPANTS_TO_START_BATTLE = 2;
    public readonly int MAX_TURNS_IN_QUEUE = 6;

    public event Action CreateBattleQueue;
    public event Action<Vector3> StartBattle;
    public event Action EndBattle;
    public event Action<BattleTurn> StartTurn;

    public List<BattleTurn> BattleParticipants { get; private set; }
    public Queue<BattleTurn> BattleQueue { get; private set; }

    public bool IsBattleState => _isBattleState;
    private bool _isBattleState = false;

    private int _lastCharacterInQueue;
    public BattleTurn actualBattleTurn => _actualBattleTurn;
    private BattleTurn _actualBattleTurn;

    public void Initialize()
    {
        BattleParticipants = new List<BattleTurn>();
        BattleQueue = new Queue<BattleTurn>();
    }

    public void Start(Vector3 position)
    {
        _isBattleState = true;
        StartBattle?.Invoke(position);
    }

    public void End()
    {
        BattleParticipants.Clear();
        _isBattleState = false;
        EndBattle?.Invoke();
    }

    public bool TryUpdateMovePoints(float distance)
    {
        var movePoints = distance + _actualBattleTurn.usedMovePoints;
        if (movePoints < MAX_MOVE_POINTS)
        {
            _actualBattleTurn.usedMovePoints = movePoints;
            return true;
        }
        return false;
    }


    public void AddParticipant(BattleTurn turn)
    {
        if (IsBattleState)
        {
            BattleParticipants.Add(turn);
            if (BattleParticipants.Count >= MIN_REQUIRED_PARTICIPANTS_TO_START_BATTLE)
            {
                CreateQueue();
            }
        }
    }

    public void EndTurn()
    {
        if (IsBattleState)
        {
            UpdateQueue();
        }
    }

    private void UpdateQueue()
    {
        FillQueue();

        _actualBattleTurn = BattleQueue.Dequeue();
        CreateBattleQueue?.Invoke();
        StartTurn?.Invoke(_actualBattleTurn);
    }

    private void CreateQueue()
    {
        BattleQueue.Clear();
        _lastCharacterInQueue = 0;

        FillQueue();

        _actualBattleTurn = BattleQueue.Dequeue();
        CreateBattleQueue?.Invoke();
        StartTurn?.Invoke(_actualBattleTurn);
    }

    private void FillQueue()
    {
        for (int i = BattleQueue.Count; i <= MAX_TURNS_IN_QUEUE; i++)
        {
            _lastCharacterInQueue = (_lastCharacterInQueue + 1) % BattleParticipants.Count;
            var turn = BattleParticipants[_lastCharacterInQueue];
            BattleQueue.Enqueue(turn);
        }
    }
}
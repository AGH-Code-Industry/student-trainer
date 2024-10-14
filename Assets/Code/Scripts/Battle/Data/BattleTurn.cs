using System;
using UnityEngine;

public struct BattleTurn
{
    public Guid id;
    public Sprite sprite;
    public float usedMovePoints;

    public BattleTurn(CharacterData data)
    {
        id = data.id;
        sprite = data.sprite;
        usedMovePoints = 0;
    }
}
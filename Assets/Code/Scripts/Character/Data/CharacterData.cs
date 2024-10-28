using System;
using UnityEngine;

public struct CharacterData
{
    public Guid id;
    public string name;
    public Sprite sprite;

    public CharacterData(CharacterSettings settings)
    {
        id = Guid.NewGuid();
        name = settings.name;
        sprite = settings.sprite;
    }
}
using UnityEngine;

public abstract class StatusEffectSO : ScriptableObject
{
    public string effectName;
    public float duration;
    public abstract void ApplyEffect(Character target);
    public abstract void RemoveEffect(Character target);
}
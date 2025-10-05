using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public CharacterStatsSO Stats => stats;
    [SerializeField] protected CharacterStatsSO stats;
    public CharacterActionState ActionState { get; private set; }
    public abstract void ApplyEffects(StatusEffectSO effect);
}
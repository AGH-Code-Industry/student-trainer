using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSO", menuName = "AttackSO", order = 0)]
public class AttackSO : ScriptableObject
{
    public AnimationClip clip;
    public float damage;
    public float knockback;
    public bool canBeBlocked;
    public bool canBeParried;
    public List<StatusEffectSO> appliedEffects;
}
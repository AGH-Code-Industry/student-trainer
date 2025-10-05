using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "WeaponSO", order = 0)]
public class WeaponSO : ItemSo
{
    public int baseDamage;
    public float baseKnockback;
    public float staminaCost;
    public List<AttackSetSO> attacks;
}
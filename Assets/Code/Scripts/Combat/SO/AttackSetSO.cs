using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackSetSO", menuName = "AttackSetSO", order = 0)]
public class AttackSetSO : ScriptableObject
{
    public string binding;
    public List<AttackSO> attacks;
}
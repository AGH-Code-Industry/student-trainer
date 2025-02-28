using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboList", menuName = "Settings/Combo List")]
public class ComboList : ScriptableObject
{
    public ComboSystem.Combo[] combos;

    [Tooltip("How quickly the player needs to press the LMB after the current attack ends to continue the combo.")]
    public float comboTime = 0.5f;
    [Tooltip("Additional time after an attack ends until the player can move again.")]
    public float recovery = 0.3f;
}

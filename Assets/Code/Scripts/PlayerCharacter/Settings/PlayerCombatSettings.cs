using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombat", menuName = "Settings/Player Combat Settings")]
public class PlayerCombatSettings : ScriptableObject
{
    // Todo: Add a list of combos with editable variables for the designers

    public Combo[] combos;

    [Tooltip("How quickly the player needs to press the LMB after the current attack ends to continue the combo.")]
    public float comboTime = 0.5f;
    [Tooltip("Additional time after an attack ends until the player can move again.")]
    public float recovery = 0.3f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

    [CreateAssetMenu(fileName = "ComboList", menuName = "Settings/Combo List")]
    public class ComboList : ScriptableObject
    {
        public Combo[] combos;

        [Tooltip("How quickly another attack needs to be started to maintain the combo.")]
        public float comboTime = 0.5f;
        [Tooltip("Additional time after an attack animation ends, that dictates when the actual attack ends.")]
        public float recovery = 0.3f;

        [Tooltip("Dictates the chances a combo has for being picked (applicable only to enemies)")]
        public ComboWeight[] weights;
    }

    [System.Serializable]
    public struct ComboWeight
    {
        public string comboName;
        public float weight;
    }

}
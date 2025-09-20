using System.Collections.Generic;
using UnityEngine;

namespace Combat
{

    public class CombatMove : ScriptableObject
    {
        public float speedMultiplier;
        public AnimationClip mainAnimation;
        [SerializeReference] public List<HitEffect> hitEffects;
    }

}
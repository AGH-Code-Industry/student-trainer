using UnityEngine;

namespace Combat
{

    public class CombatMove : ScriptableObject
    {
        public float speedMultiplier;
        public AnimationClip mainAnimation;
        public HitEffect[] hitEffects;
    }

}
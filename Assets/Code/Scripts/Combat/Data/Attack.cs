using UnityEngine;

namespace Combat
{
    [System.Serializable]
    public class Attack
    {
        public string animationName;
        public float speedMulti = 1f;

        public float duration;
        public float hitDelay;

        public DamageInfo damage;
        public float range;
        public AnimationCurve mobility;

        public float parryStart;
        public float parryEnd;
        
        public enum CanBeBlocked
        {
            Always,
            OnlyPerfect,
            IgnoreBlocks
        }

        public CanBeBlocked blockedBehavior;
        
        public Attack()
        {

        }

        public float GetDuration() => duration / speedMulti;
        public float GetHitDelay() => hitDelay / speedMulti;
        public float GetParryStart() => parryStart / speedMulti;
        public float GetParryEnd() => parryEnd / speedMulti;
    }

    [System.Serializable]
    public struct DamageInfo
    {
        public float amount;
        public float armorPierce;
        public float blockPierce;
        public float stagger;
        public float knockback;
        // damage type, armor pierceing, etc.s
    }

}
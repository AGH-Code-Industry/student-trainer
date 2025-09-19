using UnityEngine;

namespace Combat
{
    [CreateAssetMenu(fileName="New Attack", menuName="Settings/Combat/Attack")]
    public class Attack : CombatMove
    {
        public FrameData frameData;
    }

}
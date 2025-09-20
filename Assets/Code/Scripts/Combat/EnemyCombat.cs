using Combat.Interfaces;
using ModestTree;
using UnityEngine;

namespace Combat
{
    public class EnemyCombat : MonoBehaviour, ICombatant
    {
        public void ApllyEffect()
        {
            throw new System.NotImplementedException();
        }

        public void Attack()
        {
            Debug.Log("Enemy - Atakuję");
        }

        public void TakeDamage(DamageData damage)
        {
            Debug.Log("Enemy - Dostałem");
        }
    }
}
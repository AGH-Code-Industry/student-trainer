using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Combat
{
    /// <summary>
    /// Handles combat logic, like performing attacks, dodging, and blocking.
    /// Doesn't handle animations and movement/action pausing, these need to be handled externally.
    /// This class is meant to be included as a "helper" in another class (preferrablty a MonoBehavior).
    /// </summary>
    public class CombatSystem
    {
        // A point below which mobility will be counted as being 0
        const float MOBILITY_THRESHOLD = 0.1f;

        MonoBehaviour mediator;
        Transform attackOrigin;

        Attack currentAttack = null;
        Coroutine currentCoroutine = null;
        float recoveryTime = 0f;
        bool recoveryPhase = false;

        float attackStartPoint = 0f;

        // Passes the current attack class as an argument
        public Action<Attack> onAttackStart;
        public Action<AttackResult> onAttackPerformed;
        public Action onAttackEnd;
        // Invoked when recovery time is up
        public Action onRecoveryEnd;

        public float GetMobility()
        {
            if (currentAttack == null || attackStartPoint == 0f)
                return 1f;

            float timeNormalized = (Time.time - attackStartPoint) / currentAttack.GetDuration();
            float mob = currentAttack.mobility.Evaluate(timeNormalized);
            return mob < MOBILITY_THRESHOLD ? 0f : mob;
        }

        public void PerformAttack(Attack attack)
        {
            // An attack is in progress
            if (currentAttack != null || attackStartPoint != 0f)
            {
                // The attack is in the recovery phase, so another attack can already be performed, but the player (or enemy) can't move
                if(recoveryPhase)
                {
                    recoveryPhase = false;
                    mediator.StopCoroutine(currentCoroutine);
                }
                else
                {
                    return;
                }
            }

            currentAttack = attack;
            attackStartPoint = Time.time;
            currentCoroutine = mediator.StartCoroutine(AttackCoroutine());
        }

        IEnumerator AttackCoroutine()
        {
            onAttackStart?.Invoke(currentAttack);

            yield return new WaitForSeconds(currentAttack.GetHitDelay());

            onAttackPerformed?.Invoke(AttackLogic());

            yield return new WaitForSeconds(currentAttack.GetDuration() - currentAttack.GetHitDelay());

            onAttackEnd?.Invoke();

            recoveryPhase = true;
            yield return new WaitForSeconds(recoveryTime);
            recoveryPhase = false;

            currentAttack = null;
            currentCoroutine = null;
            attackStartPoint = 0f;

            onRecoveryEnd?.Invoke();
        }

        AttackResult AttackLogic()
        {
            if (currentAttack == null)
                return AttackResult.Miss;

            IHitResult resultComponent;
            IDamageable damageComponent;

            // If the attackOrigin object is inside an enemy, this will ensure that it still deals damage
            Collider[] overlappingColliders = Physics.OverlapSphere(attackOrigin.position, 0.25f);
            foreach (Collider col in overlappingColliders)
            {
                // Avoid dealing damage to self
                if (col.transform.root == mediator.transform.root)
                    continue;

                resultComponent = col.transform.root.GetComponent<IHitResult>();
                damageComponent = col.transform.root.GetComponent<IDamageable>();
                if(resultComponent != null)
                {
                    AttackResult res = resultComponent.GetAttackResult(currentAttack.damage);
                    if(res == AttackResult.Hit || res == AttackResult.Blocked)
                    {
                        if (damageComponent != null)
                            damageComponent.TakeDamage(currentAttack.damage);
                    }
                    return res;
                }
                else
                {
                    // If no result component is available, treat the object as being able to be damaged
                    if (damageComponent != null)
                    {
                        damageComponent.TakeDamage(currentAttack.damage);
                        return AttackResult.Hit;
                    }
                    else
                    {
                        return AttackResult.Miss;
                    }
                }
            }

            RaycastHit hit;
            Ray ray = new Ray(attackOrigin.position, attackOrigin.forward);
            bool inRange = Physics.Raycast(ray, out hit, currentAttack.range);
            if (!inRange)
                return AttackResult.Miss;

            resultComponent = hit.transform.root.GetComponent<IHitResult>();
            damageComponent = hit.transform.root.GetComponent<IDamageable>();
            if (resultComponent != null)
            {
                AttackResult res = resultComponent.GetAttackResult(currentAttack.damage);
                if (res == AttackResult.Hit || res == AttackResult.Blocked)
                {
                    if (damageComponent != null)
                        damageComponent.TakeDamage(currentAttack.damage);
                }
                return res;
            }
            else
            {
                // If no result component is available, treat the object as being able to be damaged
                if (damageComponent != null)
                {
                    damageComponent.TakeDamage(currentAttack.damage);
                    return AttackResult.Hit;
                }
                else
                {
                    return AttackResult.Miss;
                }
            }
        }
    }

    public enum AttackResult
    {
        Miss,
        Hit,
        Blocked,
        Parried
    }
}

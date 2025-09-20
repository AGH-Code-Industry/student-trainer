using System.Collections.Generic;
using Combat.Interfaces;
using Combat.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Combat
{
    public class PlayerCombat : MonoBehaviour, ICombatant, IInputConsumer
    {

        [SerializeField] private CombatMove combatMove;
        [Inject] private InputService _input;
        [Inject] private CombatService _combat;
        private Animator _animator;

        public int priority => 1;

        void Start()
        {
            _animator = GetComponent<Animator>();
            _input.RegisterConsumer(this, new List<string>() { "MouseClick" }, false);
        }

        public void ApllyEffect()
        {
            throw new System.NotImplementedException();
        }

        public void Attack()
        {
            throw new System.NotImplementedException();
        }

        public bool ConsumeInput(InputAction.CallbackContext context)
        {
            InputHelper.MouseClickData click = new InputHelper.MouseClickData(context);
            if (click.button == InputHelper.MouseClickData.MouseButton.Left)
            {
                _animator.Play("Punch_Left");
            }

            return false;
        }

        public void TakeDamage(DamageData damage)
        {
            throw new System.NotImplementedException();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                _combat.ResolveAttack(new Data.AttackRequest()
                {
                    attacker = this,
                    targets = new List<IDamagable>() { other.GetComponent<IDamagable>() },
                    hitEffects = combatMove.hitEffects
                });
            }
        }
    }
}
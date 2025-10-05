using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Fists : MonoBehaviour, IUsable
{
    [SerializeField] private WeaponSO weapon;
    [Inject] private readonly CombatService combatService;

    private Character user = null;
    public IEnumerable<string> GetBindings()
    {
        throw new System.NotImplementedException();
    }

    public void Use(Character user, string binding)
    {
        this.user = user;

        var animator = user.GetComponent<Animator>();
        animator.Play("Punch_Left");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damagable))
        {
            combatService.ResolveAttack(user, damagable, weapon.attacks.First().attacks.First(), weapon);
        }
    }

}
using UnityEngine;

public interface ICombatant
{
    bool IsAttacking {get;}
    bool IsBlocking {get;}
    float CurrentHP {get;}
    float MaxHP {get;}

    bool CanLightAttack();
    void LightAttack(Transform target);
    bool CanHeavyAttack();
    void HeavyAttack();
    bool CanBlock();
    void Block();
    bool CanDodge();
    void Dodge();
}
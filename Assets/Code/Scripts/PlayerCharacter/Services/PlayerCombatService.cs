using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerCombatService : IInitializable
{
    int currentStep = 0;
    bool attackInProgress = false;
    bool bufferAttack = false;
    float lastTime = 0f;

    Combo currentCombo;
    Combo[] availableCombos;

    PlayerAnimationController _animationController;
    PlayerCombatInstance _combatInstance;

    [Inject] readonly PlayerMovementService _movement;
    [Inject] readonly ResourceReader _reader;
    PlayerCombatSettings _settings;

    public void Initialize()
    {
        currentStep = 0;
        attackInProgress = false;
        bufferAttack = false;
        lastTime = 0f;
        _settings = _reader.ReadSettings<PlayerCombatSettings>();

        /*ComboPart u1 = new ComboPart("CharacterArmature|Punch_Right", 25f);
        ComboPart u1 = new ComboPart("Punch_Right", 0.8f, 0.43f, 10, 1f);
        ComboPart u2 = new ComboPart("Punch_Left", 0.8f, 0.43f, 10, 1f);
        ComboPart u3 = new ComboPart("Kick_Right", 0.9f, 0.4f, 25, 1.5f);
        currentCombo = new List<ComboPart> { u1, u2, u3 };*/

        availableCombos = _settings.combos;

        ChangeCombo("unarmed");
    }

    public void ChangeCombo(string newComboName)
    {
        foreach(Combo c in availableCombos)
        {
            if(c.name == newComboName)
            {
                // Change combo and reset all timing parameters
                currentCombo = c;
                currentStep = 0;
                attackInProgress = false;
                bufferAttack = false;
                lastTime = 0f;
            }
        }
    }

    public void Attack()
    {
        if (attackInProgress)
        {
            bufferAttack = true;
            return;
        }

        if (_combatInstance == null)
            _combatInstance = Object.FindObjectOfType<PlayerCombatInstance>();
        if(_animationController == null)
            _animationController = Object.FindObjectOfType<PlayerAnimationController>();

        // How much time has passed since the last attack was initiated
        float diff = Time.time - lastTime;
        float lastAttackDuration = currentCombo.parts[currentStep].duration;

        if(diff <= lastAttackDuration + _settings.comboTime)
        {
            // Valid timing, continue combo
            currentStep++;
            // If the combo reaches the end, start anew
            if(currentStep == currentCombo.parts.Length)
            {
                currentStep = 0;
            }
        }
        else
        {
            // Invalid timing, start the combo anew
            currentStep = 0;
        }

        // Remove the workaround!
        // Use the combat instance to start the coroutine, as the StartCoroutine function needs to be executed on a MonoBehavior class
        _combatInstance.StopAllCoroutines();
        _combatInstance.StartCoroutine(PerformAttack());
    }

    IEnumerator PerformAttack()
    {
        ComboPart part = currentCombo.parts[currentStep];

        _movement.Freeze();
        _animationController.PlayAnimation(part.animationName);
        attackInProgress = true;
        lastTime = Time.time;

        yield return new WaitForSeconds(part.hitMoment);

        float toEnd = part.duration - part.hitMoment;

        // Damage logic
        _combatInstance.Attack(part.damage, part.range);

        yield return new WaitForSeconds(toEnd);

        attackInProgress = false;
        // Perform a buffered attack if exists
        if (bufferAttack)
        {
            Attack();
            bufferAttack = false;
            yield break;
        }

        yield return new WaitForSeconds(_settings.recovery);

        _movement.Unfreeze();
    }
}

[System.Serializable]
public struct Combo
{
    public string name;
    public ComboPart[] parts;

    public Combo(string _name, ComboPart[] _parts)
    {
        name = _name;
        parts = _parts;
    }
}

[System.Serializable]
public struct ComboPart
{
    public string animationName;
    public float duration;
    public float hitMoment;
    public float damage;
    public float range;

    public ComboPart(string _animName, float _duration, float _hitMoment, float _damage, float _range)
    {
        animationName = _animName;
        duration = _duration;
        hitMoment = _hitMoment;
        damage = _damage;
        range = _range;
    }
}
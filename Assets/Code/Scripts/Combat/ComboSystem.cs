using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComboSystem
{
    int currentStep = 0;
    bool attackInProgress = false;
    bool bufferAttack = false;
    float lastTime = 0f;

    Combo currentCombo;
    Combo[] availableCombos;

    public ComboList comboList { get; private set; }

    // Mediator script
    MonoBehaviour mediator;
    Coroutine lastCoroutine = null;

    // Passes attack animation's name as argument
    public Action<string> onAttackStart;
    // Invoked when the attack's damaging action should take place, passes damage and range as arguments
    public Action<float, float> onAttackPerformed;
    // Invoked after attack animation ends. Passes true as an argument if this attack was the end of a combo
    public Action<bool> onAttackEnd;
    // Invoked when recovery time is up
    public Action onRecoveryEnd;

    public ComboSystem(ComboList newList, string defaultCombo, MonoBehaviour mediatorClass)
    {
        currentStep = 0;
        attackInProgress = false;
        bufferAttack = false;
        lastTime = 0f;

        comboList = newList;
        availableCombos = comboList.combos;
        ChangeCombo(defaultCombo);

        mediator = mediatorClass;
    }

    public float GetMinAttackRange()
    {
        float minRange = availableCombos[0].parts[0].range;

        foreach(Combo combo in availableCombos)
        {
            foreach(ComboPart part in combo.parts)
            {
                minRange = Math.Min(minRange, part.range);
            }
        }

        return minRange;
    }

    public void ChangeCombo(string newComboName)
    {
        foreach (Combo c in availableCombos)
        {
            if (c.name == newComboName)
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

    public bool HasCombo(string comboName)
    {
        foreach(Combo c in availableCombos)
        {
            if (c.name == comboName)
                return true;
        }

        return false;
    }

    public void Attack()
    {
        if (attackInProgress)
        {
            bufferAttack = true;
            return;
        }

        // How much time has passed since the last attack was initiated
        float diff = Time.time - lastTime;
        float lastAttackDuration = currentCombo.parts[currentStep].duration;

        if (diff <= lastAttackDuration + comboList.comboTime)
        {
            // Valid timing, continue combo
            currentStep++;
            // If the combo reaches the end, start anew
            if (currentStep == currentCombo.parts.Length)
            {
                currentStep = 0;
            }
        }
        else
        {
            // Invalid timing, start the combo anew
            currentStep = 0;
        }

        // Use the mediator MonoBehavior to start a coroutine
        if (lastCoroutine != null)
            mediator.StopCoroutine(lastCoroutine);
        lastCoroutine = mediator.StartCoroutine(PerformAttack());
    }

    IEnumerator PerformAttack()
    {
        ComboPart part = currentCombo.parts[currentStep];

        onAttackStart?.Invoke(part.animationName);
        attackInProgress = true;
        lastTime = Time.time;

        yield return new WaitForSeconds(part.hitMoment);

        float toEnd = part.duration - part.hitMoment;

        onAttackPerformed?.Invoke(part.damage, part.range);

        yield return new WaitForSeconds(toEnd);

        attackInProgress = false;
        bool comboEnd = currentStep == currentCombo.parts.Length - 1;
        onAttackEnd?.Invoke(comboEnd);
        // Perform a buffered attack if exists
        if (bufferAttack)
        {
            Attack();
            bufferAttack = false;
            yield break;
        }

        yield return new WaitForSeconds(comboList.recovery);

        onRecoveryEnd?.Invoke();
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
}

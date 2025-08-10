using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Combat
{

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
            float minRange = availableCombos[0].attacks[0].range;

            foreach (Combo combo in availableCombos)
            {
                foreach (Attack attack in combo.attacks)
                {
                    minRange = Math.Min(minRange, attack.range);
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
            foreach (Combo c in availableCombos)
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
            float lastAttackDuration = currentCombo.attacks[currentStep].GetDuration();

            if (diff <= lastAttackDuration + comboList.comboTime)
            {
                // Valid timing, continue combo
                currentStep++;
                // If the combo reaches the end, start anew
                if (currentStep == currentCombo.attacks.Length)
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
        }
    }

}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public Animator animator;



    public void PerformSkill(string skillName)
    {
        switch (skillName)
        {
            case "Death":
                Death();
                break;
            case "Gun_Shoot":
                Gun_Shoot();
                break;
            case "HitRecive":
                HitRecive();
                break;
            case "HitRecieve_2":
                HitRecieve_2();
                break;
            case "Idle_0":
                Idle_0();
                break;
            case "Idle_Gun":
                Idle_Gun();
                break;
            case "Idle_Gun_Pointing":
                Idle_Gun_Pointing();
                break;
            case "Idle_Gun_Shoot":
                Idle_Gun_Shoot();
                break;
            case "Idle_Neutral":
                Idle_Neutral();
                break;
            case "Idle_Sword":
                Idle_Sword();
                break;
            case "Interact":
                Interact();
                break;
            case "Kick_Left":
                Kick_Left();
                break;
            case "Kick_Right":
                Kick_Right();
                break;
            case "Punch_Left":
                Punch_Left();
                break;
            case "Punch_Right":
                Punch_Right();
                break;
            case "Roll":
                Roll();
                break;
            case "Run_0":
                Run_0();
                break;
            case "Run_Back":
                Run_Back();
                break;
            case "Run_Left":
                Run_Left();
                break;
            case "Run_Right":
                Run_Right();
                break;
            case "Run_Shoot":
                Run_Shoot();
                break;
            case "Sword_Slash":
                Sword_Slash();
                break;
            case "Walk":
                Walk();
                break;
            case "Wave":
                Wave();
                break;
        }
    }

    void Death()
    {
        animator.Play(SkillsEnum.Skills.Death.ToString());
    }

    void Gun_Shoot()
    {
        animator.Play(SkillsEnum.Skills.Gun_Shoot.ToString());
    }

    void HitRecive()
    {
        animator.Play(SkillsEnum.Skills.HitRecive.ToString());
    }

    void HitRecieve_2()
    {
        animator.Play(SkillsEnum.Skills.HitRecieve_2.ToString());
    }

    void Idle_0()
    {
        animator.Play(SkillsEnum.Skills.Idle_0.ToString());
    }

    void Idle_Gun()
    {
        animator.Play(SkillsEnum.Skills.Idle_Gun.ToString());
    }

    void Idle_Gun_Pointing()
    {
        animator.Play(SkillsEnum.Skills.Idle_Gun_Pointing.ToString());
    }
    
    void Idle_Gun_Shoot()
    {
        animator.Play(SkillsEnum.Skills.Idle_Gun_Shoot.ToString());
    }

    void Idle_Neutral()
    {
        animator.Play(SkillsEnum.Skills.Idle_Neutral.ToString());
    }

    void Idle_Sword()
    {
        animator.Play(SkillsEnum.Skills.Idle_Sword.ToString());
    }

    void Interact()
    {
        animator.Play(SkillsEnum.Skills.Interact.ToString());
    }

    void Kick_Left()
    {
        animator.Play(SkillsEnum.Skills.Kick_Left.ToString());
    }

    void Kick_Right()
    {
        animator.Play(SkillsEnum.Skills.Kick_Right.ToString());
    }

    void Punch_Left()
    {
        animator.Play(SkillsEnum.Skills.Punch_Left.ToString());
    }

    void Punch_Right()
    {
        animator.Play(SkillsEnum.Skills.Punch_Right.ToString());
    }

    void Roll()
    {
        animator.Play(SkillsEnum.Skills.Roll.ToString());
    }

    void Run_0()
    {
        animator.Play(SkillsEnum.Skills.Run_0.ToString());
    }

    void Run_Back()
    {
        animator.Play(SkillsEnum.Skills.Run_Back.ToString());
    }

    void Run_Left()
    {
        animator.Play(SkillsEnum.Skills.Run_Left.ToString());
    }

    void Run_Right()
    {
        animator.Play(SkillsEnum.Skills.Run_Right.ToString());
    }

    void Run_Shoot()
    {
        animator.Play(SkillsEnum.Skills.Run_Shoot.ToString());
    }

    void Sword_Slash()
    {
        animator.Play(SkillsEnum.Skills.Sword_Slash.ToString());
    }

    void Walk()
    {
        animator.Play(SkillsEnum.Skills.Walk.ToString());
    }

    void Wave()
    {
        animator.Play(SkillsEnum.Skills.Wave.ToString());
    }
}

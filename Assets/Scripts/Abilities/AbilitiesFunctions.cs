using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public static Animator animator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.Play("Punch_Left");
        }
    }
    public static void PerformSkill(string skillName)
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

    static void Death()
    {
        animator.Play(Skill.Skillname.Death.ToString());
    }

    static void Gun_Shoot()
    {
        animator.Play(Skill.Skillname.Gun_Shoot.ToString());
    }

    static void HitRecive()
    {
        animator.Play(Skill.Skillname.HitRecive.ToString());
    }

    static void HitRecieve_2()
    {
        animator.Play(Skill.Skillname.HitRecieve_2.ToString());
    }

    static void Idle_0()
    {
        animator.Play(Skill.Skillname.Idle_0.ToString());
    }

    static void Idle_Gun()
    {
        animator.Play(Skill.Skillname.Idle_Gun.ToString());
    }

    static void Idle_Gun_Pointing()
    {
        animator.Play(Skill.Skillname.Idle_Gun_Pointing.ToString());
    }

    static void Idle_Gun_Shoot()
    {
        animator.Play(Skill.Skillname.Idle_Gun_Shoot.ToString());
    }

    static void Idle_Neutral()
    {
        animator.Play(Skill.Skillname.Idle_Neutral.ToString());
    }

    static void Idle_Sword()
    {
        animator.Play(Skill.Skillname.Idle_Sword.ToString());
    }

    static void Interact()
    {
        animator.Play(Skill.Skillname.Interact.ToString());
    }

    static void Kick_Left()
    {
        animator.Play(Skill.Skillname.Kick_Left.ToString());
    }

    static void Kick_Right()
    {
        animator.Play(Skill.Skillname.Kick_Right.ToString());
    }

    static void Punch_Left()
    {
        animator.Play(Skill.Skillname.Punch_Left.ToString());
    }

    static void Punch_Right()
    {
        animator.Play(Skill.Skillname.Punch_Right.ToString());
    }

    static void Roll()
    {
        animator.Play(Skill.Skillname.Roll.ToString());
    }

    static void Run_0()
    {
        animator.Play(Skill.Skillname.Run_0.ToString());
    }

    static void Run_Back()
    {
        animator.Play(Skill.Skillname.Run_Back.ToString());
    }

    static void Run_Left()
    {
        animator.Play(Skill.Skillname.Run_Left.ToString());
    }

    static void Run_Right()
    {
        animator.Play(Skill.Skillname.Run_Right.ToString());
    }

    static void Run_Shoot()
    {
        animator.Play(Skill.Skillname.Run_Shoot.ToString());
    }

    static void Sword_Slash()
    {
        animator.Play(Skill.Skillname.Sword_Slash.ToString());
    }

    static void Walk()
    {
        animator.Play(Skill.Skillname.Walk.ToString());
    }

    static void Wave()
    {
        animator.Play(Skill.Skillname.Wave.ToString());
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public static Abilities Instance;

    public Animator animator;

    private void Awake()
    {
        Instance = this;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.Play("Punch_Left");
        }
    }
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
        animator.Play(Skill.Skillname.Death.ToString());
    }

    void Gun_Shoot()
    {
        animator.Play(Skill.Skillname.Gun_Shoot.ToString());
    }

    void HitRecive()
    {
        animator.Play(Skill.Skillname.HitRecive.ToString());
    }

    void HitRecieve_2()
    {
        animator.Play(Skill.Skillname.HitRecieve_2.ToString());
    }

    void Idle_0()
    {
        animator.Play(Skill.Skillname.Idle_0.ToString());
    }

    void Idle_Gun()
    {
        animator.Play(Skill.Skillname.Idle_Gun.ToString());
    }

    void Idle_Gun_Pointing()
    {
        animator.Play(Skill.Skillname.Idle_Gun_Pointing.ToString());
    }

    void Idle_Gun_Shoot()
    {
        animator.Play(Skill.Skillname.Idle_Gun_Shoot.ToString());
    }

    void Idle_Neutral()
    {
        animator.Play(Skill.Skillname.Idle_Neutral.ToString());
    }

    void Idle_Sword()
    {
        animator.Play(Skill.Skillname.Idle_Sword.ToString());
    }

    void Interact()
    {
        animator.Play(Skill.Skillname.Interact.ToString());
    }

    void Kick_Left()
    {
        animator.Play(Skill.Skillname.Kick_Left.ToString());
    }

    void Kick_Right()
    {
        animator.Play(Skill.Skillname.Kick_Right.ToString());
    }

    void Punch_Left()
    {
        animator.Play(Skill.Skillname.Punch_Left.ToString());
        Debug.Log(Skill.Skillname.Punch_Left.ToString());
    }

    void Punch_Right()
    {
        animator.Play(Skill.Skillname.Punch_Right.ToString());
    }

    void Roll()
    {
        animator.Play(Skill.Skillname.Roll.ToString());
    }

    void Run_0()
    {
        animator.Play(Skill.Skillname.Run_0.ToString());
    }

    void Run_Back()
    {
        animator.Play(Skill.Skillname.Run_Back.ToString());
    }

    void Run_Left()
    {
        animator.Play(Skill.Skillname.Run_Left.ToString());
    }

    void Run_Right()
    {
        animator.Play(Skill.Skillname.Run_Right.ToString());
    }

    void Run_Shoot()
    {
        animator.Play(Skill.Skillname.Run_Shoot.ToString());
    }

    void Sword_Slash()
    {
        animator.Play(Skill.Skillname.Sword_Slash.ToString());
    }

    void Walk()
    {
        animator.Play(Skill.Skillname.Walk.ToString());
    }

    void Wave()
    {
        animator.Play(Skill.Skillname.Wave.ToString());
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Panel/NewSkill")]
public class Skill : ScriptableObject
{
    public string name = "NewSkill";
    public Sprite icon = null;
    public Skillname skillname;
    public enum Skillname
    {
        Death,
        Gun_Shoot,
        HitRecive,
        HitRecieve_2,
        Idle_0,
        Idle_Gun,
        Idle_Gun_Pointing,
        Idle_Gun_Shoot,
        Idle_Neutral,
        Idle_Sword,
        Interact,
        Kick_Left,
        Kick_Right,
        Punch_Left,
        Punch_Right,
        Roll,
        Run_0,
        Run_Back,
        Run_Left,
        Run_Right,
        Run_Shoot,
        Sword_Slash,
        Walk,
        Wave
    }

}
using UnityEngine;

[CreateAssetMenu(fileName = "New Animation Set", menuName = "Settings/Animation Set")]
public class AnimationSet : ScriptableObject
{
    public string idle, walkForward, runForward, death;
}
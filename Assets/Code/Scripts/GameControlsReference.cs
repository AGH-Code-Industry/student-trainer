using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "input", menuName = "Input")]
public class GameControlsReference : ScriptableObject
{
    [SerializeField] public InputActionAsset input;
}

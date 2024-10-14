using UnityEngine;
using Zenject;

public class CharacterDataHandler : MonoBehaviour
{
    [SerializeField] private CharacterType type;

    [Inject] private ResourceReader _resourceReader;

    public CharacterData Data { get; private set; }

    private void Awake()
    {
        var characterSettings = _resourceReader.ReadCharacterSettings(type);
        Data = new CharacterData(characterSettings);
    }
}
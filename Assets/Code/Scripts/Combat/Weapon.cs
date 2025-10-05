using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IUsable
{
    [SerializeField] private WeaponSO weaponData;

    public IEnumerable<string> GetBindings()
    {
        throw new System.NotImplementedException();
    }

    public void Use(Character user, string binding)
    {
        throw new System.NotImplementedException();
    }
}
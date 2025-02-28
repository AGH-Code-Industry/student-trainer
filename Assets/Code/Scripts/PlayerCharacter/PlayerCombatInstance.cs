using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

// An in-scene "representative" used by the Combat Service to perform attacks and coroutines
public class PlayerCombatInstance : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Transform attackOrigin;

    public Renderer[] renderers;
    RaycastHit hit;

    [Inject] PlayerService _playerService;
    Dictionary<Material, Color> _materialColors = new();


    void Start()
    {
        foreach (Renderer r in renderers)
        {
            foreach (Material material in r.materials)
            {
                _materialColors.Add(material, material.color);
            }
        }
    }

    void Update()
    {

    }

    public void Attack(float amount, float range)
    {
        Ray ray = new Ray(attackOrigin.position, attackOrigin.forward);
        bool inRange = Physics.Raycast(ray, out hit, range);
        if (!inRange)
            return;

        IDamageable damageComponent = hit.transform.root.GetComponent<IDamageable>();
        if (damageComponent != null)
            damageComponent.TakeDamage(amount);
    }

    IEnumerator FlashDamage()
    {
        foreach (Renderer r in renderers)
        {
            foreach (Material material in r.materials)
            {
                material.color = Color.red;
            }
        }

        yield return new WaitForSeconds(0.1f);

        foreach (Renderer r in renderers)
        {
            foreach (Material material in r.materials)
            {
                material.color = _materialColors[material];
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(attackOrigin.position, hit.point);
    }

    public void TakeDamage(float amount)
    {
        StartCoroutine(FlashDamage());
        _playerService.Health -= amount;
    }
}

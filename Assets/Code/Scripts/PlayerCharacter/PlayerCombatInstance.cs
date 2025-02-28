using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An in-scene "representative" used by the Combat Service to perform attacks and coroutines
public class PlayerCombatInstance : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Transform attackOrigin;

    RaycastHit hit;

    void Start()
    {

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(attackOrigin.position, hit.point);
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("Dostałem");
    }
}

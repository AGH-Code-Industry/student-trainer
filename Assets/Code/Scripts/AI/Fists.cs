using UnityEngine;

public class Fists : MonoBehaviour {
    [SerializeField] private Transform mainBody;
    [SerializeField] private ICombatant combatant;

    void Start()
    {
        combatant = mainBody.GetComponent<ICombatant>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(combatant.IsAttacking && !other.Equals(mainBody))
        {
            other.GetComponent<IDamagable>()?.TakeDamage(5);
        }
    }
}
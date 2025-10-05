using UnityEngine;

public class Player : Character, IDamageable, IKnockbackable
{
    [SerializeField] private GameObject itemInHand;
    private IUsable item;


    private void Start()
    {
        item = itemInHand.GetComponent<IUsable>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            item.Use(this, "MouseButton");
        }
    }

    public override void ApplyEffects(StatusEffectSO effect)
    {
        throw new System.NotImplementedException();
    }

    public void ApplyKnockback(Character attacker, float force)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float amount)
    {
        throw new System.NotImplementedException();
    }
}
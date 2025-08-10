using UnityEngine;

public class HealthSystem
{
    public float health { get; private set; }
    public float maxHealth;

    public float armor;

    public System.Action onHealthZero;

    void ValidateHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health < 0)
            onHealthZero?.Invoke();
    }

    public void Damage(float amount, bool ignoreArmor = false)
    {
        // The minimum damage received is always 1, regardless or armor
        float finalAmount = ignoreArmor ? amount : Mathf.Max(amount - armor, 1);
        health -= finalAmount;
        ValidateHealth();
    }
}

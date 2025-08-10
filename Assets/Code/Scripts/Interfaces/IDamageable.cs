namespace Combat
{

    // Interface for all objects that are able to be damaged (either by the player or NPCs)
    // Scripts implementing this interface should always be present at the ROOT of the damageable object
    public interface IDamageable
    {
        //void TakeDamage(float amount);
        void TakeDamage(DamageInfo damage);
    }

}
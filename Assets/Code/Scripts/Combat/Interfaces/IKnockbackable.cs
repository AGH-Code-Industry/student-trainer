using System.Numerics;

public interface IKnockbackable
{
    void ApplyKnockback(Character attacker, float force);
}
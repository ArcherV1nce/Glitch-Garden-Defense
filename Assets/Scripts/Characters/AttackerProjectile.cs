using UnityEngine;

public class AttackerProjectile : Projectile
{
    protected override bool TryDealDamage(Collider2D damageReciever)
    {
        if (CheckForDefender(damageReciever, out Defender defender))
        {
            defender.TakeDamage(Damage);
            return true;
        }

        else
        {
            return false;
        }
    }

    private bool CheckForDefender (Collider2D damageReciever, out Defender defender)
    {
        if (TryGetCharacterOnCollision(damageReciever, out Character character) && character is Defender)
        {
            defender = character as Defender;
            return true;
        }

        else
        {
            defender = null;
            return false;
        }
    }
}
using UnityEngine;

public class DefenderProjectile : Projectile
{
    protected override bool TryDealDamage(Collider2D damageReciever)
    {
        if (CheckForAttacker(damageReciever, out Attacker attacker))
        {
            DealDamage(attacker);
            return true;
        }

        else
        {
            return false;
        }
    }

    private bool CheckForAttacker(Collider2D damageReciever, out Attacker attacker)
    {
        if (TryGetCharacterOnCollision(damageReciever, out Character character) && character is Attacker)
        {
            attacker = character as Attacker;
            return true;
        }

        else
        {
            attacker = null;
            return false;
        }
    }
}
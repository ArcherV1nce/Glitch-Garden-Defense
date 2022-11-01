using UnityEngine;
using UnityEngine.Events;

public abstract class Character : MonoBehaviour, ICanDie
{
    [SerializeField] private Health _health;

    public event UnityAction<Character> Died;

    public bool IsAlive => _health.IsAlive;

    public void TakeDamage (Damage damage)
    {
        _health.ApplyDamage(damage);
        CheckDeath();
    }

    public void Die()
    {
        TakeDamage(new Damage(_health.Value));
        CheckDeath();
    }

    public bool CheckDeath()
    {
        if (_health.IsAlive == false)
        {
            TriggerDeathActions();
            Destroy(gameObject, Time.deltaTime);
            return true;
        }

        else
        {
            return false;
        }
    }

    protected virtual void TriggerDeathActions()
    {
        Died?.Invoke(this);
    }
}
using UnityEngine;
using UnityEngine.Events;

public abstract class Character : MonoBehaviour, IDeath
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

    public abstract void SetStartingState();

    public abstract void SetDefaultState();

    protected virtual void TriggerDeathActions()
    {
        Died?.Invoke(this);
    }
}
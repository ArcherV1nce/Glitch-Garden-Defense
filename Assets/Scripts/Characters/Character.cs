using UnityEngine;
using UnityEngine.Events;

public abstract class Character : MonoBehaviour, IDeath
{
    [SerializeField] private Health _health;

    public event UnityAction<Character> Died;

    public bool IsAlive => _health.IsAlive;
    public float HealthValue => _health.Value;
    public float MaxHealth => _health.ValueMax;

    protected virtual void OnValidate()
    {
        ValidateHealth();
    }

    public virtual void TakeDamage (Damage damage)
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

    private void ValidateHealth()
    {
        int value = _health.Value;
        int maxValue = _health.ValueMax;
        bool fixIsNeeded = false;

        if (_health.Value < _health.ValueMin)
        {
            fixIsNeeded = true;
            value = _health.ValueMin;
        }

        if (_health.ValueMax < _health.ValueMin)
        {
            fixIsNeeded = true;
            maxValue = _health.ValueMin;
        }

        if (_health.Value != _health.ValueMax)
        {
            fixIsNeeded = true;
            value = maxValue;
        }

        if (fixIsNeeded)
        {
            _health = null;
            _health = new Health(value, maxValue);
        }
    }
}
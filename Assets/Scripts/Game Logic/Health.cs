using System;
using UnityEngine;

[Serializable]
public class Health
{
    [SerializeField] private int _value = 100;

    public int Value => _value;
    public bool IsAlive => _value > 0;

    public Health (int value)
    {
        _value = value;
    }

    public void ApplyDamage(Damage damage)
    {
        _value -= damage.Value;
    }
}
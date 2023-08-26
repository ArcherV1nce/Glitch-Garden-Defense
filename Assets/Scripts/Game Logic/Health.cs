using System;
using UnityEngine;

[Serializable]
public class Health
{
    private const int BaseValueMin = 10;

    [SerializeField] private int _value = 100;
    [SerializeField] private int _valueMax = 100;

    public int Value => _value;
    public int ValueMax => _valueMax;
    public int ValueMin => BaseValueMin;
    public bool IsAlive => _value > 0;

    public Health (int value)
    {
        _value = ValidateValueInput(value);
        _valueMax = _value;
    }

    public Health(int value, int valueMax)
    {
        _value = ValidateValueInput(value);
        _valueMax = ValidateValueInput(value);
    }

    public void ApplyDamage(Damage damage)
    {
        _value -= damage.Value;
    }

    private int ValidateValueInput(int value)
    {
        if (value < BaseValueMin == false)
        {
            return value;
        }
        else
        {
            return BaseValueMin;
        }
    }
}
using System;
using UnityEngine;

[Serializable]
public struct Damage
{
    public static readonly int NoDamageValue = 0;

    [SerializeField] private int _value;

    public int Value => _value;
    public int ValueMin => NoDamageValue;

    public Damage (int value)
    {
        _value = value;
    }

    private void OnValidate()
    {
        ValidateValue();
    }

    private void ValidateValue()
    {
        if (_value < ValueMin)
        {
            _value = ValueMin;
        }
    }
}
using System;
using UnityEngine;

[Serializable]
public struct Damage
{
    [SerializeField] private int _value;

    public int Value => _value;

    public Damage (int value)
    {
        _value = value;
    }

    private void OnValidate ()
    {
        if (_value < 0)
        {
            _value = 0;
        }
    }
}
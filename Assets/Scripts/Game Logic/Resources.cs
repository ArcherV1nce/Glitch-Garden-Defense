using System;
using UnityEngine;

[Serializable]
public class Resources
{
    [SerializeField] private int _coins;

    public int Coins => _coins;

    public Resources (int coins)
    {
        _coins = coins;
    }

    public void ValidateCoinsAmount()
    {
        if (Coins <= 0)
        {
            _coins = 0;
        }
    }

    public void AddResouces(Resources resources)
    {
        _coins += resources.Coins;
    }

    public bool TrySpendResources(Resources resources)
    {
        if (resources == null)
        {
            return false;
        }

        if (resources.Coins > _coins)
        {
            return false;
        }

        _coins -= resources.Coins;
        return true;
    }
}
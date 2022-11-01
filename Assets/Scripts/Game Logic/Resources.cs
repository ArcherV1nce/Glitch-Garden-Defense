using System;
using UnityEngine;

[Serializable]
public class Resources
{
    [SerializeField] private int _stars;

    public int Stars => _stars;

    public Resources (int stars)
    {
        _stars = stars;
    }

    public void ValidateStarsAmount()
    {
        if (Stars <= 0)
        {
            _stars = 0;
        }
    }

    public void AddResouces(Resources resources)
    {
        _stars += resources.Stars;
    }

    public bool TrySpendResources(Resources resources)
    {
        if (resources == null)
        {
            return false;
        }

        if (resources.Stars > _stars)
        {
            return false;
        }

        _stars -= resources.Stars;
        return true;
    }
}
using UnityEngine;
using UnityEngine.Events;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private Resources _money;

    public event UnityAction AmountChanged;

    public int Coins => _money.Coins;

    public bool TrySpendResources(Defender defender)
    {
        if (_money.TrySpendResources(defender.Price))
        {
            AmountChanged?.Invoke();
            return true;
        }

        else
        {
           return false;
        }
    }

    public void AddResources(Resources resources)
    {
        _money.AddResouces(resources);
        AmountChanged?.Invoke();
    }
}
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [SerializeField] private Resources _money;

    public bool TrySpendResources(Defender defender)
    {
        return _money.TrySpendResources(defender.Price);
    }

    public void AddResources(Resources resources)
    {
         _money.AddResouces(resources);
    }
}
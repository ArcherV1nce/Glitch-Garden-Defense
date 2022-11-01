using UnityEngine;

[RequireComponent(typeof(CellSelect), typeof(DefenderSelect))]
public class DefenderSpawner : MonoBehaviour
{
    [SerializeField] private PlayerResources _money;

    private Defender _selectedDefender;

    public void SetActiveDefender (Defender defender)
    {
        _selectedDefender = defender;
    }

    public bool TrySpawnDefender (Cell cell)
    {
        if (_selectedDefender == null)
        {
            return false;
        }

        if (_money.TrySpendResources(_selectedDefender))
        {
            Defender defender = Instantiate(
                _selectedDefender, cell.transform.position, Quaternion.identity, cell.transform);

            cell.TryAddCharacter(defender);

            return true;
        }

        return false;
    }
}
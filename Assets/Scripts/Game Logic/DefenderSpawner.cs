using UnityEngine;

[RequireComponent(typeof(CellSelection))]
public class DefenderSpawner : MonoBehaviour
{
    [SerializeField] private PlayerResources _money;

    private Defender _selectedDefender;
    private CellSelection _cellSelect;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeOnSelection();
    }

    private void OnDisable()
    {
        UnsubscribeFromSelection();
    }

    public void SetSelectedDefender(Defender defender)
    {
        _selectedDefender = defender;
    }

    private void Setup()
    {
        _cellSelect = GetComponent<CellSelection>();
    }

    private void OnCellClicked(Cell cell)
    {
        TrySpawnDefender(cell);
    }

    private bool TrySpawnDefender(Cell cell)
    {
        if (_selectedDefender == null || cell.IsFree == false)
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

    private void SubscribeOnSelection()
    {
        _cellSelect.CellClicked += OnCellClicked;
    }

    private void UnsubscribeFromSelection()
    {
        _cellSelect.CellClicked -= OnCellClicked;
    }
}
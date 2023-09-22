using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CellSelection))]
public class DefenderSpawner : MonoBehaviour
{
    [SerializeField] private PlayerResources _money;
    [SerializeField] private List<DefenderSpawnLimit> _spawnLimit;

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

        if (CheckDefenderLimit(_selectedDefender))
        {
            if (_money.TrySpendResources(_selectedDefender))
            {
                Defender defender = Instantiate(
                    _selectedDefender, cell.transform.position, Quaternion.identity, cell.transform);

                cell.TryAddCharacter(defender);
                SubscribeToDefenderDeath(defender);

                return true;
            }
            else
            {
                DecreaseDefenderLimit(_selectedDefender);
            }
        }
        
        return false;
    }

    private bool CheckDefenderLimit(Defender defender)
    {
        for (int i = 0; i < _spawnLimit.Count; i++)
        {
            if (_spawnLimit[i].Defender.Id == defender.Id)
            {
                if (_spawnLimit[i].SpawnLimit > _spawnLimit[i].Spawned)
                {
                    _spawnLimit[i].DefenderSpawned();
                    return true;
                }

                else
                {
                    return false;
                }
            }
        }

        return false;
    }

    private void DecreaseDefenderLimit(Defender defender)
    {
        for (int i = 0; i < _spawnLimit.Count; i++)
        {
            if (_spawnLimit[i].Defender.Id == defender.Id)
            {
                _spawnLimit[i].DefenderRemoved();
            }
        }
    }

    private void OnDied(Defender defender)
    {
        DecreaseDefenderLimit(defender);
        UnsubscribeFromDefenderDeath(defender);
    }

    private void SubscribeOnSelection()
    {
        _cellSelect.CellClicked += OnCellClicked;
    }

    private void SubscribeToDefenderDeath(Defender defender)
    {
        defender.Died += OnDied;
    }

    private void UnsubscribeFromSelection()
    {
        _cellSelect.CellClicked -= OnCellClicked;
    }

    private void UnsubscribeFromDefenderDeath(Defender defender)
    {
        defender.Died -= OnDied;
    }
}
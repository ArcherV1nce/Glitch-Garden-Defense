using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Line))]
public class LineCells : MonoBehaviour
{
    [SerializeField] private Cell _template;
    [SerializeField] private int _cellsCount;
    [SerializeField] private List<Cell> _cells;

    private Line _line;

    private void Awake()
    {
        Setup();
    }

    public void GenerateCells()
    {
        int startingCellNumber = 0;

        if (_cells == null)
        {
            _cells = new List<Cell>();
        }

        else if (_cellsCount > _cells.Count)
        {
            startingCellNumber = _cells.Count;

            for (int i = startingCellNumber; i < _cellsCount; i++)
            {
                Vector3 lastCellPosition = TryGetLastCellPosition();
                Vector3 newCellPosition = new Vector3(lastCellPosition.x + _template.Size, lastCellPosition.y, lastCellPosition.z);
                Cell newCell = Instantiate(_template, newCellPosition, Quaternion.identity, gameObject.transform);
                newCell.SetLine(_line);
                _cells.Add(newCell);
            }
        }
    }

    public void DestroyCells()
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            DestroyImmediate(_cells[i].gameObject);
        }

        _cells.Clear();
    }

    private void Setup()
    {
        _line = GetComponent<Line>();
    }

    private Vector3 TryGetLastCellPosition()
    {
        if (_cells.Count > 0)
        {
            return _cells.Last().transform.position;
        }

        else
        {
            return transform.position;
        }
    }
}
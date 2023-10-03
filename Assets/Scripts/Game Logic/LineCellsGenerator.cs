using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Line))]
public class LineCellsGenerator : MonoBehaviour
{
    [SerializeField] private Cell _template;
    [SerializeField] private int _cellsCount;
    [SerializeField] private List<Cell> _cells;

    private Line _line;

    private void Awake()
    {
        Setup();
        CheckChildCells();
    }

    private void OnValidate()
    {
        Setup();
    }

    public void GenerateCells()
    {
        int startingCellNumber;

        _cells ??= new List<Cell>();

        if (_cellsCount > _cells.Count)
        {
            startingCellNumber = _cells.Count;

            for (int i = startingCellNumber; i < _cellsCount; i++)
            {
                Vector3 lastCellPosition = TryGetLastCellPosition();
                Vector3 newCellPosition = new(lastCellPosition.x + _template.Size, lastCellPosition.y, lastCellPosition.z);
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

    private void CheckChildCells()
    {
        _cells.Clear();
        List<Cell> cells = GetComponentsInChildren<Cell>().ToList();
        RegenerateCells();

        if (cells.Count > 0)
        {
            List<Cell> cellsToDestroy = new();

            for (int i = 0; i < _cellsCount; i++)
            {
                if (cells[i].IsFree)
                {
                    cellsToDestroy.Add(cells[i]);
                }

                else if (cells[i].IsFree == false)
                {
                    cellsToDestroy.Add(_cells[i]);
                    _cells[i] = cells[i];
                }
            }

            for (int i = cellsToDestroy.Count; i > 0; i--)
            {
                GameObject objectToDestroy = cellsToDestroy[i - 1].gameObject;
                Destroy(objectToDestroy);
            }

            cellsToDestroy.Clear();
        }
    }

    private void Setup()
    {
        if (_line == null)
        {
            _line = GetComponent<Line>();
        }
    }

    private void RegenerateCells()
    {
        DestroyCells();
        GenerateCells();
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
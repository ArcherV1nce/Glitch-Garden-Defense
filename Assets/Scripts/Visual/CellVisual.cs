using UnityEngine;

[RequireComponent(typeof(Cell), typeof(SpriteRenderer))]
public class CellVisual : MonoBehaviour
{
    private Cell _cell;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        _cell.FillChanged += OnFillChanged;
    }

    private void OnDisable()
    {
        _cell.FillChanged -=OnFillChanged;
    }

    private void OnValidate()
    {
        if (_cell == null || _spriteRenderer == null)
        {
            Setup();
        }
    }

    private void Setup()
    {
        _cell = GetComponent<Cell>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnFillChanged()
    {
        _spriteRenderer.enabled = _cell.IsFree;
    }
}
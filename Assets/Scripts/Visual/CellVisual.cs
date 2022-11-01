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
        _cell.FillChanged.AddListener(OnFillChanged);
    }

    private void OnDisable()
    {
        _cell.FillChanged.RemoveListener(OnFillChanged);
    }

    private void Setup()
    {
        _cell = GetComponent<Cell>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnFillChanged()
    {
        if (_cell.IsFree)
        {
            _spriteRenderer.enabled = true;
        }

        else
        {
            _spriteRenderer.enabled = false;
        }
    }
}
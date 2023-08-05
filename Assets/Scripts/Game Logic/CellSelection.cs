using UnityEngine;
using UnityEngine.Events;

public class CellSelection : MonoBehaviour
{
    [SerializeField] private LayerMask _contactFilter;
    
    private Camera _camera;

    public UnityAction<Cell> CellClicked;

    private void Awake()
    {
        Setup();
    }

    private void Update()
    {
        CheckMouseInput();
    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CheckCellDetection(out Cell cell))
            {
                CellClicked?.Invoke(cell);
            }
        }
    }

    private bool CheckCellDetection(out Cell cell)
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D target = Physics2D.OverlapPoint(mousePosition, _contactFilter);

        if (target != null && target.TryGetComponent<Cell>(out cell))
        {
            return true;
        }

        else
        {
            cell = null;
            return false;
        }
    }

    private void Setup()
    {
        _camera = Camera.main;
    }
}
using UnityEngine;
using UnityEngine.Events;

public class CellSelection : MonoBehaviour
{
    private const int LeftMouseButtonId = 0;

    [SerializeField] private LayerMask _contactFilter;
    
    private Camera _camera;
    private Level _level;
    private bool _isEnabled;

    public UnityAction<Cell> CellClicked;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToLevel();
    }

    private void OnDisable()
    {
        UnsubscribeFromLevel();
    }

    private void Update()
    {
        CheckMouseInput();
    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButtonDown(LeftMouseButtonId) && _isEnabled)
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

    private void DisableSelection(bool finished)
    {
        _isEnabled = finished == false;
    }

    private void Setup()
    {
        _camera = Camera.main;
        _level = FindObjectOfType<Level>();
        _isEnabled = true;
    }

    private void SubscribeToLevel()
    {
        _level.Finished += DisableSelection;
    }

    private void UnsubscribeFromLevel()
    {
        _level.Finished -= DisableSelection;
    }
}
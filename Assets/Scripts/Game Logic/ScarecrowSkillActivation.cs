using UnityEngine;

public class ScarecrowSkillActivation : MonoBehaviour
{
    [SerializeField] private LayerMask _contactFilter;

    private Camera _camera;

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
            if (CheckScarecrowButtonDetection(out ScarecrowRiposteButton button))
            {
                button.TriggerClick();
            }
        }
    }

    private bool CheckScarecrowButtonDetection(out ScarecrowRiposteButton button)
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D target = Physics2D.OverlapPoint(mousePosition, _contactFilter);

        if (target != null && target.TryGetComponent<ScarecrowRiposteButton>(out button))
        {
            return true;
        }

        else
        {
            button = null;
            return false;
        }
    }

    private void Setup()
    {
        _camera = Camera.main;
    }
}
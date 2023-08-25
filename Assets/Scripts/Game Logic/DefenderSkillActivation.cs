using UnityEngine;
using UnityEngine.UIElements;

public class DefenderSkillActivation : MonoBehaviour
{
    private const int LeftMouseButtonId = 0;

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
        if (Input.GetMouseButtonDown(LeftMouseButtonId))
        {
            if (CheckScarecrowDetection(out Defender defender))
            {
                defender.UseSkill();
            }
        }
    }

    private bool CheckScarecrowDetection(out Defender defender)
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D target = Physics2D.OverlapPoint(mousePosition, _contactFilter);

        if (target != null && target.TryGetComponent<Defender>(out defender))
        {
            return true;
        }

        else
        {
            defender = null;
            return false;
        }
    }

    private void Setup()
    {
        _camera = Camera.main;
    }
}
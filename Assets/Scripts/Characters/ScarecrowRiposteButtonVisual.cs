using UnityEngine;

public class ScarecrowRiposteButtonVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _buttonSprite;
    [SerializeField] private SpriteRenderer _textSprite;

    private Scarecrow _scarecrow;

    private void OnValidate()
    {
        ValidadeScarecrow();
    }

    private void OnEnable()
    {
        ValidadeScarecrow();
        SubscribeToScarecrow();
    }

    private void OnDisable()
    {
        UnsubscribeFromScarecrow();
    }

    private void UpdateVisualState(bool isActive)
    {
        if (isActive)
        {
            _buttonSprite.enabled = true;
            _textSprite.enabled = true;
        }
        else
        {
            _buttonSprite.enabled = false;
            _textSprite.enabled = false;
        }
    }

    private void ValidadeScarecrow()
    {
        _scarecrow = GetComponentInParent<Scarecrow>();
    }

    private void SubscribeToScarecrow()
    {
        _scarecrow.OnAttackStatusChanged += UpdateVisualState;
    }

    private void UnsubscribeFromScarecrow()
    {
        _scarecrow.OnAttackStatusChanged -= UpdateVisualState;
    }
}
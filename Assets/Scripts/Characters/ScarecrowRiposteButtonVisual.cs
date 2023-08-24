using UnityEngine;

[RequireComponent(typeof(ScarecrowRiposteButton))]
public class ScarecrowRiposteButtonVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _buttonSprite;
    [SerializeField] private SpriteRenderer _textSprite;

    private ScarecrowRiposteButton _button;

    private void OnValidate()
    {
        ValidateButton();
    }

    private void OnEnable()
    {
        ValidateButton();
        SubscribeToRiposteButton();
    }

    private void OnDisable()
    {
        UnsubscribeFromRiposteButton();
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

    private void ValidateButton()
    {
        _button = GetComponent<ScarecrowRiposteButton>();
    }

    private void SubscribeToRiposteButton()
    {
        _button.OnSkillStatusUpdated += UpdateVisualState;
    }

    private void UnsubscribeFromRiposteButton()
    {
        _button.OnSkillStatusUpdated -= UpdateVisualState;
    }
}
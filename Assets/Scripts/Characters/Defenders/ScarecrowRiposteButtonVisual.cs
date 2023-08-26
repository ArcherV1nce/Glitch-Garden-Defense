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

    private void UpdateVisualState()
    {

    }

    private void ValidadeScarecrow()
    {
        _scarecrow = GetComponentInParent<Scarecrow>();
    }

    private void SubscribeToScarecrow()
    {
    }

    private void UnsubscribeFromScarecrow()
    {
    }
}
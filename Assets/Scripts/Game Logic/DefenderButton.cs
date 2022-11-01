using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DefenderButton : MonoBehaviour
{
    [SerializeField] Defender _defender;
    [SerializeField] DefenderSelect _select;

    private Button _button;

    public Resources DefenderPrice => _defender.Price;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToButtonClick();
    }

    private void OnDisable()
    {
        UnsubscribeFromButtonClick();
    }

    private void Setup()
    {
        _button = GetComponent<Button>();
    }

    private void SubscribeToButtonClick()
    {
        _button.onClick?.AddListener(SetDefender);
    }

    private void UnsubscribeFromButtonClick()
    {
        _button.onClick?.RemoveListener(SetDefender);
    }

    private void SetDefender ()
    {
        if (_defender == null)
            return;

        _select.SetDefender(_defender);
    }
}
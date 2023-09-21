using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DefenderButton : MonoBehaviour
{
    [SerializeField] private Defender _defender;

    private Button _button;

    public UnityAction<Defender> DefenderSelected;

    public int DefenderPrice => _defender.Price.Coins;

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

    public void SendDefender()
    {
        if (_defender != null)
            DefenderSelected?.Invoke(_defender);
    }

    private void Setup()
    {
        _button = GetComponent<Button>();
    }

    private void SubscribeToButtonClick()
    {
        _button.onClick?.AddListener(SendDefender);
    }

    private void UnsubscribeFromButtonClick()
    {
        _button.onClick?.RemoveListener(SendDefender);
    }
}
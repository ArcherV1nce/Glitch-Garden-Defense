using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ResourcesView : MonoBehaviour
{
    private const string CoinsTextDefault = "Coins: ";

    [SerializeField] private PlayerResources _resources;
    [SerializeField] private string _coinsText;

    private TextMeshProUGUI _coinsDisplay;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToResoucesChange();
    }

    private void OnDisable()
    {
        UnsubscribeFromResoucesChange();
    }

    private void OnValidate()
    {
        if (_coinsDisplay == null)
        {
            Setup();
        }

        ValidateText();
    }

    private void Setup()
    {
        _coinsDisplay = GetComponent<TextMeshProUGUI>();
        UpdateCoinsDisplay();
    }

    private void ValidateText()
    {
        if (_coinsText == "")
        {
            _coinsText = CoinsTextDefault;
        }
    }

    private void SubscribeToResoucesChange()
    {
        _resources.AmountChanged += UpdateCoinsDisplay;
    }

    private void UnsubscribeFromResoucesChange()
    {
        _resources.AmountChanged -= UpdateCoinsDisplay;
    }

    private void UpdateCoinsDisplay()
    {
        _coinsDisplay.text = _coinsText + _resources.Coins;
    }
}

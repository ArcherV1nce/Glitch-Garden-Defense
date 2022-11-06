using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ResourcesView : MonoBehaviour
{
    private const string StarsTextDefault = "Stars: ";

    [SerializeField] private PlayerResources _resources;
    [SerializeField] private string _starsText;

    private TextMeshProUGUI _starsDisplay;

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
        if (_starsDisplay == null)
        {
            Setup();
        }

        ValidateText();
    }

    private void Setup()
    {
        _starsDisplay = GetComponent<TextMeshProUGUI>();
        UpdateStarsDisplay();
    }

    private void ValidateText()
    {
        if (_starsText == "")
        {
            _starsText = StarsTextDefault;
        }
    }

    private void SubscribeToResoucesChange()
    {
        _resources.AmountChanged += UpdateStarsDisplay;
    }

    private void UnsubscribeFromResoucesChange()
    {
        _resources.AmountChanged -= UpdateStarsDisplay;
    }

    private void UpdateStarsDisplay()
    {
        _starsDisplay.text = _starsText + _resources.Stars;
    }
}

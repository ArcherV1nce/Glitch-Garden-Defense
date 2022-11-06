using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class VillageHealthView : MonoBehaviour
{
    private const string HealthTextDefault = "Village health: ";

    [SerializeField] private Village _village;
    [SerializeField] private string _healthText;
    
    private TextMeshProUGUI _healthDisplay;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToVillageHealthChange();
    }

    private void OnDisable()
    {
        UnsubscribeFromVillageHealthChange();
    }

    private void OnValidate()
    {
        if (_healthDisplay == null)
        {
            Setup();
        }

        ValidateText();
    }

    private void Setup()
    {
        _healthDisplay = GetComponent<TextMeshProUGUI>();
        UpdateHealthDisplay();
    }

    private void ValidateText()
    {
        if (_healthText == "")
        {
            _healthText = HealthTextDefault;
        }
    }

    private void SubscribeToVillageHealthChange()
    {
        _village.HealthChanged += UpdateHealthDisplay;
    }

    private void UnsubscribeFromVillageHealthChange()
    {
        _village.HealthChanged -= UpdateHealthDisplay;
    }

    private void UpdateHealthDisplay()
    {
        _healthDisplay.text = _healthText + _village.Health;
    }
}
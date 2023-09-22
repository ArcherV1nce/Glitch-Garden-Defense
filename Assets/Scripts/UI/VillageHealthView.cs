using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VillageHealthView : MonoBehaviour
{

    [SerializeField] private Village _village;
    [SerializeField] private Slider _healthView;
    
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
        if (_healthView == null)
        {
            Setup();
        }
    }

    private void Setup()
    {
        _healthView = GetComponent<Slider>();
        UpdateHealthDisplay();
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
        _healthView.value = (float)_village.Health/_village.HealthMax;
    }
}
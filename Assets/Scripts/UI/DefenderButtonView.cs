using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image), typeof(DefenderButton))]
public class DefenderButtonView : MonoBehaviour
{
    private readonly Color32 AvailableColor = new Color32(255, 255, 255, 255);
    private readonly Color32 LimitedColor = new Color32(255, 0, 0, 255);

    [SerializeField] private Sprite _defenderImage;
    [SerializeField] TextMeshProUGUI _priceText;

    private DefenderButton _button;
    private Image _imageComponent;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToButton();
    }

    private void OnDisable()
    {
        UnsubscribeFromButton();
    }

    private void OnValidate()
    {
        ValidateImage();
    }

    private void Setup()
    {
        _button = GetComponent<DefenderButton>();
        _priceText.text = _button.DefenderPrice.ToString();
        
        if (_imageComponent == null)
        {
            _imageComponent = GetComponent<Image>();
            _imageComponent.sprite = _defenderImage;
        }
    }

    private void OnDefenderAvailabilityUpdated(bool isAvailable)
    {
        UpdateColor(isAvailable);
    }

    private void UpdateColor(bool isAvailable)
    {
        if (isAvailable)
        {
            _imageComponent.color = AvailableColor;
        }

        else
        {
            _imageComponent.color = LimitedColor;
        }
    }

    private void ValidateImage()
    {
        if (_imageComponent == null)
        {
            _imageComponent = GetComponent<Image>();
        }

        if (_defenderImage == null)
        {
            Debug.LogError($"{_defenderImage} is null. Defender image is required.");
        }

        else
        {
            _imageComponent.sprite = _defenderImage;
        }
    }

    private void SubscribeToButton()
    {
        _button.DefenderAvailabilityUpdated += OnDefenderAvailabilityUpdated;
    }

    private void UnsubscribeFromButton()
    {
        _button.DefenderAvailabilityUpdated -= OnDefenderAvailabilityUpdated;
    }
}
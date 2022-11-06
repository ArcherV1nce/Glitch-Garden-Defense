using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image), typeof(DefenderButton))]
public class DefenderButtonView : MonoBehaviour
{
    [SerializeField] private Sprite _defenderImage;
    [SerializeField] TextMeshProUGUI _priceText;

    private DefenderButton _defenderButton;
    private Image _imageComponent;

    private void Awake()
    {
        Setup();
    }

    private void OnValidate()
    {
        ValidateImage();
    }

    private void Setup()
    {
        _defenderButton = GetComponent<DefenderButton>();
        _priceText.text = _defenderButton.DefenderPrice.ToString();
        
        if (_imageComponent == null)
        {
            _imageComponent = GetComponent<Image>();
            _imageComponent.sprite = _defenderImage;
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
}
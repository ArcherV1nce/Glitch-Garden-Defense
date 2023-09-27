using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RatingStarView : MonoBehaviour
{
    [SerializeField] private Color32 _failed;
    [SerializeField] private Color32 _competed;
    [SerializeField] private bool _isCompleted;

    private Image _image;

    private void Awake()
    {
        Setup();
    }

    private void OnValidate()
    {
        ValidateImage();
        SetColor();
    }

    private void Setup()
    {
        ValidateImage();
    }

    private void ValidateImage()
    {
        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
    }

    private void SetColor()
    {
        switch (_isCompleted)
        {
            case true:
                _image.color = _competed;
                break;
                
            case false:
                _image.color = _failed;
                break;
        }
    }
}
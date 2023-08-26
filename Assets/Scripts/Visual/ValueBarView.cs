using UnityEngine;

public class ValueBarView : MonoBehaviour
{
    protected const float FillingValueMin = 0f;
    protected const float FillingValueMax = 1f;

    [SerializeField] private Transform _filling;

    protected float Value;

    protected virtual void Awake()
    {
        Setup();
    }

    protected virtual void OnValidate()
    {
        ValidateTransform();
    }

    protected virtual void OnValueChanged(float value, float maxValue)
    {
        Value = value/maxValue;
        UpdateFilling();
    }

    private void ValidateTransform()
    {
        if (_filling == null)
        {
            _filling = GetComponent<Transform>();
        }
    }

    protected void UpdateFilling()
    {
        if (Value < FillingValueMin)
        {
            Value = FillingValueMin;
        }
        else if (Value > FillingValueMax)
        {
            Value = FillingValueMax;
        }

        _filling.localScale = new Vector3 (Value, _filling.localScale.y, _filling.localScale.z);
    }

    protected virtual void Setup()
    {
        ValidateTransform();
    }
}
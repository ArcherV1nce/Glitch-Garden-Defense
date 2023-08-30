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
        UpdateFilling(value, maxValue);
    }

    private void ValidateTransform()
    {
        if (_filling == null)
        {
            _filling = GetComponent<Transform>();
        }
    }

    protected void SetFillingScale(float valueX)
    {
        _filling.localScale = new Vector3(valueX, _filling.localScale.y, _filling.localScale.z);
    }

    protected void UpdateFilling(float value, float maxValue)
    {
        Value = value / maxValue;
        if (Value < FillingValueMin)
        {
            Value = FillingValueMin;
        }
        else if (Value > FillingValueMax)
        {
            Value = FillingValueMax;
        }

        SetFillingScale(Value);
    }

    protected virtual void Setup()
    {
        ValidateTransform();
    }
}
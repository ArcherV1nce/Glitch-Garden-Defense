using UnityEngine;

public class Healthbar : ValueBarView
{
    [SerializeField] private Character _character;

    protected override void Awake()
    {
        base.Awake();
        ValidateCharacter();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateCharacter();
    }

    private void OnEnable()
    {
        SubscribeToCharacterHealthChange();
    }

    private void OnDisable()
    {
        UnsubscribeFromCharacterHealthChange();
    }

    protected override void Setup()
    {
        base.Setup();
        ValidateCharacter();
    }

    private void ValidateCharacter()
    {
        if (_character == null)
        {
            _character = GetComponentInParent<Character>();
        }
    }

    private void SubscribeToCharacterHealthChange()
    {
        _character.HealthValueChanged += OnValueChanged;
    }

    private void UnsubscribeFromCharacterHealthChange()
    {
        _character.HealthValueChanged -= OnValueChanged;
    }
}
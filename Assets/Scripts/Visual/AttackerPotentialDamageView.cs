public class AttackerPotentialDamageView : ValueBarView
{
    private const float BaseValue = 0f;

    private Attacker _attacker;

    protected override void Awake()
    {
        base.Awake();
        ResetValue();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateAttacker();
    }

    public void UpdatePotentialDamageView(float damage)
    {
        UpdateFilling(damage, _attacker.MaxHealth);
    }

    private void ValidateAttacker()
    {
        if (_attacker == null)
        {
            _attacker = GetComponentInParent<Attacker>();
        }
    }

    private void ResetValue()
    {
        UpdatePotentialDamageView(BaseValue);
    }

    protected override void Setup()
    {
        base.Setup();
        ValidateAttacker();
    }
}
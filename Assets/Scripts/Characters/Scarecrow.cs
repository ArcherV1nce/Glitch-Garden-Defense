using UnityEngine;
using UnityEngine.Events;

public class Scarecrow : Defender
{
    private const float RiposteThresholdMin = 0.1f;
    private const float RiposteDamageMultiplierMax = 3f;

    [SerializeField] private DefenderState _charged;
    [SerializeField] private DefenderState _chargedAttacked;
    [SerializeField] private DefenderRiposte _riposte;
    [SerializeField] private float _damageRequiredForRiposte; //Перенести в класс Riposte
    [SerializeField, Range(0, RiposteDamageMultiplierMax)] private float _riposteMultiplier; //Перенести в класс Riposte

    private float _damageTaken;

    public UnityEvent<DefenderState> StateChanged;
    public UnityEvent<bool> RiposteChargeStatusUpdated;
    public UnityEvent<Damage> RiposteDamageUpdated;

    public Scarecrow(Resources price) : base(price) { }

    public bool IsCharged => _damageTaken >= _damageRequiredForRiposte;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateRiposteDamageMin();
    }

    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
        AccumulateDamageForAttack(damage);
        CheckRiposteStatus();
    }

    public override void SetAttacked()
    {
        if (IsCharged)
        {
            SetActiveState(_chargedAttacked);
            StateChanged?.Invoke(Active);
        }
    }

    public override void SetIdle()
    {
        if (IsCharged)
        {
            SetActiveState(_charged);
        }
        else
        {
            SetDefaultState();
        }

        StateChanged?.Invoke(Active);
    }

    public void PerformRiposte()
    {

    }

    private void CheckRiposteStatus() //Перенести в класс Riposte
    {
        RiposteChargeStatusUpdated?.Invoke(IsCharged);

        if (IsCharged)
        {
            RiposteDamageUpdated?.Invoke(GetRiposteDamage(_damageTaken));
        }
        else
        {
            RiposteDamageUpdated?.Invoke(new Damage(Damage.NoDamageValue));
        }
    }

    private void AccumulateDamageForAttack(Damage damage) //Перенести в класс Riposte
    {
        _damageTaken += damage.Value;
    }

    private void ValidateRiposteDamageMin() //Перенести в класс Riposte
    {
        if (_damageRequiredForRiposte <= MaxHealth * RiposteThresholdMin || _damageRequiredForRiposte < MaxHealth == false)
        {
            _damageRequiredForRiposte = MaxHealth * RiposteThresholdMin;
        }
    }

    private Damage GetRiposteDamage(float damageAccumulated) //Перенести в класс Riposte
    {
        float riposteDamageValue = damageAccumulated * _riposteMultiplier;
        return new Damage((int)riposteDamageValue);
    }
}
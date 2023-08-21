using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ScarecrowSkill))]
public class Scarecrow : Defender
{
    [SerializeField] private DefenderState _charged;
    [SerializeField] private DefenderState _chargedAttacked;
    [SerializeField] private ScarecrowSkill _riposte;
    
    public UnityEvent<DefenderState> StateChanged;
    public UnityEvent<bool> RiposteChargeStatusUpdated;
    public UnityEvent<Damage> RiposteDamageUpdated;
    public UnityEvent RiposteTriggered;

    public Scarecrow(Resources price) : base(price) { }

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateSkillComponent();
    }

    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
        RiposteDamageUpdated?.Invoke(damage);
        _riposte.CheckRiposteStatus();
    }

    public override void SetAttacked()
    {
        if (_riposte.IsCharged)
        {
            SetActiveState(_chargedAttacked);
            StateChanged?.Invoke(Active);
        }
    }

    public override void SetIdle()
    {
        if (_riposte.IsCharged)
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

    private void ValidateSkillComponent()
    {
        if (_riposte == null)
        {
            _riposte = GetComponent<ScarecrowSkill>();
        }
    }
}
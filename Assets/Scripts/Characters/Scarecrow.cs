using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ScarecrowSkill))]
public class Scarecrow : Defender
{
    [SerializeField] private DefenderState _charged;
    [SerializeField] private DefenderState _chargedAttacked;
    [SerializeField] private ScarecrowSkill _riposte;

    private bool _isAttacked;

    public UnityAction<bool> OnAttackStatusChanged;
    public UnityAction OnRiposteTriggered;
    public UnityEvent<Damage> DamageTaken;
    public UnityEvent<DefenderState> StateChanged;

    public bool IsAttacked => _isAttacked;

    public Scarecrow(Resources price) : base(price) { }

    private void OnEnable()
    {
        ValidateSkillComponent();
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateSkillComponent();
    }

    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
        DamageTaken?.Invoke(damage);
        _riposte.CheckRiposteStatus();
    }

    public override void SetAttacked()
    {
        _isAttacked = true;

        if (_riposte.IsCharged)
        {
            SetActiveState(_chargedAttacked);
            StateChanged?.Invoke(Active);
            OnAttackStatusChanged?.Invoke(IsAttacked);
        }
    }

    public override void UseSkill()
    {
        if (_isAttacked)
        {
            OnRiposteTriggered?.Invoke();
        }
    }

    public override void SetIdle()
    {
        _isAttacked = false;

        if (_riposte.IsCharged)
        {
            Debug.Log("Activated Charged state");
            SetActiveState(_charged);
        }
        else
        {
            SetDefaultState();
        }

        StateChanged?.Invoke(Active);
    }

    private void ChooseStateByChargeStatus(bool isCharged)
    {
        switch (isCharged)
        {
            case true:
                if (_isAttacked)
                {
                    SetActiveState(_chargedAttacked);
                }
                else
                {
                    SetActiveState(_charged);
                }
                break;

            case false:
                SetDefaultState();
            break;
        }

        StateChanged?.Invoke(Active);
    }

    private void ValidateSkillComponent()
    {
        if (_riposte == null)
        {
            _riposte = GetComponent<ScarecrowSkill>();
        }
    }

    private void SubscribeToEvents()
    {
        _riposte.ChargeStatusUpdated?.AddListener(ChooseStateByChargeStatus);
    }

    private void UnsubscribeFromEvents()
    {
        _riposte.ChargeStatusUpdated?.RemoveListener(ChooseStateByChargeStatus);
    }
}
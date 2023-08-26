using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ScarecrowSkill))]
public class Scarecrow : Defender
{
    [SerializeField] private DefenderState _charged;
    [SerializeField] private DefenderState _chargedAttacked;
    [SerializeField] private ScarecrowSkill _riposteSkill;
    [SerializeField] private DefenderAlertArea _alertArea;

    private bool _isAlerted;

    public UnityAction<bool> AttackStatusUpdated;
    public UnityAction RiposteTriggered;
    public UnityEvent<Damage> DamageTaken;
    public UnityEvent<DefenderState> StateChanged;

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
        ValidateAlertArea();
    }

    public override void TakeDamage(Damage damage)
    {
        base.TakeDamage(damage);
        DamageTaken?.Invoke(damage);
        _riposteSkill.CheckRiposteStatus();
    }

    public override void SetAttacked()
    {
        _isAlerted = true;
        ChooseStateByAlert(_isAlerted);
        AttackStatusUpdated?.Invoke(_isAlerted);
    }

    public override void UseSkill()
    {
        if (_isAlerted&&_riposteSkill.IsCharged)
        {
            RiposteTriggered?.Invoke();
        }
    }

    public override void SetIdle()
    {
        if(_alertArea.IsAlerted == false)
        {
            _isAlerted = false;
        }

        if (_riposteSkill.IsCharged)
        {
            SetActiveState(_charged);
        }
        else
        {
            SetDefaultState();
        }

        AttackStatusUpdated?.Invoke(_isAlerted);
        StateChanged?.Invoke(Active);
    }

    private void ChooseStateByChargeStatus(bool isCharged)
    {
        if (isCharged)
        {
            SetActiveState(_chargedAttacked);
            StateChanged?.Invoke(Active);
            AttackStatusUpdated?.Invoke(_isAlerted);
        }
        else
        {
            SetIdle();
        }
    }

    private void ChooseStateByAlert(bool isAlert)
    {
        _isAlerted = isAlert;

        if (isAlert)
        {
            ChooseStateByChargeStatus(_riposteSkill.IsCharged);
        }
        else
        {
            SetIdle();
        }
    }

    private void ValidateSkillComponent()
    {
        if (_riposteSkill == null)
        {
            _riposteSkill = GetComponent<ScarecrowSkill>();
        }
    }

    private void ValidateAlertArea()
    {
        if (_alertArea == null)
        {
            _alertArea = GetComponentInChildren<DefenderAlertArea>();
        }

        if (_alertArea == null)
        {
            Debug.LogError($"DefenderDetectionArea component was not found in {this.name}'s children.");
        }
    }

    private void SubscribeToEvents()
    {
        _riposteSkill.ChargeStatusUpdated?.AddListener(ChooseStateByChargeStatus);
        _alertArea.AlertUpdated += ChooseStateByAlert;
    }

    private void UnsubscribeFromEvents()
    {
        _riposteSkill.ChargeStatusUpdated?.RemoveListener(ChooseStateByChargeStatus);
        _alertArea.AlertUpdated -= ChooseStateByAlert;
    }
}
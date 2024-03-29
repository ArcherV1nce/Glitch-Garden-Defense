﻿using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AttackerMovement))]
public class Attacker : Character
{
    [SerializeField] private Damage _damage;
    [SerializeField] private Resources _reward;

    [SerializeField] protected MeleeAttack AttackArea;
    [SerializeField] protected AttackerState Default;
    [SerializeField] protected AttackerState Attacking;
    [SerializeField] protected AttackerState Spawning;

    private Defender _currentTarget = null;
    private AttackerState _active;

    public event UnityAction Attacked;
    public new event UnityAction<Attacker> Died;
    public event UnityAction<AttackerState> StateChanged;

    public AttackerState Active => _active;
    public Damage Damage => _damage;
    public Resources Reward => _reward;

    protected override void Awake()
    {
        base.Awake();
        SetStartingState();
    }

    protected virtual void OnEnable()
    {
        SubscribeToTargetDeath();
        SetActiveState(Spawning);
        SubscribeToMeleeAttack();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeFromTargetDeath();
        UnsubscribeFromMeleeAttack();
    }

    public Resources GetReward ()
    {
        return _reward;
    }

    public void Attack (Defender target)
    {
        SetActiveState(Attacking);
        
        if (target != _currentTarget)
        {
            _currentTarget = target;
        }
    }

    public void SetSpawning()
    {
        SetActiveState(Spawning);
    }

    public virtual void StartMoving()
    {
        SetDefaultState();
    }

    public override void TakeDamage(Damage damage)
    {
        Attacked?.Invoke();
        base.TakeDamage(damage);
    }

    public override void SetStartingState()
    {
        SetSpawning();
    }

    public override void SetDefaultState()
    {
        SetActiveState(Default);
    }

    private void StrikeCurrentTarget()
    {
        if (!_currentTarget)
        {
            SetActiveState(Default);
        }

        else
        {
            _currentTarget.TakeDamage(Damage);
        }
    }

    private void ClearTarget(Character character)
    {
        if (character is Defender)
        {
            _currentTarget = null;
            StartMoving();
        }
    }

    private void SubscribeToTargetDeath()
    {
        if (_currentTarget != null)
        {
            _currentTarget.Died +=(ClearTarget);
        }
    }

    private void UnsubscribeFromTargetDeath()
    {
        if (_currentTarget != null)
        {
            _currentTarget.Died -= (ClearTarget);
        }
    }

    private void SubscribeToMeleeAttack()
    {
        AttackArea.CharacterEnteredMeleeAttackArea.AddListener(TrySetTarget);
    }

    private void UnsubscribeFromMeleeAttack()
    {
        AttackArea.CharacterEnteredMeleeAttackArea.RemoveListener(TrySetTarget);
    }

    protected void SetActiveState(AttackerState newState)
    {
        _active = newState;
        StateChanged?.Invoke(Active);
    }

    protected virtual void TrySetTarget(Character character)
    {
        if (character is Defender)
        {
            _currentTarget = character as Defender;
            Attack(character as Defender);
        }
    }

    protected void CheckTarget()
    {
        if (_currentTarget != null)
        {
            TrySetTarget(_currentTarget);
        }
    }

    protected override void TriggerDeathActions()
    {
        Died?.Invoke(this);
    }
}
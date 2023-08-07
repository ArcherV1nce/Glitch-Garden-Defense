using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AttackerMovement))]
public class Attacker : Character
{
    [SerializeField] private Damage _damage;
    [SerializeField] private Resources _reward;
    [SerializeField] protected CharacterState Attacking;
    [SerializeField] protected CharacterState Spawning;

    private Defender _currentTarget = null;

    public new event UnityAction<Attacker> Died;
    public UnityEvent<CharacterState> StateChanged;

    public Damage Damage => _damage;
    public Resources Reward => _reward;

    protected virtual void OnEnable()
    {
        SubscribeToTargetDeath();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeFromTargetDeath();
    }

    public Resources GetReward ()
    {
        return _reward;
    }

    public void Attack (Defender target)
    {
        _currentTarget = target;
        SetActiveState(Attacking);
    }

    private void StrikeCurrentTarget ()
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

    public void SetSpawning()
    {
        SetActiveState(Spawning);
    }

    public void StartMoving()
    {
        SetActiveState(Default);
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

    protected override void SetActiveState(CharacterState newState)
    {
        base.SetActiveState(newState);
        StateChanged?.Invoke(Active);
    }

    protected override void TriggerDeathActions()
    {
        Died?.Invoke(this);
    }
}
using UnityEngine;
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

    public new event UnityAction<Attacker> Died;
    public UnityEvent<AttackerState> StateChanged;

    public AttackerState Active => _active;
    public Damage Damage => _damage;
    public Resources Reward => _reward;

    protected virtual void Awake()
    {
        SetStartingState();
    }

    protected virtual void OnEnable()
    {
        SubscribeToTargetDeath();
        SetActiveState(Spawning);
        AttackArea.CharacterEnteredMeleeAttackArea.AddListener(TrySetTarget);
    }

    protected virtual void OnDisable()
    {
        UnsubscribeFromTargetDeath();
        AttackArea.CharacterEnteredMeleeAttackArea.RemoveListener(TrySetTarget);
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

    public void SetSpawning()
    {
        SetActiveState(Spawning);
    }

    public void StartMoving()
    {
        SetDefaultState();
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

    protected void SetActiveState(AttackerState newState)
    {
        _active = newState;
        StateChanged?.Invoke(Active);
    }

    protected void TrySetTarget(Character character)
    {
        if (character is Defender)
        {
            Attack(character as Defender);
        }
    }

    protected override void TriggerDeathActions()
    {
        Died?.Invoke(this);
    }
}
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AttackerMovement))]
public class Attacker : Character
{
    [SerializeField] private Damage _damage;
    [SerializeField] private Resources _reward;

    private bool _isAttacking = false;
    private bool _isSpawned = false;
    private AttackerMovement _movement;
    private Defender _currentTarget = null;

    public new event UnityAction<Attacker> Died;
    public UnityEvent<bool> AttackStateChanged;

    public bool IsAttacking => _isAttacking;
    public bool IsSpawned => _isSpawned;
    public Damage Damage => _damage;

    private void Awake ()
    {
        Setup();
    }

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
        _isAttacking = true;
        AttackStateChanged?.Invoke(_isAttacking);
        _currentTarget = target;
    }

    private void StrikeCurrentTarget ()
    {
        if (!_currentTarget)
        {
            _isAttacking = false;
            AttackStateChanged?.Invoke(_isAttacking);
        }

        else
        {
            _currentTarget.TakeDamage(Damage);
        }
    }

    public void SetSpawning ()
    {
        StopMoving();
        _isSpawned = false;
    }

    private void SetSpawned ()
    {
        _isSpawned = true;
        StartMoving();
    }

    public void StartMoving ()
    {
        _isAttacking = false;
        AttackStateChanged?.Invoke(_isAttacking);
    }

    public void StopMoving()
    {
        _isAttacking = true;
        AttackStateChanged?.Invoke(_isAttacking);
    }

    private void Setup ()
    {
        _movement = GetComponent<AttackerMovement>();
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

    protected override void TriggerDeathActions()
    {
        Died?.Invoke(this);
    }
}
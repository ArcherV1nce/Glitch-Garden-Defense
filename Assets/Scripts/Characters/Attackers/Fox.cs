using System.Collections;
using UnityEngine;

public class Fox : Attacker
{
    private const float ReloadTimeMin = 0f;
    private const float ReloadTimeMax = 15f;
    private const float InvincibilityTimeMin = 0f;
    private const float InvincibilityTimeMax = 2.5f;

    [SerializeField] private AttackerState _usingSkill;
    [SerializeField, Range(ReloadTimeMin, ReloadTimeMax)] 
    private float _skillReloadTime;
    [SerializeField, Range(InvincibilityTimeMin, InvincibilityTimeMax)] 
    private float _invinsibilityTime;
    [SerializeField] private FoxEvasionSkill _attackDetection;

    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private bool _skillIsReady;
    private Coroutine _reloadingRoutine;
    private Coroutine _jumpRoutine;

    protected override void Awake()
    {
        base.Awake();
        Setup();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SubscribeToEvasionSkill();
    }

    protected void OnDestroy()
    {
        UnsubscribeFromEvasionSkill();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UnsubscribeFromEvasionSkill();
        StopCooldownCoroutine();
        StopJumpRoutine();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        if (_collider == null)
        {
            _collider = GetComponent<Collider2D>();
        }
    }

    private IEnumerator DecreaseSkillCooldown()
    {
        float timer = _skillReloadTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        _skillIsReady = true;
        StopCooldownCoroutine();
    }

    private void TryUseSkill(Projectile projectile)
    {
        if (_skillIsReady && projectile is DefenderProjectile)
        {
            UseSkill();
        }
    }

    private void UseSkill()
    {
        SetActiveState(_usingSkill);
        TriggerSkillNotification();
        Immaterialize();
        _jumpRoutine = StartCoroutine(WaitForMaterialization());
        StartCooldown();
    }

    private void Immaterialize()
    {
        _skillIsReady = false;
        _rigidbody.simulated = false;
        _collider.enabled = false;
    }

    private void Materialize()
    {
        CheckTarget();
        _rigidbody.simulated = true;
        _collider.enabled = true;
    }

    private IEnumerator WaitForMaterialization()
    {
        yield return new WaitForSeconds(_invinsibilityTime);
        yield return new WaitForFixedUpdate();
        Materialize();
        StopJumpRoutine();
    }

    private void StopJumpRoutine()
    {
        if (_jumpRoutine != null)
        {
            StopCoroutine(_jumpRoutine);
            _jumpRoutine = null;
        }
    }

    private void StopCooldownCoroutine()
    {
        if (_reloadingRoutine != null)
        {
            StopCoroutine(_reloadingRoutine);
            _reloadingRoutine = null;
        }
    }

    private void SubscribeToEvasionSkill()
    {
        _attackDetection.EvasionTriggered.AddListener(TryUseSkill);
    }

    private void UnsubscribeFromEvasionSkill()
    {
        _attackDetection.EvasionTriggered.RemoveListener(TryUseSkill);
    }

    private void StartCooldown()
    {
        if (_reloadingRoutine != null)
        {
            StopCooldownCoroutine();
        }

        _reloadingRoutine = StartCoroutine(DecreaseSkillCooldown());
    }

    private void Setup()
    {
        _collider = GetComponent<Collider2D>();
        _skillIsReady = true;
        _rigidbody = GetComponent<Rigidbody2D>();
    }
}
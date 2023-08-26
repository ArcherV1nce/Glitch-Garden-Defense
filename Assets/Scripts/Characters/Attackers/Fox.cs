using System.Collections;
using UnityEngine;

public class Fox : Attacker
{
    [SerializeField] private AttackerState _usingSkill;
    [SerializeField] private float _skillReloadTime;
    [SerializeField] private FoxEvasionSkill _attackDetection;

    private Collider2D _collider;
    private Rigidbody2D _rigidbody;
    private bool _skillIsReady;
    private Coroutine _reloadingRoutine;

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

    protected override void OnDisable()
    {
        base.OnDisable();
        UnsubscribeFromEvasionSkill();
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
        Immaterialize();
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
        _rigidbody.simulated = true;
        _collider.enabled = true;
        CheckTarget();
    }

    private void StopCooldownCoroutine()
    {
        StopCoroutine(_reloadingRoutine);
        _reloadingRoutine = null;
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
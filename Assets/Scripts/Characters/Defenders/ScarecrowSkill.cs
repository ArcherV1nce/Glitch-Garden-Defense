using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Scarecrow))]
public class ScarecrowSkill : MonoBehaviour
{
    private const float RiposteThresholdMin = 0.1f;
    private const float RiposteDamageMultiplierMin = 0.5f;
    private const float RiposteDamageMultiplierMax = 3f;

    [SerializeField] private Scarecrow _defender;
    [SerializeField] private DefenderExplosion _prefab;
    [SerializeField] private float _damageRequiredForRiposte;
    [SerializeField, Range
        (RiposteDamageMultiplierMin, RiposteDamageMultiplierMax)] private float _riposteMultiplier;

    private float _damageTaken;
    private Vector3 _position;

    public event UnityAction<bool> ChargeStatusUpdated;
    public event UnityAction<float> RiposteDamageChanged;

    public bool IsCharged => _damageTaken * _riposteMultiplier >= _damageRequiredForRiposte;

    private void Awake()
    {
        SetPosition();
        ValidateDefender();
    }

    private void OnEnable()
    {
        SubscribeToScarecrow();
    }

    private void OnDisable()
    {
        UnsubscribeFromScarecrow();
    }

    private void OnValidate()
    {
        ValidateRiposteDamageMin();
        ValidateDefender();
    }

    public void SetOwner(Scarecrow owner)
    {
        _defender = owner;
    }

    public void ResetDamage()
    {
        _damageTaken = Damage.NoDamageValue;
        RiposteDamageChanged?.Invoke(GetRiposteDamage(_damageTaken).Value);
    }

    public void CheckRiposteStatus()
    {
        RiposteDamageChanged?.Invoke(GetRiposteDamage(_damageTaken).Value);
        ChargeStatusUpdated?.Invoke(IsCharged);
    }

    private void SetPosition()
    {
        _position = _defender.GetComponent<Transform>().position;
    }

    private void ValidateRiposteDamageMin()
    {
        if (_defender != null)
        {
            if (_damageRequiredForRiposte <= _defender.MaxHealth * RiposteThresholdMin || _damageRequiredForRiposte < _defender.MaxHealth == false)
            {
                _damageRequiredForRiposte = _defender.MaxHealth * RiposteThresholdMin;
            }
        }
    }

    private void ValidateDefender()
    {
        if (_defender == null)
        {
            _defender = GetComponent<Scarecrow>();
        }
    }

    private void AccumulateDamageForAttack(Damage damage)
    {
        _damageTaken += damage.Value;
        RiposteDamageChanged?.Invoke(GetRiposteDamage(_damageTaken).Value);
    }

    private void PerformRiposteExplosion()
    {
        DefenderExplosion explosion = Instantiate(_prefab, _position, Quaternion.identity);
        explosion.SetDamage(GetRiposteDamage(_damageTaken));
        ResetDamage();
        ChargeStatusUpdated?.Invoke(IsCharged);
    }

    private Damage GetRiposteDamage(float damageAccumulated)
    {
        float riposteDamageValue = damageAccumulated * _riposteMultiplier;
        return new Damage((int)riposteDamageValue);
    }

    private void OnDamageTaken(Damage damage)
    {
        AccumulateDamageForAttack(damage);
    }

    private void OnRiposteTriggered()
    {
        PerformRiposteExplosion();
    }

    private void SubscribeToScarecrow()
    {
        _defender.DamageTaken += OnDamageTaken;
        _defender.RiposteTriggered += OnRiposteTriggered;
    }

    private void UnsubscribeFromScarecrow()
    {
        _defender.DamageTaken -= OnDamageTaken;
        _defender.RiposteTriggered -= OnRiposteTriggered;
    }
}
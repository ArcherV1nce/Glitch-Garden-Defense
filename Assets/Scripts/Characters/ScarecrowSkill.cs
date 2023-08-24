using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Scarecrow))]
public class ScarecrowSkill : MonoBehaviour
{
    private const float RiposteThresholdMin = 0.1f;
    private const float RiposteDamageMultiplierMax = 3f;

    [SerializeField] private Scarecrow _defender;
    [SerializeField] private ScarecrowRiposteButton _button;
    [SerializeField] private ScarecrowRiposte _prefab;
    [SerializeField] private float _damageRequiredForRiposte;
    [SerializeField, Range(0, RiposteDamageMultiplierMax)] private float _riposteMultiplier;

    private float _damageTaken;
    private Vector3 _position;

    public UnityEvent<bool> ChargeStatusUpdated;
    public bool IsCharged => _damageTaken >= _damageRequiredForRiposte;

    private void Awake()
    {
        SetPosition();
        ValidateDefender();
    }

    private void OnEnable()
    {
        SubscribeToScarecrow();
        SubscribeToRiposteButton();
    }

    private void OnDisable()
    {
        UnsubscribeFromScarecrow();
        UnsubscribeFromRiposteButton();
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
        _damageTaken = 0;
        SetDamage(new Damage(Damage.NoDamageValue));
    }

    public void CheckRiposteStatus()
    {
        ChargeStatusUpdated?.Invoke(IsCharged);
    }

    private void SetPosition()
    {
        _position = _defender.GetComponent<Transform>().position;
    }

    private void SetDamage(Damage damage)
    {
        _prefab.SetDamage(damage);
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
    }

    private void PerformRiposteExplosion()
    {
        ScarecrowRiposte explosion = Instantiate(_prefab, _position, Quaternion.identity);
        explosion.SetDamage(GetRiposteDamage(_damageTaken));
        ResetDamage();
        ChargeStatusUpdated?.Invoke(IsCharged);
    }

    private Damage GetRiposteDamage(float damageAccumulated)
    {
        float riposteDamageValue = damageAccumulated * _riposteMultiplier;
        return new Damage((int)riposteDamageValue);
    }

    private void SubscribeToRiposteButton()
    {
        _button.OnDefenderClicked += PerformRiposteExplosion;
    }

    private void UnsubscribeFromRiposteButton()
    {
        _button.OnDefenderClicked -= PerformRiposteExplosion;
    }

    private void SubscribeToScarecrow()
    {
        _defender.DamageTaken.AddListener(AccumulateDamageForAttack);
    }

    private void UnsubscribeFromScarecrow()
    {
        _defender.DamageTaken.RemoveListener(AccumulateDamageForAttack);
    }
}
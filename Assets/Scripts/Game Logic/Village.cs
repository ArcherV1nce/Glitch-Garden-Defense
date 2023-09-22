using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(PlayerResources))]
public class Village : MonoBehaviour
{
    [SerializeField] private DefenderSpawner _defenderSpawner;
    [SerializeField] private Health _health;

    private PlayerResources _resources;
    private Collider2D _trigger;

    public event UnityAction HealthChanged;
    public event UnityAction Destroyed;

    public int Health => _health.Value;
    public int HealthMax => _health.ValueMax;

    private void Awake()
    {
        Setup();
    }

    private void OnValidate()
    {
        ValidateTrigger();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckTrespasser(collision);
    }

    public void AddResources (Resources resources)
    {
        _resources.AddResources(resources);
    }

    private void CheckTrespasser(Collider2D trespasser)
    {
        if (trespasser.TryGetComponent(out Attacker attacker))
        {
            DealWithAttacker(attacker);
        }
    }

    private void DealWithAttacker (Attacker attacker)
    {
        _health.ApplyDamage(attacker.Damage);
        HealthChanged?.Invoke();
        attacker.Die();

        if (_health.IsAlive == false)
        {
            Destroyed?.Invoke();
        }
    }

    private void Setup()
    {
        ValidateTrigger();
        _resources = GetComponent<PlayerResources>();
    }

    private void ValidateTrigger()
    {
        if (_trigger == null)
            _trigger = GetComponent<Collider2D>();

        if (_trigger.isTrigger == false)
            _trigger.isTrigger = true;
    }
}
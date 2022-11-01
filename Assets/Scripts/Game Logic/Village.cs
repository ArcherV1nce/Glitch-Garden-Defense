using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(PlayerResources))]
public class Village : MonoBehaviour
{
    [SerializeField] private DefenderSpawner _defenderSpawner;
    [SerializeField] private Health _health;

    private Collider2D _trigger;

    public UnityEvent VillageDestroyed;

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
        attacker.Die();

        if (_health.IsAlive == false)
        {
            VillageDestroyed?.Invoke();
        }
    }

    private void Setup()
    {
        ValidateTrigger();
    }

    private void ValidateTrigger()
    {
        if (_trigger == null)
            _trigger = GetComponent<Collider2D>();

        if (_trigger.isTrigger == false)
            _trigger.isTrigger = true;
    }
}
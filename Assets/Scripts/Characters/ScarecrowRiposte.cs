using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ScarecrowRiposte : MonoBehaviour
{
    private const float LifeTimeMin = 0.1f;

    [SerializeField] private Collider2D _trigger;
    [SerializeField] private float _lifetime;

    private Damage _damage;
    private List<Attacker> _enemies;

    private void Awake()
    {
        Setup();
    }

    private void OnValidate()
    {
        ValidateLifetime();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckExplosionAreaForEnemies(collision);
    }

    public void SetDamage(Damage damage)
    {
        _damage = damage;
    }

    private void Setup()
    {
        _enemies = new List<Attacker>();
        _trigger = GetComponent<Collider2D>();

        if (_trigger.isTrigger == false)
        {
            _trigger.isTrigger = true;
        }
    }

    private void CheckExplosionAreaForEnemies(Collider2D collision)
    {
        if (collision.TryGetComponent<Attacker>(out Attacker enemy))
        {
            AddEnemyToList(enemy);
        }
    }

    private void AddEnemyToList(Attacker attacker)
    {
        _enemies.Add(attacker);
    }

    private void DealDamageToEnemies()
    {
        foreach (Attacker enemy in _enemies)
        {
            enemy.TakeDamage(_damage);
        }
    }

    private void PerformRiposteExplosion()
    {
        DealDamageToEnemies();
        Destroy(this, _lifetime);
    }

    private void ValidateLifetime()
    {
        if (_lifetime < LifeTimeMin)
        {
            _lifetime = LifeTimeMin;
        }
    }
}
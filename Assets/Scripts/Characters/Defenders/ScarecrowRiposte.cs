using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ScarecrowRiposte : MonoBehaviour
{
    private const float ExplosionSizeMin = 0f;
    private const float ExplosionSizeMax = 5f;

    private const float LifeTimeMin = 0.1f;

    [SerializeField] private Vector2 _size;
    [SerializeField] private float _lifetime;

    private BoxCollider2D _trigger;
    private Damage _damage;
    private List<Attacker> _enemies;

    private void Awake()
    {
        Setup();
    }

    private void OnValidate()
    {
        ValidateTrigger();
        ValidateLifetime();
        ValidateSizeValues();
        UpdateTriggerSize();
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

        ValidateTrigger();
        UpdateTriggerSize();
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
        Destroy(this.gameObject, _lifetime);
    }

    private void ValidateLifetime()
    {
        if (_lifetime < LifeTimeMin)
        {
            _lifetime = LifeTimeMin;
        }
    }

    private void ValidateSizeValues()
    {
        if (_size.x > ExplosionSizeMax)
        {
            _size.x = ExplosionSizeMax;
        }
        else if (_size.x < ExplosionSizeMin)
        {
            _size.x = ExplosionSizeMin;
        }

        if (_size.y > ExplosionSizeMax)
        {
            _size.y = ExplosionSizeMax;
        }
        else if (_size.y < ExplosionSizeMin)
        {
            _size.y = ExplosionSizeMin;
        }
    }

    private void ValidateTrigger()
    {
        _trigger = GetComponent<BoxCollider2D>();

        if (_trigger.isTrigger == false)
        {
            _trigger.isTrigger = true;
        }
    }

    private void UpdateTriggerSize()
    {
        _trigger.size = _size;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class DefenderRiposte : MonoBehaviour
{
    private const float LifeTimeMin = 0.05f;

    [SerializeField] private Collider2D _trigger;
    [SerializeField] private float _lifetime;

    private Scarecrow _owner;
    private Damage _damage;
    private List<Attacker> _enemies;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToOwner();
    }

    private void OnDisable()
    {
        UnsubscribeFromOwner();
    }

    private void OnValidate()
    {
        ValidateLifetime();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckExplosionAreaForEnemies(collision);
    }

    public void SetOwner(Scarecrow owner)
    {
        _owner = owner;
    }

    private void Setup()
    {
        _enemies = new List<Attacker>();
    }

    private void SetDamage(Damage damage)
    {
        _damage = damage;
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

    private void SubscribeToOwner()
    {
         _owner.RiposteDamageUpdated.AddListener(SetDamage);
    }

    private void UnsubscribeFromOwner()
    {
        _owner.RiposteDamageUpdated.RemoveListener(SetDamage);
    }
}
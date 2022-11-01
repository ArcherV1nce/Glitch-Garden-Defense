using UnityEngine;
using UnityEngine.Events;

public class Cactus : Defender
{
    [SerializeField] private Transform _projectilePosition;
    [SerializeField] private DefenderProjectile _projectile;
    [SerializeField] private float _shootingDelay;

    public UnityEvent AttackStateChanged;

    public Cactus (Resources price) : base (price) { }
    public bool IsShooting => IsAttacked;

    public override void SetAttacked()
    {
        base.SetAttacked();
        AttackStateChanged?.Invoke();
    }

    public override void SetIdle()
    {
        base.SetIdle();
        AttackStateChanged?.Invoke();
    }

    private void Shoot()
    {
        Instantiate(_projectile, _projectilePosition.position, Quaternion.identity);
    }
}
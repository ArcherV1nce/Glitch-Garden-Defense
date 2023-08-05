using UnityEngine;
using UnityEngine.Events;

public class ShootingDefender : Defender
{
    [SerializeField] private Transform _projectilePosition;
    [SerializeField] private DefenderProjectile _projectile;
    [SerializeField] private float _shootingDelay;

    private float _shootingReloadTime;

    public UnityEvent AttackStateChanged;

    public ShootingDefender(Resources price) : base(price) { }

    public bool IsShooting => IsAttacked;
    public bool IsReadyToShoot => _shootingReloadTime <= 0;

    private void Update()
    {
        if (!IsReadyToShoot)
        {
            _shootingReloadTime -= Time.deltaTime;
        }
    }

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
        if (IsReadyToShoot)
        {
            Instantiate(_projectile, _projectilePosition.position, Quaternion.identity);
            _shootingReloadTime = _shootingDelay;
        }
    }
}

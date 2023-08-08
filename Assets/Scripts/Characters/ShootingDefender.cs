using UnityEngine;
using UnityEngine.Events;

public class ShootingDefender : Defender
{
    [SerializeField] private Transform _projectilePosition;
    [SerializeField] private DefenderProjectile _projectile;
    [SerializeField] private float _shootingDelay;
    [SerializeField] private DefenderState _attacked;

    private float _shootingReloadTime;

    public UnityEvent <DefenderState> AttackStateChanged;

    public ShootingDefender(Resources price) : base(price) { }

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
        SetActiveState(_attacked);
        AttackStateChanged?.Invoke(Active);
    }

    public override void SetIdle()
    {
        SetDefaultState();
        AttackStateChanged?.Invoke(Active);
    }

    private void Shoot()
    {
        if (IsReadyToShoot)
        {
            Instantiate(_projectile, _projectilePosition.position, Quaternion.identity);
            _shootingReloadTime = _shootingDelay;
        }
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        if (_attacked != null && _attacked.Character != this.GetComponent<ShootingDefender>())
        {
            _attacked = null;
        }
    }
}
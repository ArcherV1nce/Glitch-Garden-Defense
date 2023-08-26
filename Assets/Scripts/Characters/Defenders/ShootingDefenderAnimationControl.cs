using UnityEngine;

[RequireComponent(typeof(ShootingDefender))]
public class ShootingDefenderAnimationControl : DefenderAnimationControl
{
    private ShootingDefender _shooter;

    private void OnEnable()
    {
        SubscribeToShootingDefender();
    }

    private void OnDisable()
    {
        UnsubscribeFromShootingDefender();
    }

    protected override void Setup()
    {
        base.Setup();
        _shooter = GetComponent<ShootingDefender>();
    }

    private void SubscribeToShootingDefender()
    {
        _shooter.AttackStateChanged.AddListener(UpdateStates);
    }

    private void UnsubscribeFromShootingDefender()
    {
        _shooter.AttackStateChanged.RemoveListener(UpdateStates);
    }
}
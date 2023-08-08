using UnityEngine;

[RequireComponent(typeof(ShootingDefender))]
public class ShootingDefenderAnimationControl : AnimationControl
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

    public void UpdateStates(DefenderState newState)
    {
        foreach (DefenderStateParameter stateParameter in newState.Parameters)
        {
            Animator.SetBool(stateParameter.Name.ToString(), stateParameter.State);
            Debug.Log($"Defender {this.name} entered state {newState.name} with parameter {stateParameter.Name} set to {stateParameter.State}");
        }
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
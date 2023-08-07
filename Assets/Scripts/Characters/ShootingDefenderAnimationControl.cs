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

    public void UpdateStates(CharacterState newState)
    {
        foreach (StateParameter stateParameter in newState.StateParameters)
        {
            Animator.SetBool(stateParameter.ParameterName, stateParameter.ParameterState);
            Debug.Log($"Defender {this.name} entered state {newState.name} with parameter {stateParameter.ParameterName} set to {stateParameter.ParameterState}");
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
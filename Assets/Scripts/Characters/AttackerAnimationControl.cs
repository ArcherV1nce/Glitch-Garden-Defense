using UnityEngine;

[RequireComponent(typeof(Attacker))]
public class AttackerAnimationControl : AnimationControl
{
    private const string SpawningState = "finishedSpawning";
    private const string AttackingState = "isAttacking";

    private Attacker _attacker;

    private void OnEnable()
    {
        SubscribeToAttackerStates();
    }

    private void OnDisable()
    {
        UnsubscribeFromAttackerStates();
    }

    public void UpdateStates(CharacterState newState)
    {
        foreach (StateParameter stateParameter in newState.StateParameters)
        {
            Animator.SetBool(stateParameter.ParameterName, stateParameter.ParameterState);
            Debug.Log($"Attacker {this.name} entered state {newState.name} with parameter {stateParameter.ParameterName} set to {stateParameter.ParameterState}");
        }
    }

    protected override void Setup()
    {
        base.Setup();
        _attacker = GetComponent<Attacker>();
    }

    private void SubscribeToAttackerStates()
    {
        _attacker.StateChanged.AddListener(UpdateStates);
    }

    private void UnsubscribeFromAttackerStates()
    {
        _attacker.StateChanged.RemoveListener(UpdateStates);
    }
}
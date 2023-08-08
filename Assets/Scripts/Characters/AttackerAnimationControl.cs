using UnityEngine;

[RequireComponent(typeof(Attacker))]
public class AttackerAnimationControl : AnimationControl
{
    private Attacker _attacker;

    private void OnEnable()
    {
        SubscribeToAttackerStates();
    }

    private void OnDisable()
    {
        UnsubscribeFromAttackerStates();
    }

    public void UpdateStates(AttackerState newState)
    {
        foreach (AttackerStateParameter stateParameter in newState.Parameters)
        {
            Animator.SetBool(stateParameter.Name.ToString(), stateParameter.State);
            Debug.Log($"Attacker {this.name} entered state {newState.name} with parameter {stateParameter.Name} set to {stateParameter.State}");
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
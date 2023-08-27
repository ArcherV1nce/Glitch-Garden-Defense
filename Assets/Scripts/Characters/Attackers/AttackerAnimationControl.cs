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

    public void OnStateChanged(AttackerState newState)
    {
        UpdateStates(newState);
    }

    private void UpdateStates(AttackerState newState)
    {
        foreach (AttackerStateParameter stateParameter in newState.Parameters)
        {
            Animator.SetBool(stateParameter.Name.ToString(), stateParameter.State);
        }
    }

    protected override void Setup()
    {
        base.Setup();
        _attacker = GetComponent<Attacker>();
    }

    private void SubscribeToAttackerStates()
    {
        _attacker.StateChanged += OnStateChanged;
    }

    private void UnsubscribeFromAttackerStates()
    {
        _attacker.StateChanged -= OnStateChanged;
    }
}
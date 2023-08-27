using UnityEngine;

[RequireComponent(typeof(Attacker))]
public class AttackerMovement : Movement
{
    protected Attacker Attacker;

    private void Awake()
    {
        Setup();
    }

    public void OnStateChanged(AttackerState state)
    {
        UpdateMovementState(state);
    }

    private void SubscribeToAttackerStates()
    {
        Attacker.StateChanged += OnStateChanged;
    }

    private void UnsubscribeFromAttackerStates()
    {
        Attacker.StateChanged -= OnStateChanged;
    }

    protected override void OnEnable()
    {
        SubscribeToAttackerStates();
    }

    protected override void OnDisable()
    {
        UnsubscribeFromAttackerStates();
    }

    protected override void Setup ()
    {
        Attacker = GetComponent<Attacker>();
    }

    protected virtual void UpdateMovementState(AttackerState state)
    {
        bool movementState = false;

        foreach (AttackerStateParameter parameter in state.Parameters)
        {
            if (parameter.Name == AttackerStateParameter.AttackerStateParameters.IsMoving.ToString())
            {
                movementState = parameter.State;
                break;
            }
        }

        SetMoving(movementState);
    }

    protected override void SetMoving (bool shouldMove)
    {
        base.SetMoving(shouldMove);

        if (IsMoving)
        {
            StartMovement();
        }

        else if (IsMoving == false)
        {
            StopMovement();
        }
    }
}
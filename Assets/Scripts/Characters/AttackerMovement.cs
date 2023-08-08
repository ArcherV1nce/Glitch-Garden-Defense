using UnityEngine;

[RequireComponent(typeof(Attacker))]
public class AttackerMovement : Movement
{
    private Attacker _attacker;

    private void Awake()
    {
        Setup();
    }

    private void UpdateMovementState(AttackerState state)
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
        
        SetMovementState(movementState);
    }

    private void SubscribeToAttackerStates()
    {
        _attacker.StateChanged.AddListener(UpdateMovementState);
    }

    private void UnsubscribeFromAttackerStates()
    {
        _attacker.StateChanged.RemoveListener(UpdateMovementState);
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
        _attacker = GetComponent<Attacker>();
    }

    protected override void SetMovementState (bool shouldMove)
    {
        base.SetMovementState(shouldMove);

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
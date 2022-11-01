using UnityEngine;

[RequireComponent(typeof(Attacker))]
public class AttackerMovement : Movement
{
    private Attacker _attacker;

    private void Awake()
    {
        Setup();
    }

    protected override void OnEnable()
    {
        _attacker.AttackStateChanged.AddListener(SetMovementState);
    }

    protected override void OnDisable()
    {
        _attacker.AttackStateChanged.RemoveListener(SetMovementState);
    }

    protected override void Setup ()
    {
        _attacker = GetComponent<Attacker>();
    }

    protected override void SetMovementState (bool isAttacking)
    {
        base.SetMovementState(isAttacking == false);

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
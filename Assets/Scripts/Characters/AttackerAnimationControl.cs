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

    public void UpdateAttackingState(bool isAttacking)
    {
        Animator.SetBool(AttackingState, isAttacking);
    }

    protected override void Setup()
    {
        base.Setup();
        _attacker = GetComponent<Attacker>();
    }

    private void SetSpawningState()
    {
        Animator.SetBool(SpawningState, _attacker.IsSpawned);
    }

    private void SubscribeToAttackerStates()
    {
        _attacker.AttackStateChanged.AddListener(UpdateAttackingState);
    }

    private void UnsubscribeFromAttackerStates()
    {
        _attacker.AttackStateChanged.RemoveListener(UpdateAttackingState);
    }
}
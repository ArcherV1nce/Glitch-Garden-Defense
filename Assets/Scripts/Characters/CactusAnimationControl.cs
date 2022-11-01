using UnityEngine;

[RequireComponent(typeof(Cactus))]
public class CactusAnimationControl : AnimationControl
{
    private const string ShootingState = "isShooting";

    private Cactus _cactus;

    private void OnEnable()
    {
        SubscribeToCactus();
    }

    private void OnDisable()
    {
        UnsubscribeFromCactus();
    }

    public void UpdateStates()
    {
        Animator.SetBool(ShootingState, _cactus.IsShooting);
    }

    protected override void Setup()
    {
        base.Setup();
        _cactus = GetComponent<Cactus>();
    }

    private void SubscribeToCactus()
    {
        _cactus.AttackStateChanged.AddListener(UpdateStates);
    }

    private void UnsubscribeFromCactus()
    {
        _cactus.AttackStateChanged.RemoveListener(UpdateStates);
    }
}
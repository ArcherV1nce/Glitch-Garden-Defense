using UnityEngine;

[RequireComponent(typeof(Scarecrow))]
public class ScarecrowAnimationControl : DefenderAnimationControl
{
    private Scarecrow _defender;

    private void OnEnable()
    {
        SubscribeToScarecrow();
    }

    private void OnDisable()
    {
        UnsubscribeFromScarecrow();
    }

    protected override void Setup()
    {
        base.Setup();
        _defender = GetComponent<Scarecrow>();
    }

    private void SubscribeToScarecrow()
    {
        _defender.StateChanged.AddListener(UpdateStates);
    }

    private void UnsubscribeFromScarecrow()
    {
        _defender.StateChanged.RemoveListener(UpdateStates);
    }
}
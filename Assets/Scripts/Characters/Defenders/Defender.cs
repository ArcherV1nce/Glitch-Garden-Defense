using UnityEngine;
using UnityEngine.Events;

public class Defender : Character
{
    [SerializeField] private Resources _price;
    
    [SerializeField] protected DefenderState Default;

    private DefenderState _active;

    public event UnityAction<Defender> Spawned;
    public new event UnityAction<Defender> Died;

    public DefenderState Active => _active;
    public Resources Price => _price;

    public Defender (Resources price)
    {
        _price = price;
    }

    protected virtual void Start()
    {
        AlertAboutSpawn();
        SetStartingState();
    }

    public virtual void SetAttacked()
    {

    }

    public virtual void UseSkill()
    {

    }

    public virtual void SetIdle ()
    {
        SetDefaultState();
    }

    public override void SetDefaultState()
    {
        SetActiveState(Default);
    }

    public override void SetStartingState()
    {
        SetDefaultState();
    }

    private void AlertAboutSpawn()
    {
        Spawned?.Invoke(this);
    }

    protected override void TriggerDeathActions()
    {
        Died?.Invoke(this);
    }

    protected void SetActiveState (DefenderState newState)
    {
        _active = newState;
    }
}
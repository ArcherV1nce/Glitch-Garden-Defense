using UnityEngine;
using UnityEngine.Events;

public class Defender : Character
{
    [SerializeField] private Resources _price;
    
    private bool _isAttacked;

    public new event UnityAction<Defender> Died;

    public Resources Price => _price;
    public bool IsAttacked => _isAttacked;

    public Defender (Resources price)
    {
        _price = price;
    }

    public virtual void SetAttacked()
    {
        _isAttacked = true;
    }

    public virtual void SetIdle()
    {
        _isAttacked = false;
    }

    protected override void TriggerDeathActions()
    {
        Died?.Invoke(this);
    }
}
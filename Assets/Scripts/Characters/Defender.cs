using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Defender : Character
{
    [SerializeField] private Resources _price;

    public event UnityAction<Defender> Spawned;
    public new event UnityAction<Defender> Died;

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

    public virtual void SetIdle ()
    {
        SetDefaultState();
    }

    protected virtual void OnValidate()
    {
        if (Active != null)
        {
            if (Active.Character != this.GetComponent<Character>())
            {
                Default = null;
            }
        }
    }

    protected override void TriggerDeathActions()
    {
        Died?.Invoke(this);
    }

    private void AlertAboutSpawn()
    {
        Spawned?.Invoke(this);
    }
}
using System;
using UnityEngine;

[Serializable]
public class AttackerStateParameter : StateParameter
{
    [SerializeField] private AttackerStateParameters _name;

    public string Name => _name.ToString();
    
    public enum AttackerStateParameters
    {
        Default,
        IsMoving,
        IsAttacking,
        FinishedSpawning
    }

    public AttackerStateParameter(AttackerStateParameters name, bool state) : base (state)
    {
        _name = name;
    }

    public AttackerStateParameter()
    {
        _name = AttackerStateParameters.Default;
    }
}
using System;
using UnityEngine;

[Serializable]
public class DefenderStateParameter : StateParameter
{
    [SerializeField] private DefenderStateParameters _name;

    public string Name => _name.ToString();

    public enum DefenderStateParameters
    {
        Default,
        IsAttacking,
        SkillIsReady
    }

    public DefenderStateParameter(DefenderStateParameters name, bool state) : base(state)
    {
        _name = name;
    }

    public DefenderStateParameter() : base()
    {
        _name = DefenderStateParameters.Default;
    }
}
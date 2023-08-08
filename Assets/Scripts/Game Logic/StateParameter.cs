using System;
using UnityEngine;

[Serializable]
public abstract class StateParameter
{
    [SerializeField] protected bool ParameterState;

    public bool State => ParameterState;

    public StateParameter (bool state)
    {
        ParameterState = state;
    }

    public StateParameter()
    {
        ParameterState = false;
    }
}
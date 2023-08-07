using System;
using UnityEngine;

[Serializable]
public class StateParameter
{
    [SerializeField] private string _parameterName;
    [SerializeField] private bool _parameterState;

    public string ParameterName => _parameterName;
    public bool ParameterState => _parameterState;

    public StateParameter (string parameterName, bool parameterState)
    {
        _parameterName = parameterName;
        _parameterState = parameterState;
    }

    public StateParameter()
    {
        _parameterName = "Default Parameter";
        _parameterState = false;
    }
}

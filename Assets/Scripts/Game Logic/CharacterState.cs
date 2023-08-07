using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "CharacterState")]
public class CharacterState : ScriptableObject
{
    [SerializeField] private Character _character;
    [SerializeField] private List<StateParameter> _stateParameters;

    public Character Character => _character;
    public List<StateParameter> StateParameters => _stateParameters;
}
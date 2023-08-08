using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Defender State", menuName = "CharacterStates/DefenderState")]
public class DefenderState : CharacterState
{
    [SerializeField] private Defender _defender;
    [SerializeField] private List<DefenderStateParameter> _parameters;

    public List<DefenderStateParameter> Parameters => _parameters;
    public Character Character => _defender;
}

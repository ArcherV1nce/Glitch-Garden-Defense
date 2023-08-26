using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Defender State", menuName = "CharacterStates/DefenderState")]
public class DefenderState : CharacterState
{
    [SerializeField] private List<DefenderStateParameter> _parameters;

    public List<DefenderStateParameter> Parameters => _parameters;
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attacker State", menuName = "CharacterStates/AttackerState")]
public class AttackerState : CharacterState
{
    [SerializeField] private Attacker _attacker;
    [SerializeField] private List<AttackerStateParameter> _parameters;

    public List<AttackerStateParameter> Parameters => _parameters;
    public Character Character => _attacker;
}
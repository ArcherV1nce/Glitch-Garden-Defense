using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    [SerializeField] private int _attackersCount;
    [SerializeField] private List<Attacker> _attackerPrefabs;

    public int AttackersCount => _attackersCount;
    public List<Attacker> AttackersPrefabs => _attackerPrefabs;
}
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    private const float DelayMin = 0f;
    private const float DelayMax = 60f;

    [SerializeField] private int _attackersCount;
    [SerializeField] private List<Attacker> _attackerPrefabs;
    [SerializeField, Range(DelayMin, DelayMax)] private float _spawnDelay;

    public int AttackersCount => _attackersCount;
    public float SpawnDelay => _spawnDelay;
    public List<Attacker> AttackersPrefabs => _attackerPrefabs;
}
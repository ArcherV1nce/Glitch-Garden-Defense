using System;
using UnityEngine;

[Serializable]
public class DefenderSpawnLimit
{
    public const int SpawnLimitMin = 1;
    public const int SpawnLimitMax = 12;

    [SerializeField] private Defender _defender;
    [SerializeField] private int _spawnLimit;

    public int Spawned { get; private set; }
    public Defender Defender => _defender;
    public int SpawnLimit => _spawnLimit;

    public DefenderSpawnLimit()
    {
        _defender = null;
        _spawnLimit = SpawnLimitMin;
    }

    public DefenderSpawnLimit(Defender defender)
    {
        _defender = defender;
        _spawnLimit = SpawnLimitMax;
    }

    public DefenderSpawnLimit(Defender defender, int spawnLimit)
    {
        _defender = defender;

        if (spawnLimit < SpawnLimitMin)
            _spawnLimit = SpawnLimitMin;
        if (spawnLimit > SpawnLimitMax)
            _spawnLimit = SpawnLimitMax;
        else
            _spawnLimit = spawnLimit;
    }

    public void DefenderSpawned()
    {
        Spawned++;
    }

    public void DefenderRemoved()
    {
        Spawned--;

        if (Spawned < SpawnLimitMin)
            Spawned = SpawnLimitMin; 
    }
}
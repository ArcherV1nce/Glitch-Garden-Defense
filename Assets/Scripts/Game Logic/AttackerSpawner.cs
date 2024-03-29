﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackerSpawner : MonoBehaviour
{
    private const int FirstWaveNumber = 0;

    [SerializeField] private Line _line;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private Wave _currentWave;
    
    private System.Random _random;
    private Coroutine _spawningCoroutine;
    private bool _isSpawning;
    private int _activeWaveNumber;
    private int _remainingAttackers;

    public event UnityAction WaveFinished;
    public event UnityAction Stopped;

    public bool FinishedSpawning => _isSpawning == false;
    public bool HasMoreWaves => _activeWaveNumber < _waves.Count;

    private void OnEnable()
    {
        SetupRandom();
        SetupFirstWave();
        StartSpawning();
    }

    private void OnDisable()
    {
        DisableSpawning();
    }

    public string GetLineName()
    {
        return _line.name;
    }

    public void StartSpawning()
    {
        if (_spawningCoroutine != null)
        {
            return;
        }

        _isSpawning = true;
        _spawningCoroutine = StartCoroutine(SpawnAtacker());
    }

    public void StopSpawning ()
    {
        DisableSpawning();
    }

    public void UpdateWaveData()
    {
        if (_waves.Count > _activeWaveNumber)
        {
            _currentWave = _waves[_activeWaveNumber];
            _remainingAttackers = _currentWave.AttackersCount;
            StartSpawning();
        }

        else
        {
            Stopped?.Invoke();
        }
    }

    private bool TryGetAttackerFromCurrentWave(out Attacker currentAttacker)
    {
        currentAttacker = _currentWave.AttackersPrefabs[_random.Next(0, _currentWave.AttackersPrefabs.Count)];

        if (currentAttacker != null)
        {
            return true;
        }

        else return false;
    }

    private IEnumerator SpawnAtacker()
    {
        while (_isSpawning && _remainingAttackers > 0)
        {
            if (TryGetAttackerFromCurrentWave(out Attacker attackerPrefab))
            {
                Attacker attacker = Instantiate(attackerPrefab, transform.position, transform.rotation);
                attacker.transform.parent = _line.transform;
                _line.AddCharacter(attacker);
                _remainingAttackers--;
            }

            if (_remainingAttackers <= 0)
            {
                _activeWaveNumber++;
                _isSpawning = false;
            }

            yield return new WaitForSeconds(_currentWave.SpawnDelay);
        }

        DisableSpawning();
    }

    private void DisableSpawning()
    {
        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
            WaveFinished?.Invoke();
        }
    }

    private void SetupRandom()
    {
        _random ??= new System.Random();
    }

    private void SetupFirstWave()
    {
        _activeWaveNumber = FirstWaveNumber;

        if (_currentWave == null)
        {
            Debug.LogError($"Spawner {this.name} has no Wave Scriptable object set.");
        }

        else
        {
            _currentWave = _waves[_activeWaveNumber];
            _remainingAttackers = _currentWave.AttackersCount;
        }
    }
}
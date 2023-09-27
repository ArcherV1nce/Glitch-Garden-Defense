using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackerSpawner : MonoBehaviour
{
    private const int FirstWaveNumber = 0;
    private const int NoWaves = 0;
    private const int NoAttackers = 0;

    [SerializeField] private Line _line;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private Wave _currentWave;
    
    private System.Random _random;
    private Coroutine _spawningCoroutine;
    private bool _isSpawning;
    private int _activeWaveNumber;
    private int _remainingAttackers;

    public event UnityAction<Attacker> AttackerSpawned;
    public event UnityAction WaveFinished;
    public event UnityAction Stopped;

    public bool FinishedSpawning => _isSpawning == false;
    public bool HasMoreWaves => _activeWaveNumber < _waves.Count && _waves.Count > NoWaves;

    protected virtual void Awake()
    {
        ValidateWaves();
    }

    protected virtual void OnEnable()
    {
        SetupRandom();
        SetupFirstWave();
        StartSpawning();
    }

    protected virtual void OnDisable()
    {
        DisableSpawning();
    }

    private void OnValidate()
    {
        ValidateWaves();
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

        if (_waves.Count > NoWaves)
        {
            _isSpawning = true;
            _spawningCoroutine = StartCoroutine(SpawnAtackers());
        }

        else if (_waves.Count == NoWaves)
        {
            _isSpawning= false;
            _remainingAttackers = NoAttackers;
            Stopped?.Invoke();
        }
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

    protected void DecreaseRemainingAttackersCount()
    {
        if (_remainingAttackers > NoAttackers)
        {
            _remainingAttackers--;
        }
    }

    protected void DisableSpawning()
    {
        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
            
            if (_remainingAttackers <= NoAttackers)
            {
                WaveFinished?.Invoke();
            }

            if (_waves.Count == _activeWaveNumber)
            {
                Stopped?.Invoke();
            }
        }
    }

    protected virtual IEnumerator SpawnAtackers()
    {
        while (_isSpawning && _remainingAttackers > NoAttackers)
        {
            if (TryGetAttackerFromCurrentWave(out Attacker attackerPrefab))
            {
                Attacker attacker = Instantiate(attackerPrefab, transform.position, transform.rotation);
                attacker.transform.parent = _line.transform;
                _line.AddCharacter(attacker);
                DecreaseRemainingAttackersCount();
                AttackerSpawned?.Invoke(attacker);
            }

            if (_remainingAttackers <= NoAttackers)
            {
                _activeWaveNumber++;
                _isSpawning = false;
            }

            yield return new WaitForSeconds(_currentWave.SpawnDelay);
        }

        DisableSpawning();
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

    private void ValidateWaves()
    {
        _waves ??= new List<Wave>();
    }

    private void SetupRandom()
    {
        _random ??= new System.Random();
    }

    private void SetupFirstWave()
    {
        _activeWaveNumber = FirstWaveNumber;

        if (_waves.Count > NoWaves)
        {
            _currentWave = _waves[_activeWaveNumber];
            _remainingAttackers = _currentWave.AttackersCount;
        }

        if (_currentWave == null && _waves.Count > NoWaves)
        {
            Debug.LogError($"Spawner {this.name} has no Wave Scriptable object set.");
        }
    }
}
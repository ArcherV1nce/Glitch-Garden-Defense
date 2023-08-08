using System.Collections;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    private const float SpawnDelayMin = 0.5f;
    private const float SpawnDelayMax = 5f;

    [SerializeField] private Line _line;
    [SerializeField] private Wave _currentWave;
    [SerializeField] private float _spawnDelay;

    private System.Random _random;
    private Coroutine _spawningCoroutine;
    private bool _isSpawning;
    private int _remainingAttackers;

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

    private void OnValidate()
    {
        ValidateDelay();
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
        _isSpawning = false;
        StopCoroutine(_spawningCoroutine);
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
                _isSpawning = false;
            }

            yield return new WaitForSeconds(_spawnDelay);
        }

        DisableSpawning();
    }

    private void DisableSpawning()
    {
        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }
    }

    private void SetupRandom()
    {
        _random ??= new System.Random();
    }

    private void SetupFirstWave()
    {
        if (_currentWave == null)
        {
            Debug.LogError($"Spawner {this.name} has no Wave Scriptable object set.");
        }

        else
        {
            _remainingAttackers = _currentWave.AttackersCount;
        }
    }

    private void ValidateDelay()
    {
        if (_spawnDelay > SpawnDelayMax)
        {
            _spawnDelay = SpawnDelayMax;
        }

        else if (_spawnDelay < SpawnDelayMin)
        {
            _spawnDelay = SpawnDelayMin;
        }
    }
}
using System.Collections;
using UnityEngine;

public class AttackerSpawner : MonoBehaviour
{
    private const float SpawnDelayMin = 0.5f;
    private const float SpawnDelayMax = 5f;

    [SerializeField] private Line _line;
    [SerializeField] private Attacker atackerPrefab;
    [SerializeField] private float _spawnDelay;

    private Coroutine _spawningCoroutine;
    private bool _isSpawning;

    private void OnEnable()
    {
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

    private IEnumerator SpawnAtacker()
    {
        while (_isSpawning)
        {
            if (atackerPrefab != null)
            {
                Attacker attacker = Instantiate(atackerPrefab, transform.position, transform.rotation);
                attacker.transform.parent = _line.transform;
                _line.AddCharacter(attacker);
            }

            yield return new WaitForSeconds(_spawnDelay);
        }

        StopCoroutine(_spawningCoroutine);
    }

    private void DisableSpawning()
    {
        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
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
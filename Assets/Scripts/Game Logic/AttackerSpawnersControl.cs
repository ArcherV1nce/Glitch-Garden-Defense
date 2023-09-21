using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackerSpawnersControl : MonoBehaviour
{
    private const float WaveSwitchDelayMin = 0f;
    private const float WaveSwitchDelayMax = 60f;

    [SerializeField, Range(WaveSwitchDelayMin, WaveSwitchDelayMax)] 
    private float _waveSwitchDelay;
    [SerializeField] private List<AttackerSpawner> _spawners;

    private Coroutine _delayedWaveSwitch;
    private bool _canStartNextWave;

    public event UnityAction<bool> SpawningFinished;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToSpawners();
    }

    private void OnDisable()
    {
        StopDelayedSwitchCoroutine();
        UnsubscribeFromSpawners();
    }

    private void Setup()
    {
        _spawners ??= new List<AttackerSpawner>();
        _canStartNextWave = true;
    }

    private bool CheckIfNoWavesRemain()
    {
        bool spawningFinished = true;

        foreach (AttackerSpawner spawner in _spawners)
        {
            if (spawner.HasMoreWaves)
            {
                spawningFinished = false;
                break;
            }
        }

        return spawningFinished;
    }

    private void UpdateWaves()
    {
        foreach (AttackerSpawner spawner in _spawners)
        {
            spawner.UpdateWaveData();
        }
        _canStartNextWave = true;
    }

    private IEnumerator SwitchWavesWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateWaves();
        StopDelayedSwitchCoroutine();
    }

    private void StopDelayedSwitchCoroutine()
    {
        if (_delayedWaveSwitch != null)
        {
            StopCoroutine(_delayedWaveSwitch);
            _delayedWaveSwitch = null;
        }
    }
    private void OnSpawnerStopped()
    {
        SpawningFinished?.Invoke(CheckIfNoWavesRemain());
    }

    private void OnWaveFinished()
    {
        bool spawnersHaveWavesRemaining = false;
        bool allSpawnersFinishedSpawning = true;

        foreach (AttackerSpawner spawner in _spawners)
        {
            if(spawner.HasMoreWaves)
            {
                spawnersHaveWavesRemaining = true;
            }

            if (spawner.FinishedSpawning == false)
            {
                allSpawnersFinishedSpawning = false;
            }
        }

        if (spawnersHaveWavesRemaining && allSpawnersFinishedSpawning && _canStartNextWave)
        {
            _canStartNextWave = false;
            _delayedWaveSwitch = StartCoroutine(SwitchWavesWithDelay(_waveSwitchDelay));
        }
    }

    private void SubscribeToSpawners()
    {
        foreach (AttackerSpawner spawner in _spawners)
        {
            spawner.Stopped += OnSpawnerStopped;
            spawner.WaveFinished += OnWaveFinished;
        }
    }

    private void UnsubscribeFromSpawners()
    {
        foreach (AttackerSpawner spawner in _spawners)
        {
            spawner.Stopped -= OnSpawnerStopped;
            spawner.WaveFinished -= OnWaveFinished;
        }
    }
}
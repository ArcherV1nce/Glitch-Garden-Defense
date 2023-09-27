using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [SerializeField] private Village _village;
    [SerializeField] private AttackerSpawnersControl _spawnersControl;
    [SerializeField] private LineScanner _lineInfo;

    private bool _wavesFinished;
    private bool _enemiesDefeated;

    public event UnityAction<bool> Finished;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void Setup()
    {
        _wavesFinished = false;
        _enemiesDefeated = false;

        if (_village == null)
        {
            _village = FindObjectOfType<Village>();
        }

        if (_spawnersControl == null)
        {
            _spawnersControl = FindObjectOfType<AttackerSpawnersControl>();
        }

        if (_lineInfo == null)
        {
            _lineInfo = FindObjectOfType<LineScanner>();
        }
    }

    private void OnGameOver()
    {
        Finished?.Invoke(true);
    }

    private void OnEnemiesDefeated(bool areDefeated)
    {
        _enemiesDefeated = areDefeated;
        CheckLevelFinish();
    }

    private void OnSpawningFinished(bool wavesFinished)
    {
        _wavesFinished = wavesFinished;
        CheckLevelFinish();
    }

    private void CheckLevelFinish()
    {
        bool isFinished = _wavesFinished && _enemiesDefeated;
        
        if (isFinished)
        {
            Finished?.Invoke(isFinished);
        }
    }

    private void SubscribeToEvents()
    {
        _village.Destroyed += OnGameOver;
        _spawnersControl.SpawningFinished += OnSpawningFinished;
        _lineInfo.EnemiesDefeated += OnEnemiesDefeated;
    }

    private void UnsubscribeFromEvents()
    {
        _village.Destroyed -= OnGameOver;
        _spawnersControl.SpawningFinished -= OnSpawningFinished;
        _lineInfo.EnemiesDefeated -= OnEnemiesDefeated;
    }
}
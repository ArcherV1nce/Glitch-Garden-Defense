using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Village _village;
    [SerializeField] private AttackerSpawnersControl _spawnersControl;
    [SerializeField] private LevelLoader _loader;

    private bool _wavesFinished;
    private bool _enemiesDefeated;

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

        if (_loader == null)
        {
            _loader = FindObjectOfType<LevelLoader>();
        }
    }

    private void OnGameOver()
    {
        RestartLevel();
    }

    private void OnSpawningFinished(bool wavesFinished)
    {
        _wavesFinished = wavesFinished;
    }

    private void RestartLevel()
    {
        _loader.RestartLevel();
    }

    private void SubscribeToEvents()
    {
        _village.Destroyed += OnGameOver;
        _spawnersControl.SpawningFinished += OnSpawningFinished;
    }

    private void UnsubscribeFromEvents()
    {
        _village.Destroyed -= OnGameOver;
        _spawnersControl.SpawningFinished += OnSpawningFinished;
    }
}
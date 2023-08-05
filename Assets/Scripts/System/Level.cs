using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Village _village;
    [SerializeField] private LevelLoader _loader;

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
        if (_village == null)
        {
            _village = FindObjectOfType<Village>();
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

    private void RestartLevel()
    {
        _loader.RestartLevel();
    }

    private void SubscribeToEvents()
    {
        _village.Destroyed += OnGameOver;
    }

    private void UnsubscribeFromEvents()
    {
        _village.Destroyed -= OnGameOver;
    }
}
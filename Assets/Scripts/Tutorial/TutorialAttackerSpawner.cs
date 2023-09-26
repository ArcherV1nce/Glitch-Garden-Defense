using System.Collections;
using UnityEngine;

public class TutorialAttackerSpawner : AttackerSpawner
{
    [SerializeField] private Tutorial _tutorial;
    
    private bool _isPaused;

    public bool IsPaused => _isPaused;

    private void Awake()
    {
        Setup();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SubscribeToTutorial();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UnsubscribeFromTutorial();
    }

    protected override IEnumerator SpawnAtackers()
    {
        while (IsPaused)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return base.SpawnAtackers();
    }

    private void PauseSpawning()
    {
        Debug.Log($"Tutorial asked to stop spawning.");
        _isPaused = true;
        StopSpawning();
        StartSpawning();
    }

    private void Unpause()
    {
        _isPaused = false;
    }

    private void Setup()
    {
        _isPaused = false;
        ValidateTutorial();
    }

    private void ValidateTutorial()
    { 
        if (_tutorial == null)
        {
            _tutorial = FindObjectOfType<Tutorial>();
        }
    }

    private void SubscribeToTutorial()
    {
        _tutorial.Started += PauseSpawning;
        _tutorial.Unpaused += Unpause;
    }

    private void UnsubscribeFromTutorial()
    {
        _tutorial.Started -= PauseSpawning;
        _tutorial.Unpaused -= Unpause;
    }
}
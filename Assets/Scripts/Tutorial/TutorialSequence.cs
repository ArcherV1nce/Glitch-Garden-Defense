using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Tutorial))]
public abstract class TutorialSequence : MonoBehaviour
{
    public event UnityAction Finished;

    private Tutorial _tutorial;
    private Coroutine _sequence;

    protected virtual void Awake()
    {
        ValidateTutorial();
    }

    protected virtual void OnEnabled()
    {
        SubscribeToTutorial();
    }

    protected virtual void OnDisabled()
    {
        UnsubscribeFromTutorial();
    }

    protected virtual void StartTutorial()
    {
        Debug.Log($"Tutorial should have started (abstract Tutorial Sequnece)");
        _sequence = StartCoroutine(PlayTutorial());
    }

    protected virtual void OnStarted()
    {
        Debug.Log($"Tutorial asked abstract sequence to start");
        StartTutorial();
    }

    protected void NotifiyAboutFinish()
    {
        Finished?.Invoke();
    }

    public virtual void OnDefenderSpawned(Defender defender)
    {
        
    }

    protected virtual IEnumerator PlayTutorial()
    {
        yield return new WaitForEndOfFrame();
        StopSequence();
    }

    private void StopSequence()
    {
        if (_sequence != null)
        {
            StopCoroutine(_sequence);
            _sequence = null;
        }
    }

    private void ValidateTutorial()
    {
        if (_tutorial == null)
        {
            _tutorial = GetComponent<Tutorial>();
        }
    }

    private void SubscribeToTutorial()
    {
        _tutorial.Started += OnStarted;
    }

    private void UnsubscribeFromTutorial()
    {
        _tutorial.Started -= OnStarted;
    }
}
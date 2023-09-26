using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TutorialSequence _tutorialSequence;

    private bool _isPlaying;
    private bool _finished;

    public event UnityAction Started;
    public event UnityAction Unpaused;

    public bool IsPlaying => _isPlaying;
    public bool IsFinished => _finished;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToTutorialSequence();
    }

    private void OnDisable()
    {
        UnsubscribeFromTutorialSequence();
    }

    public void StartPlaying()
    {
        if (IsPlaying == false && IsFinished == false)
        {
            _isPlaying = true;
            Started?.Invoke();
            Debug.Log($"Tutorial tried to invoke Start Event");
        }
    }

    private void Unpause()
    {
        _isPlaying = false;
        _finished = true;
        Unpaused?.Invoke();
    }

    private void OnSequenceFinished()
    {
        Unpause();
    }

    private void Setup()
    {
        _isPlaying = false;
        _finished = false;
        ValidateTutorialSequence();
    }

    private void ValidateTutorialSequence()
    {
        if (_tutorialSequence == null)
        {
            _tutorialSequence = GetComponent<TutorialSequence>();
        }
    }

    private void SubscribeToTutorialSequence()
    {
        _tutorialSequence.Finished += OnSequenceFinished;
    }

    private void UnsubscribeFromTutorialSequence()
    {
        _tutorialSequence.Finished -= OnSequenceFinished;
    }
}
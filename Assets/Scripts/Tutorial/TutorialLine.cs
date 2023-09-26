using UnityEngine;
using UnityEngine.Events;

public class TutorialLine : Line
{
    [SerializeField] Tutorial _tutorial;
    [SerializeField] TutorialSequence _sequence;

    public event UnityAction<Defender> DefenderSpawned;

    protected override void Setup()
    {
        base.Setup();
        ValidateTutorial();
        ValidateTutorialSequence();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        AddSequenceSubscribtions();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        RemoveSequenceSubscriptions();
    }

    protected override void AddAttacker(Attacker attacker)
    {
        if (_tutorial.IsPlaying == false && _tutorial.IsFinished == false)
        {
            _tutorial.StartPlaying();
        }

        base.AddAttacker(attacker);
    }

    protected override void AddDefender(Defender defender)
    {
        DefenderSpawned?.Invoke(defender);
        base.AddDefender(defender);
    }

    private void ValidateTutorial()
    {
        if (_tutorial == null)
        {
            _tutorial = FindObjectOfType<Tutorial>();
        }
    }

    private void ValidateTutorialSequence()
    {
        if (_sequence == null)
        {
            _sequence = FindObjectOfType<TutorialSequence>();
        }
    }

    private void AddSequenceSubscribtions()
    {
        DefenderSpawned += _sequence.OnDefenderSpawned;
    }

    private void RemoveSequenceSubscriptions()
    {
        DefenderSpawned -= _sequence.OnDefenderSpawned;
    }
}
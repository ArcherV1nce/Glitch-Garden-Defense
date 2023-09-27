using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FoxTutorialSequence : TutorialSequence
{
    private const float PauseDelayMin = 0.1f;
    private const float PauseDelayMax = 1f;
    private const float TimescalePaused = 0f;
    private const float TimescaleDefault = 1f;

    [SerializeField] private TextMeshProUGUI _textUI;
    [SerializeField] private Button _continueButton;
    [SerializeField, Range(PauseDelayMin, PauseDelayMax)]
    private float _pauseDelay;
    [SerializeField] private string _enemyDescription;

    private List<AttackerSpawner> _spawners;
    private Fox _fox;
    private bool _paused;
    private bool _foxUsedSkill;
    private bool _enemyDescriptionRead;

    protected override void Awake()
    {
        base.Awake();
        ValidateFields();
        ValidateSpawners();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SubscribeToFox();
        SubscribeToSpawners();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UnsubscribeFromFox();
        UnsubscribeFromSpawners();
    }

    public void OnAttackerSpawned(Attacker attacker)
    {
        if (attacker is Fox fox)
        {
            _fox = fox;
            SubscribeToFox();
        }
    }

    protected override IEnumerator PlayTutorial()
    {
        while (_foxUsedSkill == false)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(_pauseDelay);

        Time.timeScale = TimescalePaused;
        _paused = true;


        yield return new WaitForEndOfFrame();

        _textUI.gameObject.SetActive(true);
        _textUI.text = _enemyDescription;
        _continueButton.gameObject.SetActive(true);
        _continueButton.onClick.AddListener(SetEnemyDescriptionRead);
        yield return new WaitForEndOfFrame();

        while (_paused)
        {
            while (_enemyDescriptionRead == false)
            {
                yield return new WaitForEndOfFrame();
            }

            _continueButton.onClick.RemoveListener(SetEnemyDescriptionRead);
            _continueButton.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            Time.timeScale = TimescaleDefault;
            _textUI.text = string.Empty;
            _textUI.gameObject.SetActive(false);
            _paused = false;
            NotifiyAboutFinish();
        }

        yield return base.PlayTutorial();
    }

    private void SetEnemyDescriptionRead()
    {
        _enemyDescriptionRead = true;
    }

    private void ValidateFields()
    {
        if (_textUI == null)
        {
            Debug.LogError($"{_textUI} is not assigned");
        }

        if (_continueButton == null)
        {
            Debug.LogError($"{_continueButton} is not assigned");
        }

        _textUI.text = string.Empty;
        _textUI.gameObject.SetActive(false);
        _continueButton.gameObject.SetActive(false);

        _paused = false;
        _enemyDescriptionRead = false;
        _foxUsedSkill = false;
    }

    private void ValidateSpawners()
    {
        if (_spawners == null || _spawners.Count == 0)
        {
            _spawners = FindObjectsOfType<AttackerSpawner>().ToList();
        }
    }

    private void OnFoxSkillTriggered()
    {
        _foxUsedSkill = true;
    }

    private void OnDied(Attacker attacker)
    {
        UnsubscribeFromFox();
    }

    private void SubscribeToSpawners()
    {
        foreach (AttackerSpawner spawner in _spawners)
        {
            spawner.AttackerSpawned += OnAttackerSpawned;
        }
    }

    private void UnsubscribeFromSpawners()
    {
        foreach (AttackerSpawner spawner in _spawners)
        {
            spawner.AttackerSpawned -= OnAttackerSpawned;
        }
    }

    private void SubscribeToFox()
    {
        if (_fox != null)
        {
            _fox.SkillTriggered += OnFoxSkillTriggered;
            _fox.Died += OnDied;
        }
    }

    private void UnsubscribeFromFox()
    {
        if (_fox != null)
        {
            _fox.SkillTriggered -= OnFoxSkillTriggered;
            _fox.Died -= OnDied;
        }
    }
}
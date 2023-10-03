using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GravestoneTutorial : TutorialSequence
{
    private const float PauseDelayMin = 1f;
    private const float PauseDelayMax = 5f;
    private const float TimescalePaused = 0f;
    private const float TimescaleDefault = 1f;

    [SerializeField] private TextMeshProUGUI _textUI;
    [SerializeField] private Button _continueButton;
    [SerializeField, Range(PauseDelayMin, PauseDelayMax)]
    private float _pauseDelay;
    [SerializeField] private List<string> _description;

    private bool _paused;
    private bool _enemyDescriptionRead;
    private bool _defenderSpawned;

    protected override void Awake()
    {
        base.Awake();
        ValidateFields();
    }

    public override void OnDefenderSpawned(Defender defender)
    {
        _defenderSpawned = true;
    }

    protected override IEnumerator PlayTutorial()
    {
        int descriptionId = 0;
        yield return new WaitForSecondsRealtime(_pauseDelay);

        Time.timeScale = TimescalePaused;
        _paused = true;


        yield return new WaitForEndOfFrame();

        _textUI.gameObject.SetActive(true);
        _textUI.text = _description[descriptionId];
        _continueButton.gameObject.SetActive(true);
        _continueButton.onClick.AddListener(SetEnemyDescriptionRead);
        yield return new WaitForEndOfFrame();

        while (_paused)
        {
            while (_enemyDescriptionRead == false)
            {
                yield return new WaitForEndOfFrame();
            }

            if (descriptionId < _description.Capacity)
            {
                descriptionId++;
            }

            _textUI.text = _description[descriptionId];
            _continueButton.onClick.RemoveListener(SetEnemyDescriptionRead);
            _continueButton.gameObject.SetActive(false);
            yield return new WaitForEndOfFrame();

            while (_defenderSpawned == false)
            {
                yield return new WaitForEndOfFrame();
            }

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
        _defenderSpawned = false;
    }
}
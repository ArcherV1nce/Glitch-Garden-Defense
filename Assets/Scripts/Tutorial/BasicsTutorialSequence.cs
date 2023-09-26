using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BasicsTutorialSequence : TutorialSequence
{
    private const float PauseDelayMin = 1f;
    private const float PauseDelayMax = 5f;
    private const float TimescalePaused = 0f;
    private const float TimescaleDefault = 1f;

    [SerializeField] private TextMeshProUGUI _textUI;
    [SerializeField] private Button _continueButton;
    [SerializeField, Range(PauseDelayMin, PauseDelayMax)] 
    private float _pauseDelay;
    [SerializeField] private string _enemyDescription;
    [SerializeField] private string _defenderDescription;

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
        Debug.Log($"Tutorial sequence started.");
        yield return new WaitForSeconds(_pauseDelay);
        Debug.Log($"Tutorial Started");
        Time.timeScale = TimescalePaused;
        while (_paused)
        {
            Debug.Log($"Game was paused");
            yield return new WaitForFixedUpdate();

            _textUI.text = _enemyDescription;
            _continueButton.gameObject.SetActive(true);
            _continueButton.onClick.AddListener(SetEnemyDescriptionRead);
            yield return new WaitForFixedUpdate();

            Debug.Log($"Waiting for button to be pressed");
            while (_enemyDescriptionRead == false)
            {
                yield return new WaitForFixedUpdate();
            }

            Debug.Log($"Button pressed");
            _textUI.text = _defenderDescription;
            _continueButton.onClick.RemoveListener(SetEnemyDescriptionRead);
            _continueButton.gameObject.SetActive(false);
            yield return new WaitForFixedUpdate();

            Debug.Log($"Waiting for Defender to be spawned");

            while (_defenderSpawned == false)
            {
                yield return new WaitForFixedUpdate();
            }

            Time.timeScale = TimescaleDefault;
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
        _continueButton.gameObject.SetActive(false);

        _paused = false;
        _enemyDescriptionRead = false;
        _defenderSpawned = false;
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadLevelButton : MonoBehaviour
{
    private const float RestartDelay = 1f;

    [SerializeField] private SceneName _level;

    private Coroutine _restartRoutine;
    private Button _button;
    private LevelLoader _levelLoader;

    public SceneName Level => _level;

    private void OnEnable()
    {
        Setup();
        SubscribeToButton();
    }

    private void OnDisable()
    {
        UnsubscribeFromButton();
    }

    public void RestartLevel()
    {
        if (_restartRoutine == null)
        {
            _restartRoutine = StartCoroutine(Restart());
        }
    }

    private IEnumerator Restart()
    {
        ValidateLevelLoader();
        yield return new WaitForSecondsRealtime(RestartDelay);
        _levelLoader.RestartLevel();
        StopRestartCoroutine();
    }

    private void StopRestartCoroutine()
    {
        if (_restartRoutine != null)
        {
            StopCoroutine(_restartRoutine);
            _restartRoutine = null;
        }
    }

    private void SendLevelInfo()
    {
        ValidateLevelLoader();
        _levelLoader.ReceiveLevelData(this);
    }

    private void Setup()
    {
        _button = GetComponent<Button>();
        ValidateLevelLoader();
    }

    private void ValidateLevelLoader()
    {
        if (_levelLoader == null)
        {
            _levelLoader = FindObjectOfType<LevelLoader>();
        }
    }

    private void SubscribeToButton()
    {
        _button.onClick.AddListener(SendLevelInfo);
    }

    private void UnsubscribeFromButton()
    {
        _button.onClick.RemoveListener(SendLevelInfo);
    }
}
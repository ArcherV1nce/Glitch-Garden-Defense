using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadLevelButton : MonoBehaviour
{
    [SerializeField] private SceneName _level;

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
        ValidateLevelLoader();
        _levelLoader.RestartLevel();
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
using System.Collections;
using UnityEngine;

public class LevelCanvasSwitch : MonoBehaviour
{
    [SerializeField] private Canvas _gameUI;
    [SerializeField] private Canvas _completionUI;

    private Level _level;
    private bool _completionUIShown;

    private void Awake()
    {
        Setup();
        ShowUI();
    }

    private void OnEnable()
    {
        SubscribeToLevel();
    }

    private void OnDisable()
    {
        UnsubscribeFromLevel();
    }

    private void OnValidate()
    {
        ValidateLevel();
    }

    public void SwitchUI(bool levelCompleted)
    {
        _completionUIShown = !_completionUIShown;
        ShowUI();
    }

    private void ShowUI()
    {
        switch (_completionUIShown)
        {
            case true:
                _gameUI.gameObject.SetActive(false);
                _completionUI.gameObject.SetActive(true);
                break;

            case false:
                _gameUI.gameObject.SetActive(true);
                _completionUI.gameObject.SetActive(false);
                break;
        }
    }

    private void Setup()
    {
        _completionUIShown = false;
        ValidateLevel();
    }

    private void ValidateLevel()
    {
        if (_level == null)
        {
            _level = FindObjectOfType<Level>();
        }
    }

    private void SubscribeToLevel()
    {
        _level.Finished += SwitchUI;
    }

    private void UnsubscribeFromLevel()
    {
        _level.Finished -= SwitchUI;
    }
}
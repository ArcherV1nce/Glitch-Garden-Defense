using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private const int UndefinedSceneId = 0;
    private const float DestructionDelay = 0.1f;
    private const float LoadingDelay = 3f;
    private const float DefaultTimescale = 1f;

    [SerializeField] private float _timeToWait = LoadingDelay;
    [SerializeField] private bool _sceneLoadingFinished = false;
    [SerializeField] private SceneName _loadingScreen;
    [SerializeField] private SceneName _level;

    private Coroutine _loader;
    private Coroutine _loadVisualizer;  

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDisable()
    {
        StopLoadingCoroutines();
    }

    public void LoadLevel()
    {
        Time.timeScale = DefaultTimescale;

        if(_level != SceneName.Undefined && _level.ToString() != SceneManager.GetActiveScene().name)
        {
            LoadScreenLoadLevel();
        }

        else
        {
            if (_level != SceneName.Undefined)
            {
                _loader = StartCoroutine(LoadingOperationProcess());
            }
        }
    }

    public void LoadWithDelay()
    {
        if (_level != SceneName.Undefined)
        {
            _loader = StartCoroutine(LoadLevelWithDelay());
        }
    }

    public void RestartLevel()
    {
        _level = (SceneName)GetSceneEnumId(SceneManager.GetActiveScene().name);
        LoadLevel();
    }

    public void LoadScreenLoadLevel()
    {
        _loadVisualizer = StartCoroutine(LoadScreenOperationProcess());    
    }

    public bool GetLoadLevelStatus()
    {
        return _sceneLoadingFinished;
    }

    public void ReceiveLevelData(LoadLevelButton levelButton)
    {
        _level = levelButton.Level;
        LoadLevel();
    }

    private IEnumerator LoadLevelWithDelay()
    {
        yield return new WaitForSeconds(_timeToWait);
        LoadScreenLoadLevel();
        yield return null;
    }

    private IEnumerator LoadScreenOperationProcess()
    {
        SceneManager.LoadSceneAsync(_loadingScreen.ToString());
        while(SceneManager.GetActiveScene().name != _loadingScreen.ToString())
        {
            yield return new WaitForEndOfFrame();
        }

        AsyncOperation sceneLoadingOperation = SceneManager.LoadSceneAsync(_level.ToString());
        
        while (!_sceneLoadingFinished)
        {
            _sceneLoadingFinished = sceneLoadingOperation.isDone;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        StopLoadingCoroutines();
    }

    private IEnumerator LoadingOperationProcess()
    {
        AsyncOperation sceneLoadingOperation = SceneManager.LoadSceneAsync(_level.ToString());

        while (!_sceneLoadingFinished)
        {
            _sceneLoadingFinished = sceneLoadingOperation.isDone;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        StopLoadingCoroutines();
    }

    private int GetSceneEnumId (string name)
    {
        int id = UndefinedSceneId;

        string[] sceneNames = Enum.GetNames(typeof(SceneName));
        
        for(int i = 0; i < sceneNames.Length; i++)
        {
            if (name == sceneNames[i])
            {
                id = i;
            }
        }

        return id;
    }

    private void StopLoadingCoroutines()
    {
        if (_loader != null)
        {
            StopCoroutine(_loader);
            _loader = null;
        }
        
        if (_loadVisualizer != null)
        { 
            StopCoroutine(_loadVisualizer);
            _loadVisualizer = null;
        }

        Destroy(this.gameObject, DestructionDelay);
    }
}
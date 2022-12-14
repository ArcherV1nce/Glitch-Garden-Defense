using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private string _loadingScreenName = "LoadingScene";
    [SerializeField] private string _levelName = "SplashScreen";
    [SerializeField] private int _timeToWait = 5;
    [SerializeField] private bool _sceneLoadingFinished = false;

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
        if(_levelName != null)
        {
            SceneManager.LoadSceneAsync(_levelName);
        }
    }

    public void LoadWithDelay()
    {
        if (_levelName != null)
        {
            _loader = StartCoroutine(LoadLevelWithDelay());
        }
    }

    private IEnumerator LoadLevelWithDelay()
    {
        yield return new WaitForSeconds(_timeToWait);
        SceneManager.LoadSceneAsync(_levelName);
        yield return null;
    }

    public void LoadScreenLoadLevel()
    {
        _loadVisualizer = StartCoroutine(LoadScreenOperationProcess());    
    }

    private IEnumerator LoadScreenOperationProcess()
    {
        AsyncOperation tempOp = SceneManager.LoadSceneAsync(_loadingScreenName);
        while(SceneManager.GetActiveScene().name != _loadingScreenName)
        {
            yield return new WaitForEndOfFrame();
        }

        AsyncOperation sceneLoadingOperation = SceneManager.LoadSceneAsync(_levelName);
        Debug.Log(sceneLoadingOperation.progress);
        
        while (!_sceneLoadingFinished)
        {
            Debug.Log(sceneLoadingOperation.progress);
            _sceneLoadingFinished = sceneLoadingOperation.isDone;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }

    public bool GetLoadLevelStatus()
    {
        return _sceneLoadingFinished;
    }

    private void StopLoadingCoroutines()
    {
        StopCoroutine(_loader);
        StopCoroutine(_loadVisualizer);
        _loader = null;
        _loadVisualizer = null;
    }
}
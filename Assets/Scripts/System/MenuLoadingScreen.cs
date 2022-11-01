using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoadingScreen : MonoBehaviour
{
    private const string MenuSceneName = "MainMenu";
    private Coroutine _sceneLoader;

    private void Awake()
    {
        StartCoroutine(WaitForSceneLoad());
    }

    private static IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(3f);
        
        SceneManager.LoadSceneAsync(MenuSceneName);

        yield return 0;
    }
}
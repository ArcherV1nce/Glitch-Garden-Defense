using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LoadingTextDisplay : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private TextMeshProUGUI _loadingTextObject;
    [SerializeField] private string _loadingText = "";
    [SerializeField] private float _textUpdateTime = 0.3f;
    [SerializeField] private int _dotsMaxAmount = 3;

    [Header("Sceme Status")]
    [SerializeField] private bool _isLoaded = false;
    [SerializeField] private LevelLoader _leverController = null;

    private Coroutine _textUpdate;

    private void Awake()
    {
        _loadingTextObject = GetComponent<TextMeshProUGUI>();
        _leverController = FindObjectOfType<LevelLoader>();
    }

    private void Start()
    {
        StartTextUpdate();
    }

    private void Update()
    {
        UpdateLoadStatus();
    }

    private IEnumerator LoadingTextUpdate()
    {
        int i = 0;
        string outputText = _loadingText;
        _loadingTextObject.text = outputText;
        
        while (!_isLoaded)
        {
            if (i < _dotsMaxAmount)
            {
                i++;
                outputText = outputText + ".";
            }
            
            else if (i >= 3)
            {
                i = 0;
                outputText = _loadingText;
            }

            _loadingTextObject.text = outputText;

            yield return new WaitForSeconds(_textUpdateTime);
        }

        StopTextUpdate();
    }

    private void StartTextUpdate()
    {
        _textUpdate = StartCoroutine(LoadingTextUpdate());
    }

    private void StopTextUpdate()
    {
        StopCoroutine(_textUpdate);
        _textUpdate = null;
    }

    private void UpdateLoadStatus()
    {
        if (_leverController)
        {
            _isLoaded = _leverController.GetLoadLevelStatus();
        }
    }
}
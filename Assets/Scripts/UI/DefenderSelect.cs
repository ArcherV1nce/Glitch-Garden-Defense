using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(DefenderSpawner))]
public class DefenderSelect : MonoBehaviour
{
    [SerializeField] private List<DefenderButton> _buttons;
    
    private DefenderSpawner _spawner;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToButtonEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromButtonEvents();
    }

    private void Setup()
    {
        _spawner = GetComponent<DefenderSpawner>();

        if (_buttons == null || _buttons.Count == 0)
        {
            _buttons = new List<DefenderButton>();
        }

        ValidateDefenderButtons();
    }

    private void ValidateDefenderButtons()
    {
        List<DefenderButton> buttons = FindObjectsOfType<DefenderButton>().ToList();

        AddMissingButtons();

        if (buttons.Count != _buttons.Count)
        {
            _buttons.Clear();
            AddMissingButtons();
        }
    }

    private void AddMissingButtons()
    {
        List<DefenderButton> buttons = FindObjectsOfType<DefenderButton>().ToList();

        foreach (DefenderButton button in buttons)
        {
            if (_buttons.Contains(button) == false)
            {
                _buttons.Add(button);
            }
        }
    }

    private void SubscribeToButtonEvents()
    {
        foreach (DefenderButton button in _buttons)
        {
            button.DefenderSelected += _spawner.SetSelectedDefender;
        }
    }

    private void UnsubscribeFromButtonEvents()
    {
        foreach (DefenderButton button in _buttons)
        {
            button.DefenderSelected += _spawner.SetSelectedDefender;
        }
    }
}
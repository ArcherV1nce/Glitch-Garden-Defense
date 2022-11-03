using System.Collections.Generic;
using UnityEngine;

public class DefenderSelect : MonoBehaviour
{
    [SerializeField] private List<DefenderButton> _buttons;
    [SerializeField] private DefenderSpawner _spawner;

    private void OnEnable()
    {
        SubscribeOnButtonEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromButtonEvents();
    }

    private void SubscribeOnButtonEvents()
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
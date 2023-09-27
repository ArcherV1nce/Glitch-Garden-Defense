using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LineScanner : MonoBehaviour
{
    [SerializeField] private List<Line> _lines;

    public event UnityAction<bool> EnemiesDefeated;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToLines();
    }

    private void OnDisable()
    {
        UnsubscribeFromLines();
    }

    private void OnValidate()
    {
        ValidateLines();
    }

    private void AlertOnEnemiesDefeated()
    {
        EnemiesDefeated?.Invoke(CheckLinesForAttackersRemaining());
    }

    private bool CheckLinesForAttackersRemaining()
    {
        bool attackSecured = true;
        foreach (Line line in _lines)
        {
            if (line.IsAttacked)
            {
                attackSecured = false;
            }
        }

        return attackSecured;
    }

    private void Setup()
    {
        ValidateLines();
    }

    private void ValidateLines()
    {
        _lines ??= new List<Line>();

        if (_lines.Count == 0)
        {
            _lines = GetComponentsInChildren<Line>().ToList();
        }

        if (_lines.Count == 0 || _lines == null)
        {
            Debug.LogError($"{this} has to be assigned to the parent object of {typeof(Line)} Component.");
        }
    }

    private void SubscribeToLines()
    {
        foreach (Line line in _lines)
        {
            line.AttackStopped += AlertOnEnemiesDefeated;
            line.Attacked += AlertOnEnemiesDefeated;
        }
    }

    private void UnsubscribeFromLines()
    {
        foreach (Line line in _lines)
        {
            line.AttackStopped -= AlertOnEnemiesDefeated;
            line.Attacked -= AlertOnEnemiesDefeated;
        }
    }
}
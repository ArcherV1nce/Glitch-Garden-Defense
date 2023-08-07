using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Line : MonoBehaviour
{
    [SerializeField] private Village _village;
    [SerializeField] private List<Defender> _defenders;
    [SerializeField] private List<Attacker> _attackers;

    public event UnityAction AttackStopped;
    public event UnityAction Attacked;

    public bool IsDefended => _defenders.Count > 0;
    public bool IsAttacked => _attackers.Count > 0;

    private void Awake()
    {
        Setup();
    }

    private void OnEnable()
    {
        SubscribeToAllAttackersDeath();
        AddAllDefendersSubscriptions();
    }

    private void OnDisable()
    {
        UnsubscribeFromAllAttackersDeath();
        RemoveAllDefendersSubscriptions();
    }

    public void AddCharacter(Character character)
    {
        if (character is Defender)
        {
            AddDefender(character as Defender);
        }

        else if (character is Attacker)
        {
            AddAttacker(character as Attacker);
        }
    }

    public void RemoveCharacter(Character character)
    {
        if (character is Defender)
        {
            RemoveDefender(character as Defender);
        }

        else if (character is Attacker)
        {
            RemoveAttacker(character as Attacker);
        }
    }

    private void Setup()
    {
        _defenders ??= new List<Defender>();

        _attackers ??= new List<Attacker>();
    }

    private void AddDefenderSubscriptions(Defender defender)
    {
        defender.Died += RemoveCharacter;
        defender.Spawned += CheckAttackState;
        Attacked += defender.SetAttacked;
        AttackStopped += defender.SetIdle;
    }

    private void RemoveDefenderSubscriptions(Defender defender)
    {
        defender.Died -= RemoveCharacter;
        defender.Spawned -= CheckAttackState;
        Attacked -= defender.SetAttacked;
        AttackStopped -= defender.SetIdle;
    }

    private void AddAttacker(Attacker attacker)
    {
        AlertAttack();
        _attackers.Add(attacker);
        attacker.Died += RemoveCharacter;
    }

    private void AddDefender(Defender defender)
    {
        _defenders.Add(defender);
        AddDefenderSubscriptions(defender);
    }

    private void RemoveDefender(Defender defender)
    {
        _defenders.Remove(defender);
    }

    private void RemoveAttacker(Attacker attacker)
    {
        _attackers.Remove(attacker);
        _village.AddResources(attacker.Reward);

        CheckAttackFinished();
    }

    private void AlertAttack()
    {
        Attacked?.Invoke();
    }

    private void CheckAttackState(Defender defender)
    {
        if (IsAttacked)
        {
            AlertAttack();
        }

        else
        {
            AttackStopped?.Invoke();
        }
    }

    private void CheckAttackFinished()
    {
        if (IsAttacked == false)
        {
            AttackStopped?.Invoke();
        }
    }

    private void SubscribeToAllAttackersDeath()
    {
        foreach (Attacker attacker in _attackers)
        {
            attacker.Died += (RemoveCharacter);
        }
    }

    private void UnsubscribeFromAllAttackersDeath()
    {
        foreach (Attacker attacker in _attackers)
        {
            attacker.Died -= (RemoveCharacter);
        }
    }

    private void AddAllDefendersSubscriptions()
    {
        foreach (Defender defender in _defenders)
        {
            AddDefenderSubscriptions(defender);
        }
    }

    private void RemoveAllDefendersSubscriptions()
    {
        foreach (Defender defender in _defenders)
        {
            RemoveDefenderSubscriptions(defender);
        }
    }
}
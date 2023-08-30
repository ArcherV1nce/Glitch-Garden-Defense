using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScarecrowSkillChargeView : MonoBehaviour
{
    private const float ExitValue = 0f;

    [SerializeField] private DefenderAlertArea _alertArea;

    public event UnityAction<float> RiposteDamageChanged;
    
    private ScarecrowSkill _skill;
    private List<Attacker> _attackers;

    private void OnEnable()
    {
        ValidateDamageViewsList();
        ValidateSkill();
        SubscribeToDeathActions();
        ValidateAlertAreaComponent();
        SubscribeToSkill();
        SubscribeToAlertArea();
    }

    private void OnDisable()
    {
        UnsubscribeFromDeathActions();
        UnsubscribeFromSkill();
        UnsubscribeFromAlertArea();
        ResetViewValueOnDisable();
    }

    private void OnValidate()
    {
        ValidateSkill();
        ValidateAlertAreaComponent();
    }

    private void ResetViewValueOnDisable()
    {
        RiposteDamageChanged?.Invoke(ExitValue);
    }

    private void OnRiposteDamageChange(float damage)
    {
        RiposteDamageChanged?.Invoke(damage);
    }

    private void OnAttackerEnteredArea(Attacker attacker)
    {
        AddAttackerToList(attacker);
        AddSubscriptionForDamageUpdate(attacker);
        SubscribeToAttackerDeath(attacker);
    }

    private void OnAttackerLeftArea(Attacker attacker)
    {
        UnlinkAttacker(attacker);
        RemoveAttackerFromList(attacker);
    }

    private void AddSubscriptionForDamageUpdate(Attacker attacker)
    {
        if (attacker != null)
        {
            RiposteDamageChanged += 
                attacker.GetComponentInChildren<AttackerPotentialDamageView>().UpdatePotentialDamageView;
        }
    }

    private void RemoveSubscriptionForDamageUpdate(Attacker attacker)
    {
        if (attacker != null)
        {
            RiposteDamageChanged -= 
                attacker.GetComponentInChildren<AttackerPotentialDamageView>().UpdatePotentialDamageView;
        }
    }

    private void ValidateSkill()
    {
        if (_skill == null)
        {
            _skill = GetComponentInParent<ScarecrowSkill>();
        }
    }

    private void ValidateAlertAreaComponent()
    {
        if (_alertArea == null)
        {
            _alertArea = _skill.GetComponentInChildren<DefenderAlertArea>();
        }
    }

    private void ValidateDamageViewsList()
    {
        _attackers ??= new List<Attacker>();
    }

    private void AddAttackerToList(Attacker attacker)
    {
        if (attacker != null)
        {
            _attackers.Add(attacker);
        }
    }

    private void RemoveAttackerFromList(Attacker attacker)
    {
        if (attacker != null)
        {
            _attackers.Remove(attacker);
        }
    }

    private void UnlinkAttackers()
    {
        foreach(Attacker attacker in _attackers)
        {
            UnlinkAttacker(attacker);
        }
        _attackers.Clear();
    }

    private void UnlinkAttacker(Attacker attacker)
    {
        RiposteDamageChanged?.Invoke(ExitValue);
        RemoveSubscriptionForDamageUpdate(attacker);
        UnsubscribeFromAttackerDeath(attacker);
    }

    private void SubscribeToDeathActions()
    {
        _skill.GetComponent<Scarecrow>().Died += OnDied;
    }

    private void UnsubscribeFromDeathActions()
    {
        _skill.GetComponent<Scarecrow>().Died -= OnDied;
    }

    private void OnDied(Character character)
    {
        UnlinkAttackers();
    }

    private void SubscribeToSkill()
    {
        _skill.RiposteDamageChanged += OnRiposteDamageChange;
    }

    private void SubscribeToAlertArea()
    {
        _alertArea.AttackerEnteredArea += OnAttackerEnteredArea;
        _alertArea.AttackerLeftArea += OnAttackerLeftArea;
    }

    private void SubscribeToAttackerDeath(Attacker attacker)
    {
        attacker.Died += RemoveSubscriptionForDamageUpdate;
    }

    private void UnsubscribeFromAlertArea()
    {
        _alertArea.AttackerEnteredArea -= OnAttackerEnteredArea;
        _alertArea.AttackerLeftArea -= OnAttackerLeftArea;
    }

    private void UnsubscribeFromSkill()
    {
        _skill.RiposteDamageChanged -= OnRiposteDamageChange;
    }

    private void UnsubscribeFromAttackerDeath(Attacker attacker)
    {
        attacker.Died -= RemoveSubscriptionForDamageUpdate;
    }
}
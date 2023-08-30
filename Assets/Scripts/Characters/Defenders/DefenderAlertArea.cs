using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class DefenderAlertArea : MonoBehaviour
{
    private const float SizeValueXMin = 1;
    private const float SizeValueYMin = 1;

    [SerializeField] private Vector2 _size;

    private BoxCollider2D _detectionArea;
    private List<Attacker> _attackers;

    public event UnityAction<bool> AlertUpdated;
    public event UnityAction<Attacker> AttackerEnteredArea;
    public event UnityAction<Attacker> AttackerLeftArea;

    public bool IsAlerted => _attackers.Count > 0;

    private void Awake()
    {
        Setup();
    }

    private void OnValidate()
    {
        ValidateSize();
        ValidateTriggerCollider();
        ValidateListOfAttackers();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckIfAttackerEnteredArea(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckIfAttackerLeftArea(collision);
    }

    private void CheckIfAttackerEnteredArea(Collider2D collision)
    {
        if (TryGetAttacker(collision, out Attacker attacker))
        {
            AddAttackerToList(attacker);
            AttackerEnteredArea?.Invoke(attacker);
            SubsribeOnAttackerDeath(attacker);
        }
    }

    private void CheckIfAttackerLeftArea(Collider2D collision)
    {
        if (TryGetAttacker(collision, out Attacker attacker))
        {
            RemoveAttackerFromList(attacker);
            AttackerLeftArea?.Invoke(attacker);
            UnsubscribeFromAttackerDeath(attacker);
        }
    }

    private void AddAttackerToList(Attacker attacker)
    {
        _attackers.Add(attacker);
        AlertUpdated?.Invoke(IsAlerted);
    }

    private void RemoveAttackerFromList(Attacker attacker)
    {
        UnsubscribeFromAttackerDeath(attacker);
        _attackers.Remove(attacker);
        AlertUpdated?.Invoke(IsAlerted);
    }

    private bool TryGetAttacker(Collider2D collision, out Attacker attacker)
    {
        if (collision.TryGetComponent<Attacker>(out attacker))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SubsribeOnAttackerDeath(Attacker attacker)
    {
        attacker.Died += RemoveAttackerFromList;
    }

    private void UnsubscribeFromAttackerDeath(Attacker attacker)
    {
        attacker.Died -= RemoveAttackerFromList;
    }

    private void SetCenteredPosition()
    {
        transform.localPosition = Vector3.zero;
    }

    private void ValidateTriggerCollider()
    {
        if (_detectionArea == null)
        {
            _detectionArea = GetComponent<BoxCollider2D>();
        }

        _detectionArea.isTrigger = true;

        if (_detectionArea.size != _size)
        {
            _detectionArea.size = _size;
        }
    }

    private void ValidateRigidbody()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    private void ValidateSize()
    {
        if (_size.x < SizeValueXMin)
        {
            _size.x = SizeValueXMin;
        }

        if (_size.y < SizeValueYMin)
        {
            _size.y = SizeValueYMin;
        }
    }

    private void ValidateListOfAttackers()
    {
        if (_attackers == null)
        {
            _attackers = new List<Attacker>();
        }
    }

    private void Setup()
    {
        ValidateRigidbody();
        ValidateSize();
        ValidateTriggerCollider();
        ValidateListOfAttackers();
        SetCenteredPosition();
    }
}
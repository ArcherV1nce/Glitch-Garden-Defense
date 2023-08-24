using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ScarecrowRiposteButton : MonoBehaviour
{
    private Scarecrow _scarecrow;
    private ScarecrowSkill _riposteSkill;
    private bool _isActive;
    private Collider2D _collider;
    private ScarecrowRiposteButtonVisual _visual;

    public event UnityAction<bool> OnSkillStatusUpdated;
    public event UnityAction OnDefenderClicked;

    private void OnEnable()
    {
        ValidateScarecrow();
        ValidateRiposteSkill();
        ValidateVisual();
        ValidateCollider();
        SubscribeToScarecrow();
        SubscribeToScarecrowRiposteStatus();
    }

    private void OnDisable()
    {
        UnsubscriveFromScarecrow();
        UnsubscribeFromScarecrowRiposteStatus();
    }

    private void OnValidate()
    {
        ValidateScarecrow();
        ValidateRiposteSkill();
        ValidateVisual();
        ValidateCollider();
    }

    public void TriggerClick()
    {
        OnDefenderClicked?.Invoke();
    }

    private void UpdateStatus(bool isActive)
    {
        _isActive = isActive;
        OnSkillStatusUpdated.Invoke(_isActive);
    }

    private void ChangeButtonStatus(bool isActive)
    {
        if (isActive && _scarecrow.IsAttacked)
        {
            _collider.enabled = true;
        }
        else
        {
            _collider.enabled = false;
        }
    }

    private void ValidateScarecrow()
    {
        if (_scarecrow == null)
        {
            _scarecrow = GetComponentInParent<Scarecrow>();
        }
    }

    private void ValidateRiposteSkill()
    {
        if (_riposteSkill == null)
        {
            _riposteSkill = GetComponentInParent<ScarecrowSkill>();
        }
    }

    private void ValidateVisual()
    {
        if (_visual == null)
        {
            _visual = GetComponent<ScarecrowRiposteButtonVisual>();
        }
    }

    private void ValidateCollider()
    {
        if (_collider == null)
        {
            _collider = GetComponent<Collider2D>();
        }
    }

    private void SubscribeToScarecrow()
    {
        _scarecrow.OnAttackStatusChanged += ChangeButtonStatus;
    }

    private void UnsubscriveFromScarecrow()
    {
        _scarecrow.OnAttackStatusChanged -= ChangeButtonStatus;
    }

    private void SubscribeToScarecrowRiposteStatus()
    {
        _riposteSkill.ChargeStatusUpdated?.AddListener(UpdateStatus);
        _riposteSkill.ChargeStatusUpdated?.AddListener(ChangeButtonStatus);
    }

    private void UnsubscribeFromScarecrowRiposteStatus()
    {
        _riposteSkill.ChargeStatusUpdated?.RemoveListener(UpdateStatus);
        _riposteSkill.ChargeStatusUpdated?.RemoveListener(ChangeButtonStatus);
    }
}
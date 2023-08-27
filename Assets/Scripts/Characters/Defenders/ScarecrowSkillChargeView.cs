using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScarecrowSkillChargeView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _damageVisuals;
    [SerializeField] private string _text;

    private ScarecrowSkill _skill;

    private void OnEnable()
    {
        ValidateSkill();
        ValidateTextComponent();
        SubscribeToSkill();
    }

    private void OnDisable()
    {
        UnsubscribeFromSkill();
    }

    private void OnValidate()
    {
        ValidateSkill();
        ValidateTextComponent();
        ValidateText();
    }

    private void OnRiposteDamageChange(float damage)
    {
        string text = _text + "\n";
        _damageVisuals.text = text + damage.ToString();
    }

    private void ValidateSkill()
    {
        if (_skill == null)
        {
            _skill = GetComponentInParent<ScarecrowSkill>();
        }
    }

    private void ValidateTextComponent()
    {
        if (_damageVisuals == null)
        {
            GetComponent<TextMeshProUGUI>();
        }
    }

    private void ValidateText()
    {
        _damageVisuals.text = _text + "\n";
    }

    private void SubscribeToSkill()
    {
        _skill.RiposteDamageChanged += OnRiposteDamageChange;
    }

    private void UnsubscribeFromSkill()
    {
        _skill.RiposteDamageChanged -= OnRiposteDamageChange;
    }
}
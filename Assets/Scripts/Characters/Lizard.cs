using UnityEngine;

public class Lizard : Attacker
{
    [SerializeField] private MeleeAttack _damageArea;

    protected override void OnEnable()
    {
        base.OnEnable();
        _damageArea.CharacterEnteredDamageArea.AddListener(TrySetTarget);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _damageArea.CharacterEnteredDamageArea.RemoveListener(TrySetTarget);
    }

    private void TrySetTarget (Character character)
    {
        if (character is Defender)
        {
            Attack(character as Defender);
        }
    }
}
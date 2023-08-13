using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class MeleeAttack : MonoBehaviour
{
    private Collider2D _trigger;

    public UnityEvent<Character> CharacterEnteredMeleeAttackArea;

    private void OnValidate()
    {
        ValidateTrigger();
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        CheckForCharacterEnter(collision);
    }

    public void ValidateTrigger()
    {
        if (_trigger == null)
        {
            _trigger = GetComponent<Collider2D>();
        }

        if (_trigger.isTrigger == false)
        {
            _trigger.isTrigger = true;
        }
    }

    private bool CheckForCharacterEnter (Collider2D collider)
    {
        if (collider.TryGetComponent<Character>(out Character character))
        {
            CharacterEnteredMeleeAttackArea?.Invoke(character);
            return true;
        }

        else
        {
            return false;
        }
    }
}
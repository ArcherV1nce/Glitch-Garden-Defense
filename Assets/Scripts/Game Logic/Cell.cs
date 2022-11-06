using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Cell : MonoBehaviour
{
    private const float SizeMin = 1f;

    [SerializeField] private float _size;
    [SerializeField] private Character _character;

    private Line _line;

    public event UnityAction FillChanged;
    public float Size => _size;
    public bool IsFree => _character == null;
    public Character Character => _character;

    private void OnEnable()
    {
        TrySubscribeOnCharacterDeath();
    }

    private void OnDisable()
    {
        UnsubscribeFromCharacterDeath();
    }

    private void OnValidate()
    {
        ValidateSize();
        ValidateLine();
    }

    public bool TryAddCharacter (Character character)
    {
        if (IsFree)
        {
            _character = character;
            _line.AddCharacter(_character);
            TrySubscribeOnCharacterDeath();
            FillChanged.Invoke();
            return true;
        }

        else
        {
            return false;
        }
    }

    public void SetLine (Line line)
    {
        _line = line;
    }

    public void Clear ()
    {
        _character = null;
    }

    private void ValidateSize ()
    {
        if (_size < SizeMin)
        {
            _size = SizeMin;
        }
    }

    private void ValidateLine()
    {
        if (_line == null)
        {
            _line = gameObject.GetComponentInParent<Line>();
        }
    }

    private bool TrySubscribeOnCharacterDeath()
    {
        if (_character != null)
        {
            if (_character is Defender defender)
            {
                defender.Died += RemoveCharacter;
            }

            else if (_character is Attacker attacker)
            {
                attacker.Died += RemoveCharacter;
            }

            Debug.Log($"Subscribed on death of: {_character.name}");
            return true;
        }

        else
        {
            return false;
        }
    }

    private void UnsubscribeFromCharacterDeath()
    {
        if (_character != null)
        {
            if (_character is Defender defender)
            {
                defender.Died -= RemoveCharacter;
            }

            else if (_character is Attacker attacker)
            {
                attacker.Died -= RemoveCharacter;
            }
        }
    }

    private void RemoveCharacter (Character character)
    {
        if (character != null && character == _character)
        {
            Debug.Log($"Removed: {_character.name} from cell: {this.name}");
            UnsubscribeFromCharacterDeath();
            Clear();
            FillChanged.Invoke();
        }
    }
}
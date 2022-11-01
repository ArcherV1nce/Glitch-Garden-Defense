using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Cell : MonoBehaviour
{
    private const float SizeMin = 1f;

    [SerializeField] private float _size;
    [SerializeField] private Character _character;

    private Collider2D _trigger;
    private Line _line;

    public UnityEvent FillChanged;
    public float Size => _size;
    public bool IsFree => _character != null;
    public Character Character => _character;

    private void Awake()
    {
        _trigger = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        TrySubscribeOnCharacterDeath();
    }

    private void OnDisable()
    {
        UnsubscribeFromCharacterDeath();
    }

    private void OnDestroy()
    {
        RemoveCharacter(_character);
    }

    private void OnValidate()
    {
        ValidateSize();
    }

    public bool TryAddCharacter (Character character)
    {
        if (IsFree)
        {
            _character = character;
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

    private bool TrySubscribeOnCharacterDeath()
    {
        if (_character != null)
        {
            _character.Died += (RemoveCharacter);
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
            _character.Died -= (RemoveCharacter);
        }
    }

    private void RemoveCharacter (Character character)
    {
        if (character != null && character == _character)
        {
            _line.RemoveCharacter(character);
            UnsubscribeFromCharacterDeath();
            Clear();
            FillChanged.Invoke();
        }
    }
}
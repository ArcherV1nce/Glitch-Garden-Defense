using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _direction;

    private Coroutine _moveRoutine;

    public bool IsMoving { get; private set; }
    public float Speed => _speed;
    public Vector2 Direction => _direction;
    public bool MoveRoutineNull => _moveRoutine == null;

    protected virtual void OnEnable()
    {
        Setup();
    }

    protected virtual void OnDisable()
    {
        StopMovement();
    }

    protected virtual void Setup ()
    {
        StartMovement();
    }

    protected virtual void SetMovementState(bool isMoving)
    {
        IsMoving = isMoving;
    }

    protected virtual IEnumerator Move ()
    {
        while (IsMoving)
        {
            transform.Translate(Direction * Speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        StopCoroutine(_moveRoutine);
    }

    protected void StartMovement()
    {
        if (MoveRoutineNull)
        {
            IsMoving = true;
            _moveRoutine = StartCoroutine(Move());
        }
    }

    protected void StopMovement()
    {
        if (MoveRoutineNull == false)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }
    }
}
using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class AnimationControl : MonoBehaviour
{
    private Animator _animator;

    public Animator Animator=> _animator;

    protected virtual void Awake()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        _animator = GetComponent<Animator>();
    }
}

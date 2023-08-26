using UnityEngine;
using UnityEngine.Events;

public class FoxEvasionSkill : MonoBehaviour
{
    private Collider2D _triggerArea;

    public UnityEvent<Projectile> EvasionTriggered;

    private void Awake()
    {
        Setup();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Projectile>(out Projectile projectile))
        {
            EvasionTriggered?.Invoke(projectile);
        }
    }

    private void Setup()
    {
        _triggerArea = GetComponent<Collider2D>();
    }
}
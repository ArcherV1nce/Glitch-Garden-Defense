using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private Damage _damage;
    [SerializeField] [Range(0.1f, 10f)] 
        private float _lifeTime = 3f;
    [SerializeField] private bool _isPenetrating = false;

    public Damage Damage => _damage;
    public bool IsPenetrating => _isPenetrating;
    public float LifeTime => _lifeTime;

    protected virtual void Update()
    {
        DestroyByCountdown();
    }

    protected virtual void OnTriggerEnter2D(Collider2D damageReceiver)
    {
        TryDealDamage(damageReceiver);
    }

    protected virtual bool TryDealDamage (Collider2D damageReceiver)
    {
        if (TryGetCharacterOnCollision(damageReceiver, out Character character))
        {
            DealDamage(character);
            return true;
        }

        else
        {
            return false;
        }
    }

    protected void DealDamage(Character damageReciever)
    {
        damageReciever.TakeDamage(Damage);
        DestroyByPenetration();
    }

    protected bool TryGetCharacterOnCollision (Collider2D collision, out Character character)
    {
        if (collision.TryGetComponent<Character>(out character))
        {
            return true;
        }

        else return false;
    }

    private void DestroyByPenetration ()
    {
        if (!IsPenetrating)
        {
            Destroy(gameObject);
        }
    }

    private void DestroyByCountdown ()
    {
        _lifeTime -= Time.deltaTime;

        if (LifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
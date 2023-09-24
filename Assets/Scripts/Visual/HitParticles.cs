using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HitParticles : MonoBehaviour
{
    private const float ParticlesLifetimeMin = 0f;
    private const float ParticlesLifetimeMax = 2f;

    [SerializeField, Range(ParticlesLifetimeMin, ParticlesLifetimeMax)] 
    private float _particlesLifetime = 1f;
    [SerializeField] private Attacker _attacker;

    private ParticleSystem _particles;
    private Coroutine _particlesPlayback;
    private Coroutine _deathRoutine;

    private void Awake()
    {
        Setup();
    }

    private void OnValidate()
    {
        ValidateParticleSystem();
        ValidateParticleLifetime();
    }

    private void OnEnable()
    {
        SubscribeToAttacker();
    }

    private void OnDisable()
    {
        UnsubscribeFromAttacker();
    }

    private IEnumerator PlayHitParticles()
    {
        _particles.Play();
        yield return new WaitForSeconds(_particlesLifetime);
        StopParticlesPlayback();
    }

    private IEnumerator DestroyAfterPlayback()
    {
        yield return new WaitForSeconds(_particlesLifetime);
        DestroyParticles();
    }

    private void Setup()
    {
        _attacker = GetComponentInParent<Attacker>();
        ValidateParticleSystem();
        ValidateParticleLifetime();
        _particles.Stop();
    }

    private void ValidateParticleSystem()
    {
        if (_particles == null)
        {
            _particles = GetComponent<ParticleSystem>();
        }
    }

    private void ValidateParticleLifetime()
    {
        _particles.Stop();
        ParticleSystem.MainModule particlesSettings = _particles.main;
        particlesSettings.duration = _particlesLifetime;
        particlesSettings.startLifetime = _particlesLifetime;
    }

    private void OnAttacked()
    {
        _particlesPlayback ??= StartCoroutine(PlayHitParticles());
    }

    private void OnDied(Attacker attacker)
    {
        gameObject.transform.parent = null;
        StartCoroutine(DestroyAfterPlayback());
    }

    private void StopParticlesPlayback()
    {
        _particles.Stop();

        if (_particlesPlayback != null)
        {
            StopCoroutine(_particlesPlayback);
            _particlesPlayback = null;
        }
    }

    private void DestroyParticles()
    {
        if (_deathRoutine != null)
        {
            StopCoroutine(_deathRoutine);
            _deathRoutine = null;
        }
        Destroy(gameObject);
    }

    private void SubscribeToAttacker()
    {
        _attacker.Attacked += OnAttacked;
        _attacker.Died += OnDied;
    }

    private void UnsubscribeFromAttacker()
    {
        _attacker.Attacked -= OnAttacked;
        _attacker.Died -= OnDied;
    }
}
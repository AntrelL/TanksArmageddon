using System;
using TanksArmageddon;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem _groundCollisionFX;
    [SerializeField] private int _boundaryOfDestruction = -50;

    public static Transform CurrentEnemyBullet { get; private set; }

    public static event Action EnemyBulletDestroyed;
    public static event Action GroundHit;
    public static event Action<int> PlayerHit;

    private LandCutter _landCutter;

    private void Start()
    {
        _landCutter = FindObjectOfType<LandCutter>();
        CurrentEnemyBullet = transform;
    }

    private void Update()
    {
        if (transform.position.y < - _boundaryOfDestruction)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            int damage = GetRandomDamage();
            PlayerHit(damage);
            player.PlayHitEffect(transform.position);
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Land land))
        {
            _landCutter.transform.position = transform.position;
            GroundHit?.Invoke();
            Invoke(nameof(DoCut), 0.001f);
        }
    }

    private int GetRandomDamage()
    {
        int randomDamage = UnityEngine.Random.Range(0, 100);

        if (randomDamage < 60)
        {
            return 100;
        }
        else if (randomDamage < 80)
        {
            return 200;
        }
        else if (randomDamage < 90)
        {
            return 250;
        }
        else
        {
            return 500;
        }
    }

    private void DoCut()
    {
        ParticleSystem flash = Instantiate(_groundCollisionFX, transform.position, transform.rotation);
        flash.Play();
        Destroy(flash.gameObject, flash.main.duration);

        _landCutter.DoCut();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EnemyBulletDestroyed?.Invoke();
    }
}
using System;
using TanksArmageddon;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem _groundCollisionFX;

    private Cutter _cutter;

    public static Transform CurrentEnemyBullet { get; private set; }

    private void Start()
    {
        _cutter = FindObjectOfType<Cutter>();
        CurrentEnemyBullet = transform;
    }

    private void Update()
    {
        if (transform.position.y < -50) Destroy(gameObject);
    }

    private void OnDestroy()
    {
        EnemyBulletDestroyed?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            var damage = GetRandomDamage();
            PlayerHit(damage);
            player.PlayHitEffect(transform.position);
            Debug.Log("Hit player");
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            Debug.Log("Enemy hit enemy");
            Destroy(gameObject);
        }

        if (collision.gameObject.TryGetComponent(out Land land))
        {
            _cutter.transform.position = transform.position;
            GroundHit?.Invoke();
            Debug.Log("Hit land");
            Invoke(nameof(DoCut), 0.001f);
        }
    }

    public static event Action EnemyBulletDestroyed;
    public static event Action GroundHit;
    public static event Action<int> PlayerHit;

    private int GetRandomDamage()
    {
        var randomDamage = Random.Range(0, 100);

        if (randomDamage < 60)
            return 100;
        if (randomDamage < 80)
            return 200;
        if (randomDamage < 90)
            return 250;
        return 500;
    }

    private void DoCut()
    {
        Debug.Log("DoCut beep");
        var flash = Instantiate(_groundCollisionFX, transform.position, transform.rotation);
        flash.Play();
        Destroy(flash.gameObject, flash.main.duration);

        _cutter.DoCut();
        Destroy(gameObject);
    }
}
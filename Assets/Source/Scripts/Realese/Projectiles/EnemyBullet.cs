using System;
using Source.Scripts.Release.Enemy;
using Source.Scripts.Release.LandCutter;
using Source.Scripts.Release.Player;
using Source.Scripts.Release.Stuff;
using UnityEngine;

namespace Source.Scripts.Release.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBullet : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _groundCollisionFX;

        private AudioManager _manager;
        private LandCutter.LandCutter _landCutter;

        public event Action Destroyed;
    
        public Transform BulletTransform => transform;

        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
        }

        private void Start()
        {
            _landCutter = FindObjectOfType<LandCutter.LandCutter>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerHealth player))
            {
                int damage = GetRandomDamage();
                player.TakeDamage(damage);
                player.PlayHitEffect(transform.position);
                Destroy(gameObject);
            }

            if (collision.gameObject.TryGetComponent(out EnemyFacade enemy))
            {
                Destroy(gameObject);
            }

            if (collision.gameObject.TryGetComponent(out Land land))
            {
                _landCutter.transform.position = transform.position;
                _manager.PlayTankHit();
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
            Destroyed?.Invoke();
        }
    }
}
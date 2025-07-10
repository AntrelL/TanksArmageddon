using System;
using System.Collections.Generic;
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
        private const float LandCutDelay = 0.001f;
        private const int MinPercentageValue = 0;
        private const int MaxPercentageValue = 100;
        
        [SerializeField] private ParticleSystem _groundCollisionFX;

        private readonly List<(int Value, int UpperRangeLimit)> _damageTable = 
            new List<(int Value, int UpperRangeLimit)>
            {
                (100, 60),
                (200, 80),
                (250, 90),
                (500, 100)
            };
        
        private AudioManager _manager;
        private LandCutter.LandCutter _landCutter;

        public event Action Destroyed;

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
                Invoke(nameof(DoCut), LandCutDelay);
            }
        }

        private int GetRandomDamage()
        {
            int damageChancePercent = UnityEngine.Random.Range(MinPercentageValue, MaxPercentageValue);

            foreach (var damageChanceRange in _damageTable)
            {
                if (damageChancePercent < damageChanceRange.UpperRangeLimit)
                    return damageChanceRange.Value;
            }

            return _damageTable[0].Value;
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
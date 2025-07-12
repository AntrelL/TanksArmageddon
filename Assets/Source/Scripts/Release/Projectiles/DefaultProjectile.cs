using Source.Scripts.Release.Enemy;
using Source.Scripts.Release.HitProcessing;
using Source.Scripts.Release.LandCutter;
using Source.Scripts.Release.Player;
using Source.Scripts.Release.Stuff;
using UnityEngine;

namespace Source.Scripts.Release.Projectiles
{
    public class DefaultProjectile : MonoBehaviour
    {
        private const float LandCutDelay = 0.001f;
        private const int MinWorldY = -50;

        private readonly bool _isDead;

        [SerializeField] private ParticleSystem _groundCollisionFX;
        [SerializeField] private float _speed;

        private Rigidbody2D _rigidbody;
        private LandCutter.LandCutter _landCutter;
        private AudioManager _manager;

        private float _targetX;
        private float _targetY;

        public Transform CurrentProjectile { get; private set; }

        public bool IsEnemyProjectile { get; set; } = false;

        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rigidbody.velocity = transform.right * _speed;
            _landCutter = FindObjectOfType<LandCutter.LandCutter>();
            CurrentProjectile = transform;
        }

        private void Update()
        {
            transform.right = _rigidbody.velocity;

            if (transform.position.y < MinWorldY)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (ProjectileTracker.Instance != null &&
                ProjectileTracker.Instance.CurrentProjectile == transform)
            {
                ProjectileTracker.Instance.ClearProjectile();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IImpactTarget impactTarget))
                OnHitImpactTarget(impactTarget);

            if (collision.gameObject.TryGetComponent<Land>(out _))
            {
                _landCutter.transform.position = transform.position;
                Invoke(nameof(DoCut), LandCutDelay);
            }
        }

        private void OnHitImpactTarget(IImpactTarget impactTarget)
        {
            if (impactTarget is EdgeOfMap)
            {
                _manager.PlayButtonClick();
                Destroy(gameObject);
            }

            if (impactTarget is IHealthImpactTarget target)
                OnHitHealthImpactTarget(target);
        }

        private void OnHitHealthImpactTarget(IHealthImpactTarget healthImpactTarget)
        {
            switch (healthImpactTarget)
            {
                case PlayerRoot when !IsEnemyProjectile:
                case EnemyFacade when IsEnemyProjectile:
                    return;
            }

            healthImpactTarget.Health.PlayHitEffect(transform.position);
            Destroy(gameObject);
        }

        private void DoCut()
        {
            ParticleSystem flash = Instantiate(_groundCollisionFX, transform.position, transform.rotation);
            flash.Play();
            Destroy(flash.gameObject, flash.main.duration);

            _landCutter.DoCut();
            _manager.PlayTankHit();
            Destroy(gameObject);
        }
    }
}

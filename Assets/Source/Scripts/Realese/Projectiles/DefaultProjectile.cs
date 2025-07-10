using Source.Scripts.Release.Enemy;
using Source.Scripts.Release.LandCutter;
using Source.Scripts.Release.Player;
using Source.Scripts.Release.Stuff;
using UnityEngine;

namespace Source.Scripts.Release.Projectiles
{
    public class DefaultProjectile : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _groundCollisionFX;
        [SerializeField] private float _speed;

        private readonly bool _isDead;

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

            if (transform.position.y < -50)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out EdgeOfMap edgeOfMap))
            {
                _manager.PlayButtonClick();
                Destroy(gameObject);
            }

            if (IsEnemyProjectile)
            {
                if (collision.gameObject.TryGetComponent(out PlayerHealth player))
                {
                    player.PlayHitEffect(transform.position);
                    Destroy(gameObject);
                }
            }
            else
            {
                if (collision.gameObject.TryGetComponent(out EnemyFacade enemy))
                {
                    enemy.PlayHitEffect(transform.position);
                    Destroy(gameObject);
                }
            }

            if (collision.gameObject.TryGetComponent<Land>(out _))
            {
                _landCutter.transform.position = transform.position;
                Invoke(nameof(DoCut), 0.001f);
            }
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

        private void OnDestroy()
        {
            if (ProjectileTracker.Instance != null && ProjectileTracker.Instance.CurrentProjectile == transform)
            {
                ProjectileTracker.Instance.ClearProjectile();
            }
        }
    }
}
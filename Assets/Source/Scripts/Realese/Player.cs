using System;
using Assets.Constructors.FuturisticTanks.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG;

namespace TanksArmageddon
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Tank _tank;
        [SerializeField] private float _force;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _availableTravelTime;
        [SerializeField] private LayerMask _landLayer;
        [SerializeField] private Slider _petrolTank;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private ParticleSystem _hitFX;

        [Space] [Header("New Physics")] [SerializeField]
        private Transform _centerPoint;

        [SerializeField] private Transform _leftPoint;
        [SerializeField] private Transform _rightPoint;

        private float _baseDrag;
        private bool _canMove;
        private readonly float _checkRaycastLenght = 0.8f;

        private int _currentHealth;
        private Vector3 _forceDirection;
        private bool _isAlive = true;
        private bool _leftButtonPressed;

        private int _maxHealth;
        private bool _rightButtonPressed;

        private Vector3 _selectedPointPosition;
        private float _travelTimeSpent;

        private void Awake()
        {
            _baseDrag = _rigidbody2D.drag;
            _maxHealth = GameManager.Instance.GetPlayerMaxHealth();
            _currentHealth = _maxHealth;
        }

        private void Start()
        {
            _petrolTank.maxValue = _availableTravelTime;
            _petrolTank.value = _availableTravelTime;

            if (YG2.envir.isMobile)
            {
                AddEventTrigger(_leftButton.gameObject, EventTriggerType.PointerDown, () => _leftButtonPressed = true);
                AddEventTrigger(_leftButton.gameObject, EventTriggerType.PointerUp, () => _leftButtonPressed = false);
                AddEventTrigger(_rightButton.gameObject, EventTriggerType.PointerDown,
                    () => _rightButtonPressed = true);
                AddEventTrigger(_rightButton.gameObject, EventTriggerType.PointerUp, () => _rightButtonPressed = false);
            }
        }

        private void FixedUpdate()
        {
            if (_currentHealth <= 0)
            {
                _tank.Destroy();
                _isAlive = false;
                gameObject.SetActive(false);
                Defeated?.Invoke();

                return;
            }

            if (!_canMove)
            {
                _rigidbody2D.drag = 100f;

                return;
            }

            var horizontalInput = Input.GetAxis("Horizontal");

            if (_leftButtonPressed)
                horizontalInput = -1f;
            else if (_rightButtonPressed) horizontalInput = 1f;

            if (_travelTimeSpent >= _availableTravelTime)
            {
                _rigidbody2D.drag = 100f;
                _tank.Idle();

                return;
            }

            if (horizontalInput != 0)
            {
                _travelTimeSpent += Time.fixedDeltaTime;
                _tank.Move();
            }
            else
            {
                _tank.Idle();
            }

            _petrolTank.value = _availableTravelTime - _travelTimeSpent;


            _rigidbody2D.centerOfMass = _centerPoint.localPosition;

            if (horizontalInput != 0)
            {
                _rigidbody2D.drag = _baseDrag;
                _selectedPointPosition = horizontalInput == 1f ? _rightPoint.position : _leftPoint.position;
                var hit = Physics2D.Raycast(_selectedPointPosition, -Vector2.up, _checkRaycastLenght, _landLayer);

                if (hit.collider == null)
                {
                    _rigidbody2D.AddForceAtPosition(horizontalInput * Vector2.right * _force, _selectedPointPosition);
                    _rigidbody2D.gravityScale = 10f;
                    hit = Physics2D.Raycast(_centerPoint.position, -Vector2.up, _checkRaycastLenght, _landLayer);
                }

                var direction = new Vector2();

                if (hit.collider != null)
                {
                    _rigidbody2D.gravityScale = 1f;
                    direction = Vector2.right * horizontalInput;
                    direction = direction - Vector2.Dot(direction, hit.normal) * hit.normal;
                }
                else
                {
                    direction = transform.right * horizontalInput;
                    _selectedPointPosition = transform.position;
                }

                _forceDirection = direction.normalized;
                _rigidbody2D.AddForceAtPosition(direction.normalized * _force, _selectedPointPosition);
            }
            else
            {
                _rigidbody2D.drag = 100f;
            }

            if (_rigidbody2D.velocity.magnitude > _maxSpeed)
                _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _maxSpeed;
        }

        private void OnEnable()
        {
            _cameraController.UnlockMovement += OnMovementUnlocked;
            TurnManager.CanPlayerControl += OnMovementUnlocked;
            EdgeOfMap.CollisionWithPlayer += TakeDamage;
            EnemyBullet.PlayerHit += TakeDamage;
        }

        private void OnDisable()
        {
            _cameraController.UnlockMovement -= OnMovementUnlocked;
            EdgeOfMap.CollisionWithPlayer -= TakeDamage;
            TurnManager.CanPlayerControl -= OnMovementUnlocked;
            EnemyBullet.PlayerHit -= TakeDamage;
        }

        public static event Action PlayerHit;
        public event Action<int> HealthChanged;
        public event Action Defeated;

        private void DisableMovement(bool isMovementDisable)
        {
            _canMove = isMovementDisable;
        }

        private void OnMovementUnlocked(bool canPlayerMove)
        {
            _canMove = canPlayerMove;
            _travelTimeSpent = 0f;
            _petrolTank.value = _availableTravelTime;
        }

        private void AddEventTrigger(GameObject target, EventTriggerType eventType, Action action)
        {
            var trigger = target.GetComponent<EventTrigger>() ?? target.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry {eventID = eventType};
            entry.callback.AddListener(data => action());
            trigger.triggers.Add(entry);
        }

        private void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth);
        }

        public void PlayHitEffect(Vector3 hitPosition)
        {
            if (_isAlive)
            {
                PlayerHit?.Invoke();
                var flash = Instantiate(_hitFX, hitPosition, Quaternion.identity);
                flash.Play();
                Destroy(flash.gameObject, flash.main.duration);
            }
        }
    }
}
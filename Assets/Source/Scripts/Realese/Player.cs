using Assets.Constructors.FuturisticTanks.Scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        [SerializeField] private int _maxHealth = 1000;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private ParticleSystem _hitFX;

        private float _travelTimeSpent;
        private bool _leftButtonPressed = false;
        private bool _rightButtonPressed = false;
        private bool _canMove = false;

        private int _currentHealth;
        private bool _isAlive = true;

        public static event Action ButtonClicked;
        public static event Action PlayerHit;
        public event Action<int> HealthChanged;
        public event Action Defeated;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _rigidbody2D.centerOfMass = _centerOfMass.localPosition;
        }

        private void Start()
        {
            _petrolTank.maxValue = _availableTravelTime;
            _petrolTank.value = _availableTravelTime;

            AddEventTrigger(_leftButton.gameObject, EventTriggerType.PointerDown, () => _leftButtonPressed = true);
            AddEventTrigger(_leftButton.gameObject, EventTriggerType.PointerUp, () => _leftButtonPressed = false);
            AddEventTrigger(_rightButton.gameObject, EventTriggerType.PointerDown, () => _rightButtonPressed = true);
            AddEventTrigger(_rightButton.gameObject, EventTriggerType.PointerUp, () => _rightButtonPressed = false);
        }

        private void FixedUpdate()
        {
            if (_currentHealth <= 0)
            {
                Debug.Log("Player is defeated!");
                _tank.Destroy();
                _isAlive = false;
                gameObject.SetActive(false);
                Defeated?.Invoke();

                return;
            }

            if (!_canMove) return;

            float horizontalInput = Input.GetAxis("Horizontal");

            if (_leftButtonPressed)
            {
                horizontalInput = -1f;
            }
            else if (_rightButtonPressed)
            {
                horizontalInput = 1f;
            }

            if (_travelTimeSpent >= _availableTravelTime)
            {
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

            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1f, _landLayer);

            if (hit.collider != null)
            {
                Vector2 direction = Vector2.right * horizontalInput;
                direction = direction - (Vector2.Dot(direction, hit.normal) * hit.normal);

                _rigidbody2D.AddForce(direction * _force);
            }

            if (_rigidbody2D.velocity.magnitude > _maxSpeed)
                _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _maxSpeed;
        }

        private void OnEnable()
        {
            _cameraController.UnlockMovement += OnMovementUnlocked;
            TurnManager.CanPlayerControl += OnMovementUnlocked;
            EnemyBullet.PlayerHit += TakeDamage;
        }

        private void OnDisable()
        {
            _cameraController.UnlockMovement -= OnMovementUnlocked;
            TurnManager.CanPlayerControl -= OnMovementUnlocked;
            EnemyBullet.PlayerHit -= TakeDamage;
        }

        private void OnDrawGizmos()
        {
            // Отрисовываем рейкаст, как он используется в FixedUpdate
            Gizmos.color = Color.red;
            Vector2 rayDirection = -Vector2.up;
            float rayLength = 1f;
            Gizmos.DrawRay(transform.position, rayDirection * rayLength);

            // Выполняем рейкаст для определения точки столкновения
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayLength, _landLayer);

            if (hit.collider != null)
            {
                // Отрисовываем точку столкновения зелёным шариком
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(hit.point, 0.1f);
            }
        }

        private void OnMovementUnlocked(bool canPlayerMove)
        {
            _canMove = canPlayerMove;
        }

        private void AddEventTrigger(GameObject target, EventTriggerType eventType, System.Action action)
        {
            EventTrigger trigger = target.GetComponent<EventTrigger>() ?? target.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
            entry.callback.AddListener((data) => action());
            trigger.triggers.Add(entry);
        }

        private void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            HealthChanged?.Invoke(_currentHealth);
        }

        public void PlayHitEffect(Vector3 hitPosition)
        {
            if (_isAlive == true)
            {
                PlayerHit?.Invoke();
                ParticleSystem flash = Instantiate(_hitFX, hitPosition, Quaternion.identity);
                flash.Play();
                Destroy(flash.gameObject, flash.main.duration);
            }
            else
            {
                return;
            }
        }
    }
}

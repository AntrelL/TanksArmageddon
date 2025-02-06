using Assets.Constructors.FuturisticTanks.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TanksArmageddon
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Tank _tank;
        [SerializeField] private float _force;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _availableTravelTime;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Slider _petrolTank;
        [SerializeField] private int _maxHealth = 1000;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private CameraController _cameraController;

        private float _travelTimeSpent;
        private bool _leftButtonPressed = false;
        private bool _rightButtonPressed = false;
        private bool _canMove = false;

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
                return;

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

            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1f, _layerMask);

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
        }

        private void OnDisable()
        {
            _cameraController.UnlockMovement -= OnMovementUnlocked;
        }

        private void OnMovementUnlocked()
        {
            _canMove = true;
        }

        private void AddEventTrigger(GameObject target, EventTriggerType eventType, System.Action action)
        {
            EventTrigger trigger = target.GetComponent<EventTrigger>() ?? target.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
            entry.callback.AddListener((data) => action());
            trigger.triggers.Add(entry);
        }
    }
}

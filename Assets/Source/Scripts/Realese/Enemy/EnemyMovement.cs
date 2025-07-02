using Assets.Constructors.FuturisticTanks.Scripts;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _movementForce = 15f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _availableTravelTime = 3f;
    [SerializeField] private LayerMask _landLayer;
    [SerializeField] private Tank _tank;

    [Header("Physics Points")]
    [SerializeField] private Transform _centerPoint;
    [SerializeField] private Transform _leftPoint;
    [SerializeField] private Transform _rightPoint;

    private float _baseDrag;
    private float _movementTimeUsed;
    private float _moveDirection;
    private readonly float _checkRaycastLength = 0.8f;
    private Vector3 _selectedPointPosition;

    private void Awake()
    {
        _baseDrag = _rigidbody2D.drag;
    }

    public void StartMovement(float direction)
    {
        _movementTimeUsed = 0f;
        _moveDirection = direction;
    }

    public void StopMovement()
    {
        _moveDirection = 0f;
    }

    private void FixedUpdate()
    {
        if (_movementTimeUsed >= _availableTravelTime || _moveDirection == 0f)
        {
            _rigidbody2D.drag = 100f;
            _tank.Idle();
            return;
        }

        _rigidbody2D.drag = _baseDrag;
        _rigidbody2D.centerOfMass = _centerPoint.localPosition;

        _selectedPointPosition = _moveDirection > 0 ? _rightPoint.position : _leftPoint.position;
        RaycastHit2D hit = Physics2D.Raycast(_selectedPointPosition, -Vector2.up, _checkRaycastLength, _landLayer);

        if (hit.collider == null)
        {
            _rigidbody2D.AddForceAtPosition(Vector2.right * (_moveDirection * _movementForce), _selectedPointPosition);
            _rigidbody2D.gravityScale = 10f;
            hit = Physics2D.Raycast(_centerPoint.position, -Vector2.up, _checkRaycastLength, _landLayer);
        }

        Vector2 direction;

        if (hit.collider != null)
        {
            _rigidbody2D.gravityScale = 1f;
            direction = Vector2.right * _moveDirection - Vector2.Dot(Vector2.right * _moveDirection, hit.normal) * hit.normal;
        }
        else
        {
            direction = transform.right * _moveDirection;
            _selectedPointPosition = transform.position;
        }

        _rigidbody2D.AddForceAtPosition(direction.normalized * _movementForce, _selectedPointPosition);

        if (_rigidbody2D.velocity.magnitude > _maxSpeed)
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _maxSpeed;

        _tank.Move();
        _movementTimeUsed += Time.fixedDeltaTime;
    }
}

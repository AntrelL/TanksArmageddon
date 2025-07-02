using Assets.Constructors.FuturisticTanks.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Transform _centerPoint, _leftPoint, _rightPoint;
    [SerializeField] private float _checkRaycastLength = 0.8f;
    [SerializeField] private LayerMask _landLayer;
    [SerializeField] private float _force;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private Tank _tank;
    [SerializeField] private Slider _petrolTank;

    private float _travelTimeSpent;
    private float _baseDrag;
    private float _availableTravelTime;
    private bool _canMove;

    public void Initialize(float travelTime)
    {
        _availableTravelTime = travelTime;
        _petrolTank.maxValue = travelTime;
        _petrolTank.value = travelTime;
    }

    private void Awake()
    {
        _baseDrag = _rigidbody2D.drag;
    }

    private void FixedUpdate()
    {
        if (!_canMove) {
            _rigidbody2D.drag = 100f;
            return;
        }

        float input = Input.GetAxis("Horizontal");
        if (_travelTimeSpent >= _availableTravelTime)
        {
            _rigidbody2D.drag = 100f;
            _tank.Idle();
            return;
        }

        if (input != 0f)
        {
            _travelTimeSpent += Time.fixedDeltaTime;
            _tank.Move();
            ApplyForce(input);
        }
        else
        {
            _tank.Idle();
            _rigidbody2D.drag = 100f;
        }

        _petrolTank.value = _availableTravelTime - _travelTimeSpent;

        if (_rigidbody2D.velocity.magnitude > _maxSpeed)
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _maxSpeed;
    }

    private void ApplyForce(float directionInput)
    {
        _rigidbody2D.centerOfMass = _centerPoint.localPosition;
        _rigidbody2D.drag = _baseDrag;

        Vector3 selectedPoint = directionInput > 0 ? _rightPoint.position : _leftPoint.position;
        RaycastHit2D hit = Physics2D.Raycast(selectedPoint, -Vector2.up, _checkRaycastLength, _landLayer);

        if (hit.collider == null)
        {
            _rigidbody2D.AddForceAtPosition(Vector2.right * directionInput * _force, selectedPoint);
            _rigidbody2D.gravityScale = 10f;
            hit = Physics2D.Raycast(_centerPoint.position, -Vector2.up, _checkRaycastLength, _landLayer);
        }

        Vector2 forceDir = hit.collider != null
            ? Vector2.right * directionInput - Vector2.Dot(Vector2.right * directionInput, hit.normal) * hit.normal
            : (Vector2) (transform.right * directionInput);

        _rigidbody2D.gravityScale = hit.collider != null ? 1f : 10f;
        _rigidbody2D.AddForceAtPosition(forceDir.normalized * _force, selectedPoint);
    }

    public void SetCanMove(bool value)
    {
        _canMove = value;
        if (value)
        {
            _travelTimeSpent = 0;
            _petrolTank.value = _availableTravelTime;
        }
    }
}

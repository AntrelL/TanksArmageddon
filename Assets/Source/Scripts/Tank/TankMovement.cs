using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [SerializeField] [Min(0)] private float _maxSpeed;
    [SerializeField] [Min(0)] private float _force;
    [SerializeField] [Min(0)] private float _motionSmoothingSpeed;
    [SerializeField] [Range(0, 90)] private float _maxMovementElevationAngle;
    [SerializeField] [Min(0)] private float _groundRaycastDistance;
    [SerializeField] [Min(0)] private float _sideGroundRaycastDistance;
    [SerializeField] private Transform _leftGroundCheckPoint;
    [SerializeField] private Transform _rightGroundCheckPoint;
    [SerializeField] private LayerMask _groundLayerMask;

    private Rigidbody2D _rigidbody2D;
    private Transform _transform;
    private float _motionSmoothingValue;
    private float _maxSmoothingValue = 100;

    private Vector2 _movementDirectionForDraw;

    public void Construct(Rigidbody2D rigidbody2D)
    {
        _rigidbody2D = rigidbody2D;
        _transform = transform;

        _motionSmoothingValue = 0;
        _rigidbody2D.centerOfMass = Vector2.zero;
    }

    public void Move(int direction, float deltaTime)
    {
        if (direction == 0)
        {
            _motionSmoothingValue -= _motionSmoothingSpeed * deltaTime;

            if (_motionSmoothingValue > 0)
                return;

            _motionSmoothingValue = 0;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;

            return;
        }

        _rigidbody2D.constraints = RigidbodyConstraints2D.None;

        _motionSmoothingValue += _motionSmoothingSpeed * deltaTime;
        _motionSmoothingValue = Mathf.Min(_motionSmoothingValue, _maxSmoothingValue);

        if (direction < -1 || direction > 1)
        {
            Debug.LogError("Direction must be in the range [-1;1]");
            return;
        }

        if (TryToRaycastToGround(direction, out RaycastHit2D hit))
        {
            Vector2 movementDirection = Vector2.right * direction;
            movementDirection = movementDirection - (Vector2.Dot(movementDirection, hit.normal) * hit.normal);

            movementDirection = LimitElevationDirection(movementDirection, _maxMovementElevationAngle);

            _movementDirectionForDraw = movementDirection;
            _rigidbody2D.AddForce(movementDirection * _force * deltaTime);
        }

        if (_rigidbody2D.velocity.magnitude > _maxSpeed)
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _maxSpeed;
    }

    private bool TryToRaycastToGround(int inputDirection, out RaycastHit2D hit)
    {
        Vector2 startRayPosition = (inputDirection == 1 ? _rightGroundCheckPoint : _leftGroundCheckPoint).position;
        hit = Physics2D.Raycast(startRayPosition, -Vector2.up, _sideGroundRaycastDistance, _groundLayerMask);

        if (hit.collider == null)
            hit = Physics2D.Raycast(_transform.position, -Vector2.up, _groundRaycastDistance, _groundLayerMask);

        return hit.collider != null;
    }

    private Vector2 LimitElevationDirection(Vector2 direction, float maxElevationAngle)
    {
        float elevationAngle = 90 - Vector2.Angle(direction, _transform.right);

        if (elevationAngle <= _maxMovementElevationAngle)
            return direction;

        float signedAngle = Vector2.SignedAngle(_transform.right, direction);
        float angleToMaxElevation = (90 - maxElevationAngle) * Mathf.Sign(signedAngle);

        direction = Quaternion.AngleAxis(angleToMaxElevation, _transform.forward) * _transform.right;
        return direction.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector3(_movementDirectionForDraw.x, _movementDirectionForDraw.y) * 5f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.right * 3f);
    }
}

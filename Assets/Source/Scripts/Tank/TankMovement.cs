using UnityEngine;

namespace TanksArmageddon.TankComponents
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TankMovement : MonoBehaviour
    {
        [SerializeField][Min(0)] private float _maxSpeed;
        [SerializeField][Min(0)] private float _force;
        [SerializeField][Min(0)] private float _motionSmoothingSpeed;
        [Space]
        [SerializeField][Range(0, 90)] private float _maxMovementElevationAngle;
        [SerializeField][Min(0)] private float _groundRaycastDistance;
        [SerializeField][Min(0)] private float _sideGroundRaycastDistance;
        [Space]
        [SerializeField] private Transform _centerOfMassPoint;
        [SerializeField] private Transform _chassis;
        [SerializeField] private Transform _leftGroundTestPoint;
        [SerializeField] private Transform _centerGroundTestPoint;
        [SerializeField] private Transform _rightGroundTestPoint;
        [SerializeField] private LayerMask _groundLayerMask;

        private Rigidbody2D _rigidbody2D;
        private Scale _motionSmoothingScale;

        private Vector2 _movementDirectionForDraw;

        public void Construct()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _motionSmoothingScale = new Scale(startValue: 0, minValue: 0, maxValue: 1);
            _rigidbody2D.centerOfMass = _centerOfMassPoint.localPosition;
        }

        public void Move(int direction, float deltaTime)
        {
            if (direction == 0)
            {
                _motionSmoothingScale.Value -= _motionSmoothingSpeed * deltaTime;

                if (_motionSmoothingScale.IsEmpty == false)
                    return;

                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
                return;
            }

            _rigidbody2D.constraints = RigidbodyConstraints2D.None;
            _motionSmoothingScale.Value += _motionSmoothingSpeed * deltaTime;

            if (TryRaycastToGround(direction, out RaycastHit2D hit))
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

        private bool TryRaycastToGround(int inputDirection, out RaycastHit2D hit)
        {
            Vector2 startRayPosition = (inputDirection == 1 ? _rightGroundTestPoint : _leftGroundTestPoint).position;
            hit = Physics2D.Raycast(startRayPosition, -Vector2.up, _sideGroundRaycastDistance, _groundLayerMask);

            if (hit.collider == null)
            {
                startRayPosition = _centerGroundTestPoint.position;
                hit = Physics2D.Raycast(startRayPosition, -Vector2.up, _groundRaycastDistance, _groundLayerMask);
            }
                
            return hit.collider != null;
        }

        private Vector2 LimitElevationDirection(Vector2 direction, float maxElevationAngle)
        {
            float elevationAngle = 90 - Vector2.Angle(direction, _chassis.right);

            if (elevationAngle <= _maxMovementElevationAngle)
                return direction;

            float signedAngle = Vector2.SignedAngle(_chassis.right, direction);
            float angleToMaxElevation = (90 - maxElevationAngle) * Mathf.Sign(signedAngle);

            direction = Quaternion.AngleAxis(angleToMaxElevation, _chassis.forward) * _chassis.right;
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
}

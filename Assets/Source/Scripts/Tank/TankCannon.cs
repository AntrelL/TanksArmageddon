using UnityEngine;

public class TankCannon : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    private float _targetAngle;
    private float _angle;
    private Transform _transform;

    [field: SerializeField] public float MinAngle { get; private set; }
    [field: SerializeField] public float MaxAngle { get; private set; }
    [field: SerializeField] public float StartAngle { get; private set; }

    private void OnValidate()
    {
        if (MaxAngle <= MinAngle)
            MaxAngle = MinAngle + 1;

        if (StartAngle > MaxAngle || StartAngle < MinAngle)
            StartAngle = MinAngle;
    }

    public void Update()
    {
        if (_angle == _targetAngle)
            return;

        _angle = Mathf.MoveTowards(_angle, _targetAngle, _rotationSpeed);

        Vector3 rotation = _transform.rotation.eulerAngles;
        rotation.z = _angle;

        _transform.localRotation = Quaternion.Euler(rotation);
    }

    public void Construct()
    {
        _targetAngle = StartAngle;
        _angle = StartAngle;
        _transform = transform;
    }

    public void SetTargetAngle(float angle)
    {
        if (angle > MaxAngle || angle < MinAngle)
        {
            Debug.LogError($"Angle must be in the range [{MinAngle};{MaxAngle}]");
            return;
        }

        _targetAngle = angle;
    }
}

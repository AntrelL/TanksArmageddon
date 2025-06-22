using RainyPlace;
using UnityEngine;

namespace TanksArmageddon
{
    public class TankCannon : MonoBehaviour
    {
        [SerializeField] private Transform _cannon;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _minAngle;
        [SerializeField] private float _maxAngle;
        [Space] 
        [SerializeField] private Transform _firePoint;

        public void Rotate(int direction, float deltaTime)
        {
            if (direction == 0)
                return;
            
            float currentAngle = NormalizeAngle(_cannon.transform.eulerAngles.z);
            float newAngle = currentAngle + direction * _rotationSpeed * deltaTime;
            ScaleFloat rotationScale = new ScaleFloat(newAngle, _minAngle, _maxAngle, true);
            _cannon.rotation = Quaternion.Euler(0f, 0f, rotationScale.Value);
        }
        
        public void Fire(Shell shell)
        {
            Shell launchedShell = Instantiate(shell, _firePoint.position, Quaternion.identity);
            launchedShell.Fire();
        }
        
        private float NormalizeAngle(float angle)
        {
            angle %= 360f;
            
            if (angle > 180f)
                angle -= 360f;
            
            return angle;
        }
    }
}

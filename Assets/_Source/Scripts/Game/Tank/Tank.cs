using RainyPlace;
using UnityEngine;

namespace TanksArmageddon
{
    public class Tank : MonoBehaviour
    {
        // TODO: Move to a separate entity
        [SerializeField] private float _maxFuel;
        [SerializeField] private float _fuelConsumptionPerSecond;
        
        [SerializeField] private TankMovement _tankMovement;
        [SerializeField] private TankCannon _tankCannon;

        private ITankController _controller;
        private ScaleFloat _fuelScale;
        
        public IReadonlyScaleFloat Fuel => _fuelScale;

        private bool IsFuelEmpty => _fuelScale.Value.ApproximatelyEquals(_fuelScale.Min);
        
        public void Init(ITankController controller)
        {
            _controller = controller;

            _fuelScale = new ScaleFloat(_maxFuel, 0, _maxFuel, true);
        }

        private void OnEnable()
        {
            _controller.ShotActivated += Shoot;
        }

        private void OnDisable()
        {
            _controller.ShotActivated -= Shoot;
        }

        private void Update()
        {
            _tankCannon.Rotate(_controller.CannonRotateDirection, Time.deltaTime);
            
            if (IsFuelEmpty)
                return;
            
            if (_controller.MovementDirection == 0)
                return;
            
            _tankMovement.Move(_controller.MovementDirection, Time.deltaTime);
            _fuelScale.Value -= _fuelConsumptionPerSecond * Time.deltaTime;
        }

        private void Shoot()
        {
            Debug.Log("пиу");
        }
    }
}

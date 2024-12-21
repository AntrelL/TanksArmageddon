using System.Collections.Generic;
using TanksArmageddon.TankControl;
using UnityEngine;

namespace TanksArmageddon.TankComponents
{
    [RequireComponent(typeof(TankMovement))]
    public class Tank : MonoBehaviour
    {
        [SerializeField][Min(0)] private float _fuelConsumptionRate;

        private TankMovement _movement;

        private Scale _fuel;

        private ITankController _controller;
        private List<TankPhysicalPart> _physicalParts;

        public void Construct(ITankController controller)
        {
            SetComponents();
            _controller = controller;

            _movement.Construct();
            _physicalParts = new List<TankPhysicalPart>(GetComponentsInChildren<TankPhysicalPart>());

            foreach (var part in _physicalParts)
            {
                part.Construct(this);
            }

            DisableTankPhysicalPartCollisions(_physicalParts);

            _fuel = new Scale(startValue: 100, minValue: 0, maxValue: 100);
        }

        public IReadOnlyScale Fuel => _fuel;

        private void Update()
        {
            int movementDirection = _controller.GetMovementDirection();

            if (movementDirection.IsInRange(-1, 1) == false)
            {
                Debug.LogError("Direction must be in the range [-1;1]");
                return;
            }

            if (_fuel.IsEmpty)
                movementDirection = 0;

            if (movementDirection != 0)
                _fuel.Value -= _fuelConsumptionRate * Time.deltaTime;

            _movement.Move(movementDirection, Time.deltaTime);
        }

        private void SetComponents()
        {
            _movement = GetComponent<TankMovement>();
        }

        private void DisableTankPhysicalPartCollisions(List<TankPhysicalPart> physicalParts)
        {
            for (int i = 0; i < physicalParts.Count; i++)
            {
                for (int j = i + 1; j < physicalParts.Count; j++)
                {
                    Physics2D.IgnoreCollision(physicalParts[i].Collider2D, physicalParts[j].Collider2D);
                }
            }
        }
    }
}

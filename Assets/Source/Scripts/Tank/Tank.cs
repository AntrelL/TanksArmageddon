using UnityEngine;
using System.Collections.Generic;
using TanksArmageddon.TankControl;
using TanksArmageddon.CompositeRoot;

namespace TanksArmageddon.TankComponents
{
    [RequireComponent(typeof(TankCannon))]
    [RequireComponent(typeof(TankMovement))]
    [RequireComponent(typeof(TankInventory))]
    [RequireComponent(typeof(TankSpecification))]
    [RequireComponent(typeof(TankAnimator))]
    [RequireComponent(typeof(TankEffector))]
    public class Tank : MonoScriptLinked, IConstructable<ITankController>
    {
        [SerializeField][Min(0)] private float _fuelConsumptionRate;

        private TankCannon _cannon;
        private TankMovement _movement;
        private TankInventory _inventory;
        private TankSpecification _specification;

        private TankAnimator _animator;
        private TankEffector _effector;

        private Scale _fuel;
        private Scale _health;

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

        public IReadOnlyScale Health => _health;

        public override void CompositeUpdate()
        {
            int movementDirection = _controller.GetMovementDirection();

            if (movementDirection.IsInRange(-1, 1) == false)
            {
                Debug.LogError("Direction must be in the range [-1;1]");
                return;
            }

            if (_fuel.IsEmpty)
                return;

            if (movementDirection != 0)
                _fuel.Value -= _fuelConsumptionRate * Time.deltaTime;

            _movement.Move(movementDirection, Time.deltaTime);
        }

        private void SetComponents()
        {
            _cannon = GetComponent<TankCannon>();
            _movement = GetComponent<TankMovement>();
            _inventory = GetComponent<TankInventory>();
            _specification = GetComponent<TankSpecification>();

            _animator = GetComponent<TankAnimator>();
            _effector = GetComponent<TankEffector>();
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

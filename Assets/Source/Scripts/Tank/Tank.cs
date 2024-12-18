using UnityEngine;
using System.Collections.Generic;
using TanksArmageddon.TankControl;

namespace TanksArmageddon.TankComponents
{
    [RequireComponent(typeof(TankCannon))]
    [RequireComponent(typeof(TankMovement))]
    [RequireComponent(typeof(TankInventory))]
    [RequireComponent(typeof(TankSpecification))]
    [RequireComponent(typeof(TankFuel))]
    [RequireComponent(typeof(TankHealth))]
    [RequireComponent(typeof(TankAnimator))]
    [RequireComponent(typeof(TankEffector))]
    public class Tank : MonoBehaviour
    {
        private TankCannon _cannon;
        private TankMovement _movement;
        private TankInventory _inventory;
        private TankSpecification _specification;

        private TankFuel _fuel;
        private TankHealth _health;

        private TankAnimator _animator;
        private TankEffector _effector;

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
        }

        private void Update()
        {
            int movementDirection = _controller.GetMovementDirection();

            if (movementDirection.IsInRange(-1, 1) == false)
            {
                Debug.LogError("Direction must be in the range [-1;1]");
                return;
            }

            _movement.Move(movementDirection, Time.deltaTime);
        }

        private void SetComponents()
        {
            _cannon = GetComponent<TankCannon>();
            _movement = GetComponent<TankMovement>();
            _inventory = GetComponent<TankInventory>();
            _specification = GetComponent<TankSpecification>();

            _fuel = GetComponent<TankFuel>();
            _health = GetComponent<TankHealth>();

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

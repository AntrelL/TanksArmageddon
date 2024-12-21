using System.Collections.Generic;
using TanksArmageddon.TankControl;
using UnityEngine;

namespace TanksArmageddon.TankComponents
{
    [RequireComponent(typeof(TankMovement))]
    public class Tank : MonoBehaviour
    {
        private TankMovement _movement;

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

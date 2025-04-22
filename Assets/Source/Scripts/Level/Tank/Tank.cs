using UnityEngine;

namespace TanksArmageddon
{
    public class Tank : Script
    {
        [SerializeField] private TankMover _tankMover;

        private ITankController _controller;

        public void Initialize(ITankController tankController)
        {
            _controller = tankController;

            _tankMover.Initialize();
            OnInitialized();
        }

        private void Update()
        {
            Move(_controller.GetMoveDirection(), Time.deltaTime);
        }

        private void Move(int direction, float deltaTime)
        {
            if (direction.IsInRange(-1, 1) == false)
            {
                Debug.LogError("The direction of movement of the tank must be equal to 1, -1 or 0");
                return;
            }

            _tankMover.Move(direction, deltaTime);
        }  
    }
}

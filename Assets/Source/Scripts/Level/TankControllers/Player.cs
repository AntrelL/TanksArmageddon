using UnityEngine;

namespace TanksArmageddon
{
    public class Player : ITankController
    {
        private const string Horizontal = nameof(Horizontal);

        private PlayerControls _controls;

        public Player(PlayerControls controls)
        {
            _controls = controls;
        }

        public int GetMoveDirection()
        {
            int direction = 0;

            if (_controls.LeftMoveButton.IsPressed)
                direction = -1;
            else if (_controls.RightMoveButton.IsPressed)
                direction = 1;
            else
                direction = (int)Input.GetAxisRaw(Horizontal);

            return direction;
        }
    }
}

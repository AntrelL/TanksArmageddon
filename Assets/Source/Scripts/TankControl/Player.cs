using TanksArmageddon.UI;
using UnityEngine;

namespace TanksArmageddon.TankControl
{
    public class Player : ITankController
    {
        private const string Horizontal = nameof(Horizontal);

        private ButtonPressListener _moveLeftButton;
        private ButtonPressListener _moveRightButton;

        public Player(ButtonPressListener moveLeftButton, ButtonPressListener moveRightButton)
        {
            _moveLeftButton = moveLeftButton;
            _moveRightButton = moveRightButton;
        }

        public int GetMovementDirection()
        {
            if (_moveLeftButton.IsPressed)
                return -1;

            if (_moveRightButton.IsPressed)
                return 1;

            return (int)Input.GetAxisRaw(Horizontal);
        }
    }
}

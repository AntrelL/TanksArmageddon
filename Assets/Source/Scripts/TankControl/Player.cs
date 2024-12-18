using UnityEngine;

namespace TanksArmageddon.TankControl
{
    public class Player : ITankController
    {
        private const string Horizontal = nameof(Horizontal);

        public int GetMovementDirection() => (int)Input.GetAxisRaw(Horizontal);
    }
}

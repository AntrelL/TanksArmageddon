using UnityEngine;

namespace TanksArmageddon
{
    public class Player : ITankController, IHealable
    {
        private const string Horizontal = nameof(Horizontal);

        private PlayerControls _controls;
        private int _maxHealth;
        private int _currentHealth;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;

        public Player(PlayerControls controls)
        {
            _controls = controls;
        }

        public void Heal(int amount)
        {
            _currentHealth += amount;

            if (_currentHealth <= _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
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

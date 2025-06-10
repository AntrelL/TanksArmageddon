using UnityEngine;

namespace Realese
{
    public class Tank : Script
    {
        private ITankController _controller;
        private Health _health;

        public Tank(ITankController controller, IReadOnlyScale<int> healthSettings)
        {
            _controller = controller;
            _health = new(healthSettings);

            Link(_controller.ShotActivated, Shoot);
        }

        public IProtectedHealth Health => _health;

        public override void Update(float deltaTime)
        {
        }

        private void Shoot()
        {
            // пиу
            Debug.Log("пиу");
        }
    }
}

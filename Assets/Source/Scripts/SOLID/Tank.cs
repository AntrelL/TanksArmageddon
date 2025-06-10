namespace Assets.Source.Scripts.SOLID
{
    public class Tank
    {
        private ITankController _controller;
        private Health _health;

        public Tank(ITankController controller, Health health)
        {
            _controller = controller;
            _health = health;

            _controller.ShotActivated += OnShotActivated;
        }

        private void Update()
        {
        }

        private void OnShotActivated()
        {
            //пиу
        }

        private void OnDestroy()
        {
            _controller.ShotActivated -= OnShotActivated;
        }
    }
}

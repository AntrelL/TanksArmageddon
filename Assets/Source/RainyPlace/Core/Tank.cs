namespace RainyPlace.Core
{
    public class Tank : Script
    {
        private ITankController _controller;

        public Tank(ITankController controller)
        {
            _controller = controller;
            Link(_controller.ShotActivated, Shoot);
        }

        public override void Update(float deltaTime)
        {

        }

        private void Shoot()
        {
            // пиу
        }
    }
}

using UnityEngine;

namespace TanksArmageddon
{
    public class Tank : MonoBehaviour
    {
        [SerializeField] private TankMovement _tankMovement;
        [SerializeField] private TankCannon _tankCannon;

        private ITankController _controller;
        
        public void Init(ITankController controller)
        {
            _controller = controller;
            
            _tankMovement.Init();
            _tankCannon.Init();
        }

        private void OnEnable()
        {
            _controller.ShotActivated += Shoot;
        }

        private void OnDisable()
        {
            _controller.ShotActivated -= Shoot;
        }

        private void Update()
        {
            _tankMovement.Move(_controller.MovementDirection, Time.deltaTime);
        }

        private void Shoot()
        {
            Debug.Log("пиу");
        }
    }
}

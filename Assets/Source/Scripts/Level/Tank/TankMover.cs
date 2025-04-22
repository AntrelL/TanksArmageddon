using UnityEngine;

namespace TanksArmageddon
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class TankMover : Script
    {
        [SerializeField][Min(0)] private float _speed;

        private Rigidbody2D _rigidbody;

        public void Initialize()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            OnInitialized();
        }

        public void Move(int direction, float deltaTime)
        {
            _rigidbody.AddForce(transform.right * _speed * direction * deltaTime);
        }
    }
}

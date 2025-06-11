using UnityEngine;

namespace TanksArmageddon
{
    public class TankMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _track;
        [SerializeField] private int _speed;
        
        public void Init()
        {
            
        }

        public void Move(int direction, float deltaTime)
        {
            _track.AddForce(Vector2.right * (direction * _speed * deltaTime));
        }
    }
}

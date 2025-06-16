using UnityEngine;

namespace TanksArmageddon
{
    public class Shell : MonoBehaviour, IShell
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _lifeTime;
        [SerializeField] private float _force;
        [SerializeField] private Rigidbody2D _rigidbody;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnHit();
        }
        
        public void Fire()
        {
            _rigidbody.AddForce(transform.right * _force, ForceMode2D.Force);
        }

        public void OnHit()
        {
            Debug.Log("Взрыв!");
            Destroy(gameObject);
        }
    }
}

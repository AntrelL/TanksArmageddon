using System.Collections;
using UnityEngine;

namespace TanksArmageddon
{
    public abstract class Shell : MonoBehaviour, IShell
    {
        [SerializeField] protected int _damage;
        [SerializeField] protected float _destroyDelay;
        [SerializeField] protected float _force;
        [SerializeField] protected Rigidbody2D _rigidbody;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(_damage);
                ApplyEffect(collision);
            }
            
            Destroy(gameObject);
        }
        
        public void Fire()
        {
            _rigidbody.AddForce(transform.right * _force, ForceMode2D.Impulse);
            StartCoroutine(DestroyAfterDelay(_destroyDelay));
        }

        public abstract void ApplyEffect(Collision2D collision);

        private IEnumerator DestroyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            Destroy(gameObject);
        }
    }
}

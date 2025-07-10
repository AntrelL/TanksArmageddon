using Source.Scripts.Release.HitProcessing;
using Source.Scripts.Release.Stuff;
using UnityEngine;

namespace Source.Scripts.Release.Enemy
{
    public class EnemyHealth : Health
    {
        [SerializeField] private int _edgeOfMapDamage = 5000;
        [SerializeField] private EnemyCombat _combat;

        [field: SerializeField] public int MaxHealth { get; private set; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out EdgeOfMap edge))
            {
                TakeDamage(_edgeOfMapDamage);
            }
        }

        protected override void OnPlayHitEffect() => TakeDamage(_combat.PlayerDamage);

        protected override int GetMaxHealth() => MaxHealth;
    }
}
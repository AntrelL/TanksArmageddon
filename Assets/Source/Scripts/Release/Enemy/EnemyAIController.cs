using System.Collections;
using Source.Scripts.Release.Projectiles;
using UnityEngine;

namespace Source.Scripts.Release.Enemy
{
    public class EnemyAIController : MonoBehaviour
    {
        [SerializeField] private EnemyMovement _movement;
        [SerializeField] private EnemyCombat _combat;

        private EnemyBullet _activeBullet;

        private void OnEnable()
        {
            if (_combat != null && _combat.ProjectileShooter != null)
            {
                _combat.ProjectileShooter.EnemyBulletSpawned += OnEnemyBulletSpawned;
            }
        }

        private void OnDisable()
        {
            if (_combat != null && _combat.ProjectileShooter != null)
            {
                _combat.ProjectileShooter.EnemyBulletSpawned -= OnEnemyBulletSpawned;
            }
        }

        public IEnumerator DoEnemyTurn()
        {
            _movement.StartMovement(-1f);
            float elapsed = 0f;
            float checkInterval = 0.1f;

            if (_combat.TryShoot())
            {
                _movement.StopMovement();
                yield return WaitProjectileFly();
                yield break;
            }

            while (elapsed < 3f)
            {
                yield return new WaitForSeconds(checkInterval);
                elapsed += checkInterval;

                if (_combat.TryShoot())
                {
                    _movement.StopMovement();
                    yield return WaitProjectileFly();
                    yield break;
                }
            }

            _movement.StopMovement();
        }

        private IEnumerator WaitProjectileFly()
        {
            if (_activeBullet == null)
                yield break;

            bool ended = false;
            void OnDestroyed() => ended = true;

            _activeBullet.Destroyed += OnDestroyed;
            yield return new WaitUntil(() => ended);
            _activeBullet.Destroyed -= OnDestroyed;
        }

        private void OnEnemyBulletSpawned(EnemyBullet bullet)
        {
            _activeBullet = bullet;
        }
    }
}
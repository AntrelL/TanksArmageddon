using System.Collections;
using Source.Scripts.Release.Projectiles;
using Source.Scripts.Release.Utils;
using UnityEngine;

namespace Source.Scripts.Release.Enemy
{
    public class EnemyAIController : MonoBehaviour
    {
        private const float CombatAttemptTimeLimit = 3f;
        private const float LeftDirection = -1f;

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
            _movement.StartMovement(LeftDirection);
            float elapsed = 0f;
            float checkInterval = 0.1f;

            while (elapsed < CombatAttemptTimeLimit)
            {
                if (_combat.TryShoot())
                {
                    _movement.StopMovement();

                    yield return new WaitDestroy(_activeBullet);
                    yield break;
                }

                yield return new WaitForSeconds(checkInterval);
                elapsed += checkInterval;
            }

            _movement.StopMovement();
        }

        private void OnEnemyBulletSpawned(EnemyBullet bullet)
        {
            _activeBullet = bullet;
        }
    }
}

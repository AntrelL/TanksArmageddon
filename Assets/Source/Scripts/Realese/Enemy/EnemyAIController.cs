using System.Collections;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    [SerializeField] private EnemyMovement _movement;
    [SerializeField] private EnemyCombat _combat;

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
        bool ended = false;
        void OnDestroyed() => ended = true;

        EnemyBullet.EnemyBulletDestroyed += OnDestroyed;
        yield return new WaitUntil(() => ended);
        EnemyBullet.EnemyBulletDestroyed -= OnDestroyed;
    }
}
using System.Collections;
using UnityEngine;

namespace Source.Scripts.Release.Enemy
{
    public class EnemyFacade : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _health;
        [SerializeField] private EnemyMovement _movement;
        [SerializeField] private EnemyAIController _ai;
        [SerializeField] private EnemyCombat _combat;

        public void TakeDamage(int value) => _health.TakeDamage(value);
    
        public void PlayHitEffect(Vector3 pos) => _health.PlayHitEffect(pos);

        public IEnumerator DoTurn() => _ai.DoEnemyTurn();

        public bool IsAlive => _health.IsAlive;
    }
}
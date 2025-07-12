using Source.Scripts.Release.HitProcessing;
using UnityEngine;

namespace Source.Scripts.Release.Enemy
{
    public class EnemyFacade : MonoBehaviour, IHealthImpactTarget
    {
        [SerializeField] private EnemyHealth _health;

        public Health Health => _health;
    }
}

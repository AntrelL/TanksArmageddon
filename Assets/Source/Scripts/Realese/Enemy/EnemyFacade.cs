using UnityEngine;

namespace Source.Scripts.Release.Enemy
{
    public class EnemyFacade : MonoBehaviour
    {
        [SerializeField] private EnemyHealth _health;

        public void PlayHitEffect(Vector3 pos) => _health.PlayHitEffect(pos);
    }
}
using Source.Scripts.Release.HitProcessing;
using UnityEngine;

namespace Source.Scripts.Release.Player
{
    public class PlayerRoot : MonoBehaviour, IHealthImpactTarget
    {
        [SerializeField] private PlayerHealth _health;
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private float _travelTime;
        
        public Health Health => _health;
        
        private void Awake()
        {
            _movement.Initialize(_travelTime);
        }
    }
}
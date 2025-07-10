using Source.Scripts.Release.Projectiles;
using UnityEngine;

namespace Source.Scripts.Release.Enemy
{
    public class EnemyCombat : MonoBehaviour
    {
        [SerializeField] private ProjectileShooter2D _projectileShooter;
        [SerializeField] private InventoryManager.InventoryManager _inventoryManager;

        public ProjectileShooter2D ProjectileShooter => _projectileShooter;
    
        private int _playerDamage = 100;

        private void OnEnable()
        {
            _inventoryManager.UpdatePlayerDamage += OnUpdatePlayerDamage;
        }

        private void OnDisable()
        {
            _inventoryManager.UpdatePlayerDamage -= OnUpdatePlayerDamage;
        }
    
        public bool TryShoot()
        {
            return _projectileShooter.ShootIfPossible();
        }

        public int GetPlayerDamage() => _playerDamage;

        private void OnUpdatePlayerDamage(int value)
        {
            _playerDamage = value;
        }
    }
}
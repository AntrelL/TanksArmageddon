using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private ProjectileShooter2D _projectileShooter;
    [SerializeField] private InventoryManager _inventoryManager;

    public ProjectileShooter2D ProjectileShooter => _projectileShooter;
    
    private int _playerDamage = 100;

    private void OnEnable()
    {
        _inventoryManager.UpdatePlayerDamage += UpdatePlayerDamage;
    }

    private void OnDisable()
    {
        _inventoryManager.UpdatePlayerDamage -= UpdatePlayerDamage;
    }
    
    public bool TryShoot()
    {
        return _projectileShooter.ShootIfPossible();
    }

    public int GetPlayerDamage() => _playerDamage;

    private void UpdatePlayerDamage(int value)
    {
        _playerDamage = value;
    }
}
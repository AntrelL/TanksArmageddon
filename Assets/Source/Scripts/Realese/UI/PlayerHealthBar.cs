using TanksArmageddon;
using UnityEngine;

public class PlayerHealthBar : HealthBar
{
    [SerializeField] private PlayerHealth _player;

    protected override void OnEnable()
    {
        if (_player != null)
        {
            _player.HealthChanged += UpdateValue;
        }
    }

    protected override void OnDisable()
    {
        if (_player != null)
        {
            _player.HealthChanged -= UpdateValue;
        }
    }

    protected override int GetMaxHealth()
    {
        return PlayerDataHandler.Instance.GetPlayerMaxHealth();
    }
}
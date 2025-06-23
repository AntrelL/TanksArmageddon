using TanksArmageddon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : HealthBar
{
    [SerializeField] private Player _player;

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
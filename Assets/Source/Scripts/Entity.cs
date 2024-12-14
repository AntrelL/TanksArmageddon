using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    public float _health;

    public Action<float> HealthChanged;

    private float Health 
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, _maxHealth);
            HealthChanged?.Invoke(_health);
        }
    }

    private void Awake()
    {
        Health = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0)
        {
            Debug.LogError("Damage cannot be less than zero");
            return;
        }

        Health -= damage;
    }

    public void Heal(float value)
    {
        if (value < 0)
        {
            Debug.LogError("Healing value cannot be less than zero");
            return;
        }

        Health += value;
    }
}

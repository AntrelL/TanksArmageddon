using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : HealthBar
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Vector3 _offset = new Vector3(0, 2, 0);

    protected override void OnEnable()
    {
        if (_enemy != null)
        {
            _enemy.HealthChanged += UpdateValue;
            _enemy.Defeated += DisableSlider;
        }
    }

    protected override void OnDisable()
    {
        if (_enemy != null)
        {
            _enemy.HealthChanged -= UpdateValue;
            _enemy.Defeated -= DisableSlider;
        }
    }

    protected override int GetMaxHealth()
    {
        return _enemy.MaxHealth;
    }

    private void FixedUpdate()
    {
        MoveSlider();
    }

    private void DisableSlider()
    {
        HealthSlider.gameObject.SetActive(false);
    }

    private void MoveSlider()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_enemy.transform.position + _offset);
        HealthSlider.transform.position = screenPosition;
    }
}
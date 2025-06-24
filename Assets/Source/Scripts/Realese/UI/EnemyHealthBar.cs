using UnityEngine;

public class EnemyHealthBar : HealthBar
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Vector3 _offset = new Vector3(0, 4, 0);

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
        gameObject.SetActive(false);
    }

    private void MoveSlider()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_enemy.transform.position + _offset);
        transform.position = screenPosition;
    }
}
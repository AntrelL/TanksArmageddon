using UnityEngine;

public class EnemyHealthBar : HealthBar
{
    [SerializeField] private EnemyHealth _enemyHealth;
    [SerializeField] private Vector3 _offset = new Vector3(0, 4, 0);

    protected override void OnEnable()
    {
        if (_enemyHealth != null)
        {
            _enemyHealth.HealthChanged += UpdateValue;
            _enemyHealth.Defeated += DisableSlider;
        }
    }

    protected override void OnDisable()
    {
        if (_enemyHealth != null)
        {
            _enemyHealth.HealthChanged -= UpdateValue;
            _enemyHealth.Defeated -= DisableSlider;
        }
    }

    protected override int GetMaxHealth()
    {
        return _enemyHealth.MaxHealth;
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
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(_enemyHealth.transform.position + _offset);
        transform.position = screenPosition;
    }
}
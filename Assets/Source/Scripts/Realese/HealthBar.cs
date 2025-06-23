using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class HealthBar : MonoBehaviour
{
    [SerializeField] protected Slider HealthSlider;
    [SerializeField] protected float SmoothSpeed = 5f;
    [SerializeField] protected TextMeshProUGUI ValueText;

    protected int MaxHealth;
    protected float TargetHealth;

    protected virtual void Awake()
    {
        MaxHealth = GetMaxHealth();
        TargetHealth = MaxHealth;
        ValueText.text = TargetHealth + "/" + MaxHealth;
        HealthSlider.maxValue = MaxHealth;
        HealthSlider.value = MaxHealth;
    }

    protected virtual void Update()
    {
        if (HealthSlider.value != TargetHealth)
        {
            ValueText.text = TargetHealth + "/" + MaxHealth;
            HealthSlider.value = Mathf.Lerp(HealthSlider.value, TargetHealth, Time.deltaTime * SmoothSpeed);
        }
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
    }

    protected abstract int GetMaxHealth();

    protected virtual void UpdateValue(int value)
    {
        TargetHealth = value;
    }
}
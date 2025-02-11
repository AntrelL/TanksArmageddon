using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurretController : MonoBehaviour
{
    [SerializeField] private Transform _turret;
    [SerializeField] private Slider _angleSlider;
    private float _initialGunAngle;

    private void Start()
    {
        _angleSlider.onValueChanged.AddListener(OnSliderValueChanged);
        _initialGunAngle = GetLocalGunAngle();
    }

    private void OnSliderValueChanged(float value)
    {
        float clampedAngle = Mathf.Clamp(value, _angleSlider.minValue, _angleSlider.maxValue);
        _turret.localRotation = Quaternion.Euler(0, 0, _initialGunAngle + clampedAngle);
    }

    private float GetLocalGunAngle()
    {
        return _turret.localEulerAngles.z;
    }

    private void OnEnable()
    {
        TurnManager.CanPlayerShoot += OnCanPlayerShoot;
    }

    private void OnDisable()
    {
        TurnManager.CanPlayerShoot -= OnCanPlayerShoot;
        _turret.DOKill();
    }

    private void OnCanPlayerShoot(bool canShoot)
    {
        _angleSlider.interactable = canShoot;
    }

    private void OnDestroy()
    {
        if (_angleSlider != null)
        {
            _angleSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}

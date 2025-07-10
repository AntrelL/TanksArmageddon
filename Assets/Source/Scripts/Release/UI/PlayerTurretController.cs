using DG.Tweening;
using Source.Scripts.Release.TurnManager;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.Release.UI
{
    public class PlayerTurretController : MonoBehaviour
    {
        [SerializeField] private Transform _turret;
        [SerializeField] private Slider _angleSlider;
        [SerializeField] private TurnState _turnManager;

        private float _initialGunAngle;

        private void Start()
        {
            _angleSlider.onValueChanged.AddListener(OnSliderValueChanged);
            _initialGunAngle = GetLocalGunAngle();
        }
    
        private void OnEnable()
        {
            _turnManager.CanPlayerShoot += OnCanPlayerShoot;
        }

        private void OnDisable()
        {
            _turnManager.CanPlayerShoot -= OnCanPlayerShoot;
            _turret.DOKill();
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
}
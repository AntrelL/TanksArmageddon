using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TankTurretController : MonoBehaviour
{
    [SerializeField] private Transform _turret;
    [SerializeField] private Slider _angleSlider;

    private void OnDisable()
    {
        _turret.DOKill();
    }

    void Start()
    {
        _angleSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        _turret.rotation = Quaternion.Euler(0, 0, value);
    }

    void OnDestroy()
    {
        if (_angleSlider != null)
        {
            _angleSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}

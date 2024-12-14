using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Tank))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private ButtonPressListener _moveLeftButton;
    [SerializeField] private ButtonPressListener _moveRightButton;
    [SerializeField] private Slider _cannonElevationRegulator;
    [SerializeField] private Button _shootButton;

    private Tank _tank;

    private void OnEnable()
    {
        _shootButton.onClick.AddListener(OnShootButtonClick);
        _cannonElevationRegulator.onValueChanged.AddListener(OnCannonElevationValueChanged);
    }

    private void OnDisable()
    {
        _shootButton.onClick.RemoveListener(OnShootButtonClick);
        _cannonElevationRegulator.onValueChanged.RemoveListener(OnCannonElevationValueChanged);
    }

    private void Start()
    {
        _tank = GetComponent<Tank>();
        _tank.ResetFuel();

        SetCannonElevationRegulatorAngles(_tank.GetCannonAngleSettings());
    }

    private void Update()
    {
        float horizontalInput = GetHorizontalInput();
        _tank.Move((int)horizontalInput);
    }

    private float GetHorizontalInput()
    {
        if (_moveLeftButton.IsPressed)
            return -1;

        if (_moveRightButton.IsPressed)
            return 1;

        return Input.GetAxisRaw("Horizontal");
    }

    private void SetCannonElevationRegulatorAngles((float minAngle, float maxAngle, float startAngle) settigns)
    {
        _cannonElevationRegulator.minValue = settigns.minAngle;
        _cannonElevationRegulator.maxValue = settigns.maxAngle;
        _cannonElevationRegulator.value = settigns.startAngle;
    }

    private void OnShootButtonClick() => _tank.Shoot();

    private void OnCannonElevationValueChanged(float value) => _tank.MoveCannon(value);
}

using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Tank : Entity
{
    [SerializeField] private TankArmament _armament;
    [SerializeField] private TankCannon _cannon;
    [SerializeField] private TankMovement _movement;
    [SerializeField] [Min(0)] private float _maxFuel;
    [SerializeField] [Min(0)] private float _fuelConsumption;

    private float _fuel = 0;

    public event Action<float> FuelChanged;

    private float Fuel
    {
        get => _fuel;
        set
        {
            _fuel = Mathf.Max(value, 0);
            FuelChanged?.Invoke(_fuel);
        }
    }

    private void Awake()
    {
        Construct();

        _movement.Construct(GetComponent<Rigidbody2D>());
        _armament.Construct(gameObject);
        _cannon.Construct();
    }

    public void MoveLeft() => Move(-1);

    public void MoveRight() => Move(1);

    public void Move(int direction) => Move(direction, Time.deltaTime);

    public void Move(int direction, float deltaTime)
    {
        if (Fuel == 0)
            direction = 0;
        
        if (direction != 0)
        {
            Fuel -= _fuelConsumption * deltaTime;
            
        }

        _movement.Move(direction, deltaTime);
    }

    public void MoveCannon(float angle)
    {
        _cannon.SetTargetAngle(angle);
    }

    public (float minAngle, float maxAngle, float startAngle) GetCannonAngleSettings()
    {
        return (_cannon.MinAngle, _cannon.MaxAngle, _cannon.StartAngle);
    }

    public void Shoot()
    {
        Shell shell = _armament.GetSelectedShell();
        _cannon.Shoot(shell);
    }

    public void ResetFuel() => Fuel = _maxFuel;
}

using Assets.Constructors.FuturisticTanks.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Tank _tank;
    [SerializeField] private float _force;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _availableTravelTime;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Slider _petrolTank;
    [SerializeField] private int _maxHealth = 1000;

    //public event Action<int> HealthChanged;

    private float _travelTimeSpent;
    private Vector2 _movementDirectionForDraw;

    private void Start()
    {
        _petrolTank.maxValue = _availableTravelTime;
        _petrolTank.value = _availableTravelTime;
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (_travelTimeSpent >= _availableTravelTime)
            return;

        if (horizontalInput != 0)
        {
            _travelTimeSpent += Time.fixedDeltaTime;
            _tank.Move();
        }
        else
        {
            _tank.Idle();
        }

        _petrolTank.value = _availableTravelTime - _travelTimeSpent;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1f, _layerMask);

        if (hit.collider != null)
        {
            Vector2 direction = Vector2.right * horizontalInput;
            direction = direction - (Vector2.Dot(direction, hit.normal) * hit.normal);

            _movementDirectionForDraw = direction;
            _rigidbody2D.AddForce(direction * _force);
        }

        if (_rigidbody2D.velocity.magnitude > _maxSpeed)
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _maxSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector3(_movementDirectionForDraw.x, _movementDirectionForDraw.y) * 5f);
    }
}

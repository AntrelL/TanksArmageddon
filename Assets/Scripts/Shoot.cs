using Assets.Constructors.FuturisticTanks.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Tank _tank;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _maxLineLength = 5f;
    [SerializeField] private Animator _animator;

    private float _initialRotationZ;

    private void Start()
    {
        _lineRenderer = _firePoint.GetComponent<LineRenderer>();
        _initialRotationZ = _firePoint.rotation.eulerAngles.z;
    }

    private void Update()
    {
        AimTowardsMouse();
        DrawAimLine();

        if (Input.GetButtonDown("Fire1"))
        {
            StartShoot();
        }
    }

    private void StartShoot()
    {
        _tank.Shot();
        Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);

        ParticleSystem flash = Instantiate(_muzzleFlash, _firePoint.position, _firePoint.rotation);
        flash.Play();

        Destroy(flash.gameObject, flash.main.duration);
    }

    private void AimTowardsMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = _mainCamera.ScreenToWorldPoint(mousePosition);

        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float clampedAngle = Mathf.Clamp(angle, _initialRotationZ - 20, _initialRotationZ + 20);

        _firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, clampedAngle));
    }

    void DrawAimLine()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = _mainCamera.ScreenToWorldPoint(mousePosition);

        Vector2 direction = (mousePosition - _firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float clampedAngle = Mathf.Clamp(angle, _initialRotationZ - 20, _initialRotationZ + 20);

        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, clampedAngle));
        Vector2 clampedDirection = rotation * Vector2.right;
        Vector2 endPoint = _firePoint.position + (Vector3)clampedDirection * _maxLineLength;

        _lineRenderer.SetPosition(0, _firePoint.position);
        _lineRenderer.SetPosition(1, endPoint);
    }
}

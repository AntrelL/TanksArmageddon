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
    [SerializeField] private Animator _animator;
    [SerializeField] private UIController _UIcontroller;

    private void OnEnable()
    {
        _UIcontroller.ButtonPressed += StartShoot;
    }

    private void OnDisable()
    {
        _UIcontroller.ButtonPressed -= StartShoot;
    }

    private void StartShoot()
    {
        _tank.Shot();
        Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);

        ParticleSystem flash = Instantiate(_muzzleFlash, _firePoint.position, _firePoint.rotation);
        flash.Play();

        Destroy(flash.gameObject, flash.main.duration);
    }
}

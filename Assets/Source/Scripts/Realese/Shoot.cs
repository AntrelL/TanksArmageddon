using System;
using Assets.Constructors.FuturisticTanks.Scripts;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Tank _tank;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private UIController _UIcontroller;
    
    private AudioManager _manager;

    private void Awake()
    {
        _manager = FindObjectOfType<AudioManager>();
    }

    private void OnEnable()
    {
        _UIcontroller.PlayerShootButtonPressed += StartShoot;
    }

    private void OnDisable()
    {
        _UIcontroller.PlayerShootButtonPressed -= StartShoot;
    }

    private void StartShoot()
    {
        _manager.PlayProjectileShoot();
        _tank.Shot();
        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        
        ProjectileTracker.Instance?.RegisterProjectile(bullet.transform);

        ParticleSystem flash = Instantiate(_muzzleFlash, _firePoint.position, _firePoint.rotation);
        flash.Play();

        Destroy(flash.gameObject, flash.main.duration);
    }
}
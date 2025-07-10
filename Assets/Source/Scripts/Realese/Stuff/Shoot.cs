using Assets.Constructors.FuturisticTanks.Scripts;
using Source.Scripts.Release.Projectiles;
using Source.Scripts.Release.UI;
using UnityEngine;

namespace Source.Scripts.Release.Stuff
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Tank _tank;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private ParticleSystem _muzzleFlash;
        [SerializeField] private UIController _uiController;
    
        private AudioManager _manager;

        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
        }

        private void OnEnable()
        {
            _uiController.PlayerShootButtonPressed += StartShoot;
        }

        private void OnDisable()
        {
            _uiController.PlayerShootButtonPressed -= StartShoot;
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
}
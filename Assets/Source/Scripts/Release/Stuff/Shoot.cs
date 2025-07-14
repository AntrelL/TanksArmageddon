using Assets.Constructors.FuturisticTanks.Scripts;
using Source.Scripts.Release.LandCutter;
using Source.Scripts.Release.Projectiles;
using Source.Scripts.Release.UI.ControllerParts;
using UnityEngine;

namespace Source.Scripts.Release.Stuff
{
    public class Shoot : MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Tank _tank;
        [SerializeField] private Transform _firePoint;
        [SerializeField] private ParticleSystem _muzzleFlash;
        [SerializeField] private PlayerInteractionUI _playerInteractionUI;
        [SerializeField] private LandCutterFacade _landCutter;

        private AudioManager _manager;

        private void Awake()
        {
            _manager = AudioManager.Instance;
        }

        private void OnEnable()
        {
            _playerInteractionUI.PlayerShootButtonPressed += StartShoot;
        }

        private void OnDisable()
        {
            _playerInteractionUI.PlayerShootButtonPressed -= StartShoot;
        }

        private void StartShoot()
        {
            _manager.PlayProjectileShoot();
            _tank.Shot();
            GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
            bullet.GetComponent<IBullet>().SetLandCutter(_landCutter);

            ProjectileTracker.Instance?.RegisterProjectile(bullet.transform);

            ParticleSystem flash = Instantiate(_muzzleFlash, _firePoint.position, _firePoint.rotation);
            flash.Play();

            Destroy(flash.gameObject, flash.main.duration);
        }
    }
}
using Assets.Constructors.FuturisticTanks.Scripts;
using DG.Tweening;
using UnityEngine;

public class EnemyTurretController : MonoBehaviour
{
    [SerializeField] private Transform _turret;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private float _aimDuration = 1f;
    [SerializeField] private Tank _tank;

    public void AimAndShootAt(Transform target)
    {
        Vector2 direction = (target.position - _turret.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f;

        _turret.DORotate(new Vector3(0, 0, targetAngle), _aimDuration)
               .OnComplete(() => Shoot());
    }

    private void Shoot()
    {
        _tank.Shot();

        GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation * Quaternion.Euler(0, 0, -180));
        DefaultProjectile projectile = bullet.GetComponent<DefaultProjectile>();

        if (projectile != null)
        {
            projectile.IsEnemyProjectile = true;
        }

        ParticleSystem flash = Instantiate(_muzzleFlash, _firePoint.position, _firePoint.rotation);
        flash.Play();
        Destroy(flash.gameObject, flash.main.duration);
    }
}

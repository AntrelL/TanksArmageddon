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
    [SerializeField] private float _minAngle = -30f;
    [SerializeField] private float _maxAngle = 30f;
    [SerializeField] private DefaultProjectile _projectilePrefab;

    private float _projectileSpeed;
    private TurnManager _turnManager;

    private void Start()
    {
        var projectile = _bulletPrefab.GetComponent<DefaultProjectile>();
        if (projectile != null) _projectileSpeed = _projectilePrefab.Speed;

        _turnManager = FindObjectOfType<TurnManager>();
    }

    private float GetRandomTargetX(float playerX, float difficultyFactor)
    {
        var offset = playerX * difficultyFactor;
        return Random.Range(playerX - offset, playerX + offset);
    }

    private float CalculateBallisticAngle(Vector2 start, Vector2 target, float speed)
    {
        var g = Mathf.Abs(Physics2D.gravity.y);
        var d = Mathf.Abs(target.x - start.x);
        var y = target.y - start.y;

        var v2 = speed * speed;
        var disc = v2 * v2 - g * (g * d * d + 2 * y * v2);
        if (disc < 0) return 0f;

        var sqrtDisc = Mathf.Sqrt(disc);
        var angle = Mathf.Atan((v2 - sqrtDisc) / (g * d));
        return angle * Mathf.Rad2Deg;
    }

    public bool CanShoot(Transform target)
    {
        var difficultyFactor = _turnManager.DifficultyFactor;
        Vector2 firePosition = _firePoint.position;

        var targetX = GetRandomTargetX(target.position.x, difficultyFactor);
        var targetPos = new Vector2(targetX, target.position.y);

        var angle = CalculateBallisticAngle(firePosition, targetPos, _projectileSpeed);
        return angle >= _minAngle && angle <= _maxAngle;
    }

    public void Shoot(Transform target)
    {
        var difficultyFactor = _turnManager.DifficultyFactor;
        Vector2 firePosition = _firePoint.position;

        var targetX = GetRandomTargetX(target.position.x, difficultyFactor);
        var targetPos = new Vector2(targetX, target.position.y);

        var angle = CalculateBallisticAngle(firePosition, targetPos, _projectileSpeed);
        angle = Mathf.Clamp(angle, _minAngle, _maxAngle);

        _turret.DORotate(new Vector3(0, 0, angle), _aimDuration)
            .OnComplete(() => Fire(target, targetX, target.position.y));
    }

    private void Fire(Transform target, float targetX, float targetY)
    {
        _tank.Shot();
        var bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);

        var projectile = bullet.GetComponent<DefaultProjectile>();
        if (projectile != null)
        {
            projectile.IsEnemyProjectile = true;
            projectile.SetupBallisticTrajectory(targetX, targetY);
        }

        var flash = Instantiate(_muzzleFlash, _firePoint.position, _firePoint.rotation);
        flash.Play();
        Destroy(flash.gameObject, flash.main.duration);
    }
}
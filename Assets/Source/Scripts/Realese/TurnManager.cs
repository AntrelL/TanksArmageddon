using System;
using System.Collections;
using System.Collections.Generic;
using TanksArmageddon;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    [SerializeField] private Player _player;
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private UIController _uiController;
    [SerializeField] private ProjectileShooter2D _enemyTurett;

    [Header("Параметры ходов")]
    [SerializeField] private float _projectileTransitionDuration = 1f;
    [SerializeField] private float _enemyMaxMovementTime = 5f;
    [SerializeField] public float _difficultyFactor = 0.1f;

    private int _turnCount = 0;
    private bool _isPlayerTurn = true;
    private bool _allEnemiesDead = false;
    private Transform _nextTarget;

    public static event Action AllEnemiesDead;
    public static event Action<bool> CanPlayerControl;
    public static event Action<bool> CanPlayerShoot;
    public static event Action<Transform> TurnStarted;

    public static bool CurrentTurnIsPlayer { get; private set; }

    private void Start()
    {
        _uiController.ButtonPressed += OnPlayerShoot;

        if (_cameraController.IntroFinished)
        {
            StartCoroutine(TurnCycle());
        }
        else
        {
            _cameraController.UnlockMovement += OnCameraIntroFinished;
        }
    }

    private IEnumerator TurnCycle()
    {
        while (!_allEnemiesDead)
        {
            if (_player != null)
            {
                yield return StartCoroutine(PlayerTurn());

                if (CheckAllEnemiesDead())
                    break;
            }
        }
    }

    private void OnCameraIntroFinished(bool unlocked)
    {
        if (unlocked)
        {
            _cameraController.UnlockMovement -= OnCameraIntroFinished;
            StartCoroutine(TurnCycle());
        }
    }

    private IEnumerator PlayerTurn()
    {
        _isPlayerTurn = true;
        _turnCount++;
        CurrentTurnIsPlayer = true;

        Debug.Log($"[Ход {_turnCount}] Ход игрока начался");

        TurnStarted?.Invoke(_player.transform);
        UnblockPlayerControls(true);

        bool shotFired = false;
        Action onShot = () => { shotFired = true; };
        _uiController.ButtonPressed += onShot;

        yield return new WaitUntil(() => shotFired);
        UnblockPlayerControls(false);

        _uiController.ButtonPressed -= onShot;
        bool projectileEnded = false;
        Action onProjectileDestroyed = () => { projectileEnded = true; };

        DefaultProjectile.ProjectileDestroyed += onProjectileDestroyed;

        yield return new WaitUntil(() => projectileEnded);

        DefaultProjectile.ProjectileDestroyed -= onProjectileDestroyed;

        Transform nextTarget = GetNextTargetForCamera();

        if (nextTarget != null)
        {
            yield return StartCoroutine(_cameraController.TransitionToTarget(nextTarget, _projectileTransitionDuration));
        }

        Debug.Log($"[Ход {_turnCount}] Ход игрока завершён");
        _isPlayerTurn = false;
    }

    private Transform GetNextTargetForCamera()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
            {
                return enemy.transform;
            }
        }

        return _player.transform;
    }

    private bool CheckAllEnemiesDead()
    {
        bool anyAlive = false;

        foreach (Enemy enemy in _enemies)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
            {
                anyAlive = true;
                break;
            }
        }

        if (!anyAlive)
        {
            OnAllEnemiesDead();
            return true;
        }

        return false;
    }

    private void OnAllEnemiesDead()
    {
        if (_allEnemiesDead)
            return;

        _allEnemiesDead = true;
        AllEnemiesDead?.Invoke();
        Debug.Log($"Все враги мертвы. Общее число ходов: {_turnCount}");
    }

    private void OnPlayerShoot()
    {
        if (!_isPlayerTurn)
        {
            Debug.LogWarning("Игрок попытался выстрелить не в свой ход!");
        }
    }

    private void UnblockPlayerControls(bool canControl)
    {
        CanPlayerControl?.Invoke(canControl);
        CanPlayerShoot?.Invoke(canControl);
    }
}

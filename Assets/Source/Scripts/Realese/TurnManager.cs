using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TanksArmageddon;

public class TurnManager : MonoBehaviour
{
    [Header("Ссылки на объекты")]
    [SerializeField] private Player _player;
    [SerializeField] private List<Enemy> _enemies;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private UIController _uiController;

    [Header("Параметры ходов")]
    [SerializeField] private float _projectileTransitionDuration = 1f;
    [SerializeField] private float _difficultyFactor = 0.1f;

    private int _turnCount = 0;
    public int TurnCount => _turnCount;
    public float DifficultyFactor => _difficultyFactor;
    private bool _allEnemiesDead = false;

    public static event Action AllEnemiesDead;
    public static event Action<bool> CanPlayerControl;
    public static event Action<bool> CanPlayerShoot;
    public static event Action<Transform> TurnStarted;
    public static event Action PlayerTurnFinished;
    public static event Action<int> CompletedTurns;

    public static bool CurrentTurnIsPlayer { get; private set; }

    private void Start()
    {
        if (_cameraController.IntroFinished)
        {
            StartCoroutine(TurnCycle());
        }
        else
        {
            _cameraController.UnlockMovement += OnCameraIntroFinished;
        }
    }

    private void OnEnable()
    {
        TutorialManager.TutorialEnded += UnblockPlayerControls;
    }

    private void OnDisable()
    {
        TutorialManager.TutorialEnded += UnblockPlayerControls;
    }

    private IEnumerator TurnCycle()
    {
        while (!_allEnemiesDead)
        {
            if (_player != null && _player.gameObject.activeSelf)
            {
                yield return StartCoroutine(PlayerTurn());

                if (CheckAllEnemiesDead())
                    break;
            }

            for (int i = 0; i < _enemies.Count; i++)
            {
                Enemy enemy = _enemies[i];

                if (enemy != null && enemy.gameObject.activeSelf)
                {
                    yield return StartCoroutine(EnemyTurn(enemy));

                    if (CheckAllEnemiesDead())
                        break;
                }
            }
        }
    }

    private IEnumerator PlayerTurn()
    {
        _turnCount++;
        CurrentTurnIsPlayer = true;
        Debug.Log($"[Ход {_turnCount}] Ход игрока начался");

        TurnStarted?.Invoke(_player.transform);
        UnblockPlayerControls(true);

        bool shotFired = false;
        bool skipTurn = false;

        Action onShot = () => { shotFired = true; };
        Action onSkipTurn = () => { skipTurn = true; };
        _uiController.PlayerShootButtonPressed += onShot;
        UIController.SkipTurnButtonPressed += onSkipTurn;

        yield return new WaitUntil(() => shotFired || skipTurn);

        UnblockPlayerControls(false);
        _uiController.PlayerShootButtonPressed -= onShot;
        UIController.SkipTurnButtonPressed += onSkipTurn;

        if (skipTurn == false)
        {
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
        }

        if (skipTurn == true)
        {
            Transform nextTarget = GetNextTargetForCamera();

            if (nextTarget != null)
            {
                yield return StartCoroutine(_cameraController.TransitionToTarget(nextTarget, _projectileTransitionDuration));
            }
        }

        Debug.Log($"[Ход {_turnCount}] Ход игрока завершён");
        PlayerTurnFinished?.Invoke();
        CompletedTurns?.Invoke(_turnCount);
        CurrentTurnIsPlayer = false;
    }

    private IEnumerator EnemyTurn(Enemy enemy)
    {
        _turnCount++;
        Debug.Log($"[Ход {_turnCount}] Ход врага {enemy.name}");

        TurnStarted?.Invoke(enemy.transform);

        yield return StartCoroutine(enemy.DoEnemyTurn());

        if (enemy == GetLastActiveEnemy())
        {
            yield return StartCoroutine(_cameraController.TransitionToTarget(_player.transform, _projectileTransitionDuration));
        }

        CompletedTurns?.Invoke(_turnCount);
        Debug.Log($"[Ход {_turnCount}] Ход врага {enemy.name} завершён");
    }

    private Enemy GetLastActiveEnemy()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            if (_enemies[i] != null && _enemies[i].gameObject.activeSelf)
            {
                return _enemies[i];
            }
        }
        return null;
    }

    private bool CheckAllEnemiesDead()
    {
        foreach (Enemy enemy in _enemies)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
                return false;
        }

        OnAllEnemiesDead();
        return true;
    }

    private Transform GetNextTargetForCamera()
    {
        foreach (Enemy enemy in _enemies)
        {
            if (enemy != null && enemy.gameObject.activeSelf)
            {
                return enemy.transform;
            }
        }

        return _player ? _player.transform : null;
    }

    private void OnCameraIntroFinished(bool unlocked)
    {
        if (unlocked)
        {
            _cameraController.UnlockMovement -= OnCameraIntroFinished;
            StartCoroutine(TurnCycle());
        }
    }

    private void OnAllEnemiesDead()
    {
        if (_allEnemiesDead)
            return;

        _allEnemiesDead = true;
        AllEnemiesDead?.Invoke();
        Debug.Log($"Все враги мертвы. Общее число ходов: {_turnCount}");
    }

    private void UnblockPlayerControls(bool canControl)
    {
        CanPlayerControl?.Invoke(canControl);
        CanPlayerShoot?.Invoke(canControl);
    }
}

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

    [Header("Параметры ходов")]
    [SerializeField] private float _enemyTurnDuration = 3f;

    private int _turnCount = 0;
    private bool _isPlayerTurn = true;
    private bool _allEnemiesDead = false;

    public static event Action AllEnemiesDead;
    public static event Action<bool> CanPlayerControl;
    public static event Action<bool> CanPlayerShoot;

    private void Start()
    {
        _uiController.ButtonPressed += OnPlayerShoot;

        //StartCoroutine(TurnCycle());

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

            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i] == null)
                    continue;

                Enemy currentEnemy = _enemies[i];

                if (currentEnemy.gameObject.activeSelf)
                {
                    yield return StartCoroutine(EnemyTurn(currentEnemy));

                    if (CheckAllEnemiesDead())
                        break;
                }
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

        Debug.Log($"[Ход {_turnCount}] Ход игрока начался");

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

        UnblockPlayerControls(false);

        Debug.Log($"[Ход {_turnCount}] Ход игрока завершён");
        _isPlayerTurn = false;
    }

    private IEnumerator EnemyTurn(Enemy enemy)
    {
        _turnCount++;
        Debug.Log($"[Ход {_turnCount}] Ход врага {enemy.name} начался");


        UnblockPlayerControls(false);

        float timer = 0f;

        while (timer < _enemyTurnDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log($"[Ход {_turnCount}] Ход врага {enemy.name} завершён");
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

        //_uiController.ShowVictoryScreen();
    }

    private void OnPlayerShoot()
    {
        if (_isPlayerTurn == false)
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

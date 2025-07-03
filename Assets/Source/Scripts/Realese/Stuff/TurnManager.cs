using System;
using System.Collections;
using System.Collections.Generic;
using Source.Scripts.Realese.Enemy;
using Source.Scripts.Realese.Player;
using Source.Scripts.Realese.Projectiles;
using Source.Scripts.Realese.UI;
using UnityEngine;

namespace Source.Scripts.Realese.Stuff
{
    public class TurnManager : MonoBehaviour
    {
        [Header("Ссылки на объекты")]
        [SerializeField] private PlayerHealth _player;
        [SerializeField] private List<EnemyAIController> _enemies;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private UIController _uiController;
        [SerializeField] private TutorialManager _tutorialManager;

        [Header("Параметры ходов")]
        [SerializeField] private float _projectileTransitionDuration = 1f;
        [SerializeField] private float _difficultyFactor = 0.1f;

        private int _turnCount = 0;
        private bool _allEnemiesDead = false;

        public event Action AllEnemiesDead;

        public event Action<bool> CanPlayerControl;

        public event Action<bool> CanPlayerShoot;

        public event Action<Transform> TurnStarted;
    
        public event Action<int> CompletedTurns;

        public static bool CurrentTurnIsPlayer { get; private set; }

        public int TurnCount => _turnCount;

        public float DifficultyFactor => _difficultyFactor;

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
            _tutorialManager.TutorialEnded += UnblockPlayerControls;
        }

        private void OnDisable()
        {
            _tutorialManager.TutorialEnded += UnblockPlayerControls;
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
                    EnemyAIController enemy = _enemies[i];

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
                void OnProjectileDestroyed() => projectileEnded = true;

                ProjectileTracker.Instance.ProjectileDestroyed += OnProjectileDestroyed;

                yield return new WaitUntil(() => projectileEnded);

                ProjectileTracker.Instance.ProjectileDestroyed -= OnProjectileDestroyed;

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
        
            CompletedTurns?.Invoke(_turnCount);
            CurrentTurnIsPlayer = false;
        }

        private IEnumerator EnemyTurn(EnemyAIController enemy)
        {
            _turnCount++;

            TurnStarted?.Invoke(enemy.transform);

            yield return StartCoroutine(enemy.DoEnemyTurn());

            if (enemy == GetLastActiveEnemy())
            {
                yield return StartCoroutine(_cameraController.TransitionToTarget(_player.transform, _projectileTransitionDuration));
            }

            CompletedTurns?.Invoke(_turnCount);
        }

        private EnemyAIController GetLastActiveEnemy()
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
            foreach (EnemyAIController enemy in _enemies)
            {
                if (enemy != null && enemy.gameObject.activeSelf)
                    return false;
            }

            OnAllEnemiesDead();

            return true;
        }

        private Transform GetNextTargetForCamera()
        {
            foreach (EnemyAIController enemy in _enemies)
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
        }

        private void UnblockPlayerControls(bool canControl)
        {
            CanPlayerControl?.Invoke(canControl);
            CanPlayerShoot?.Invoke(canControl);
        }
    }
}

using System;
using System.Collections.Generic;
using Source.Scripts.Release.Enemy;
using Source.Scripts.Release.Player;
using Source.Scripts.Release.UI;
using UnityEngine;

namespace Source.Scripts.Release.TurnManager
{
    public class TurnState : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private PlayerHealth _player;
        [SerializeField] private List<EnemyAIController> _enemies;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private UIController _uiController;
        [SerializeField] private TutorialManager _tutorialManager;

        [Header("Turn Settings")]
        [SerializeField] private float _difficultyFactor = 0.1f;

        private int _turnCount;
        private bool _isAllEnemiesDead;
        private bool _isPlayerTurn;

        public event Action AllEnemiesDead;
        
        public event Action<bool> CanPlayerControl;
        
        public event Action<bool> CanPlayerShoot;
        
        public event Action<Transform> TurnStarted;
        
        public event Action<int> CompletedTurns;

        public int TurnCount => _turnCount;
        
        public bool IsAllEnemiesDead => _isAllEnemiesDead;
        
        public float DifficultyFactor => _difficultyFactor;
        
        public bool CurrentTurnIsPlayer => _isPlayerTurn;

        public PlayerHealth Player => _player;
        
        public List<EnemyAIController> Enemies => _enemies;
        
        public CameraController CameraController => _cameraController;
        
        public UIController UI => _uiController;

        public void IncrementTurn() => _turnCount++;
        
        public void NotifyTurnStarted(Transform t) => TurnStarted?.Invoke(t);
        
        public void NotifyTurnCompleted() => CompletedTurns?.Invoke(_turnCount);

        private void OnEnable()
        {
            _tutorialManager.TutorialEnded += SetPlayerControl;
        }

        private void OnDisable()
        {
            _tutorialManager.TutorialEnded -= SetPlayerControl;
        }
    
        public void SetPlayerControl(bool value)
        {
            CanPlayerControl?.Invoke(value);
            CanPlayerShoot?.Invoke(value);
        }
    
        public void SetPlayerTurn(bool isPlayerTurn)
        {
            _isPlayerTurn = isPlayerTurn;
        }

        public bool CheckAllEnemiesDead()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy != null && enemy.gameObject.activeSelf) return false;
            }

            if (!_isAllEnemiesDead)
            {
                _isAllEnemiesDead = true;
                AllEnemiesDead?.Invoke();
            }

            return true;
        }

        public Transform GetNextTarget()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy != null && enemy.gameObject.activeSelf)
                    return enemy.transform;
            }

            return _player ? _player.transform : null;
        }
    }
}

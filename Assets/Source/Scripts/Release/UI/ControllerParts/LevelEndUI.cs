using System;
using System.Collections.Generic;
using Source.Scripts.Release.Player;
using Source.Scripts.Release.Stuff;
using Source.Scripts.Release.TurnManager;
using TMPro;
using UnityEngine;
using YG;

namespace Source.Scripts.Release.UI.ControllerParts
{
    public class LevelEndUI : MonoBehaviour
    {
        private readonly List<(int UpperRangeLimit, int LevelReward, int PointsReward)> _goalTable =
            new List<(int UpperRangeLimit, int LevelReward, int PointsReward)>
            {
                (10, 2000, 100),
                (20, 1000, 50),
                (40, 500, 10)
            };

        [SerializeField] private GameObject _levelFinishedCanvas;
        [SerializeField] private GameObject _levelFailedCanvas;
        [SerializeField] private List<GameObject> _textGoals;
        [SerializeField] private TMP_Text _moneyRewardText;
        [SerializeField] private TMP_Text _pointsRewardText;
        [SerializeField] private PlayerHealth _playerHealth;
        [SerializeField] private TurnState _turnState;

        private int _turnCount;
        private int _levelRewardAmount;
        private int _pointsRewardAmount;
        private AudioManager _audioManager;

        public event Action<int> PlayerRewardReceived;

        public event Action<int> PlayerPointsReceived;

        private void OnEnable()
        {
            _turnState.AllEnemiesDead += ShowWinnerScreen;
            _playerHealth.Defeated += ShowDefeatedScreen;

            PlayerDataHandler.Instance.Link(this);
        }

        private void OnDisable()
        {
            _turnState.AllEnemiesDead -= ShowWinnerScreen;
            _playerHealth.Defeated -= ShowDefeatedScreen;

            PlayerDataHandler.Instance.UnLink(this);
        }

        public void Init(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public void ShowWinnerScreen()
        {
            YG2.saves.TrainingLevelPassed = true;
            Time.timeScale = 0f;
            _levelFinishedCanvas.SetActive(true);
            _audioManager.PlayLevelFinished();
            UpdateGoalStatus();
            YG2.SaveProgress();
        }

        public void ShowDefeatedScreen()
        {
            Time.timeScale = 0f;
            _levelFailedCanvas.SetActive(true);
            _audioManager.PlayLevelFailed();
        }

        private void UpdateGoalStatus()
        {
            _turnCount = _turnState.TurnCount;

            for (int i = 0; i < _goalTable.Count; i++)
            {
                var goal = _goalTable[i];

                if (_turnCount <= goal.UpperRangeLimit)
                {
                    _levelRewardAmount = goal.LevelReward;
                    _pointsRewardAmount = goal.PointsReward;

                    int numberOfGoalsCompleted = _textGoals.Count - i;

                    for (int j = 0; j < numberOfGoalsCompleted; j++)
                        _textGoals[j].SetActive(true);

                    break;
                }
            }

            _moneyRewardText.text = $"{_levelRewardAmount}";
            _pointsRewardText.text = $"{_pointsRewardAmount}";
            PlayerRewardReceived?.Invoke(_levelRewardAmount);
            PlayerPointsReceived?.Invoke(_pointsRewardAmount);
            YG2.SaveProgress();
        }
    }
}
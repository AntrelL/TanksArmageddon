using System;
using System.Collections;
using IJunior.TypedScenes;
using Source.Scripts.Realese.Airdrop;
using Source.Scripts.Realese.Player;
using Source.Scripts.Realese.Stuff;
using Source.Scripts.Realese.TurnManager;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace Source.Scripts.Realese.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _airdropNotifierCanvasGroup;
        [SerializeField] private CanvasGroup _pauseCanvasGroup;
        [SerializeField] private CanvasGroup _continueCanvasGroup;
        [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
        [SerializeField] private CanvasGroup _unmutedSoundCanvasGroup;
        [SerializeField] private CanvasGroup _mutedSoundCanvasGroup;
        [SerializeField] private CanvasGroup _inventoryCanvasGroup;
        [SerializeField] private GameObject _levelFinishedCanvas;
        [SerializeField] private GameObject _levelFailedCanvas;
        [SerializeField] private GameObject _playerMovementCanvas;
        [SerializeField] private Button _playerShootButton;
        [SerializeField] private Button _playerSkipTurnButton;
        [SerializeField] private PlayerHealth _player;
        [SerializeField] private GameObject _textGoal1;
        [SerializeField] private GameObject _textGoal2;
        [SerializeField] private GameObject _textGoal3;
        [SerializeField] private TurnState _turnState;
        [SerializeField] private TMP_Text _counterText;
        [SerializeField] private TMP_Text _moneyRewardText;
        [SerializeField] private TMP_Text _pointsRewardText;
        [SerializeField] private float _fadeDuration = 1.0f;
        [SerializeField] private float _visibleDuration = 1.0f;
        [SerializeField] private AirdropSpawner _airdropSpawner;

        private int _turnCount;
        private int _levelRewardAmount;
        private int _pointsRewardAmount;
        private string _currentScene;
        private AudioManager _manager; 

        public event Action PlayerShootButtonPressed;

        public static event Action<int> PlayerRewardReceived;

        public static event Action<int> PlayerPointsReceived;

        //public static event Action FinishedCanvasShown;

        //public static event Action FailedCanvasShown;

        public static event Action SkipTurnButtonPressed;
    
        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
        } 

        private void Start()
        {
            Time.timeScale = 1;
            _currentScene = SceneManager.GetActiveScene().name;

            if (YG2.envir.isMobile)
            {
                _playerMovementCanvas.SetActive(true);
            }

            if (YG2.envir.isDesktop)
            {
                _playerMovementCanvas.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _airdropSpawner.Spawned += OnSpawned;
            _turnState.AllEnemiesDead += ShowWinnerScreen;
            _turnState.CanPlayerShoot += IsShootButtonInteractable;
            _turnState.CanPlayerShoot += IsSkipTurnButtonInteractable;
            _turnState.CanPlayerShoot += IsInventoryInteractabe;
            _turnState.CompletedTurns += UpdateTurnCounterText;
            _player.Defeated += ShowDefeatedScreen;
        }

        private void OnDisable()
        {
            _airdropSpawner.Spawned -= OnSpawned;
            _turnState.AllEnemiesDead -= ShowWinnerScreen;
            _turnState.CanPlayerShoot -= IsShootButtonInteractable;
            _turnState.CanPlayerShoot -= IsSkipTurnButtonInteractable;
            _turnState.CanPlayerShoot -= IsInventoryInteractabe;
            _turnState.CompletedTurns -= UpdateTurnCounterText;
            _player.Defeated -= ShowDefeatedScreen;
        }

        public void Win()
        {
            ShowWinnerScreen();
        }

        public void ShootButtonPressed()
        {
            if (!_playerShootButton.interactable)
                return;

            _playerShootButton.interactable = false;
            _manager.PlayButtonClick();
            PlayerShootButtonPressed?.Invoke();
        }

        public void SkipTurnButton()
        {
            if (!_playerSkipTurnButton.interactable)
                return;

            _playerSkipTurnButton.interactable = false;
            _manager.PlayButtonClick();
            SkipTurnButtonPressed?.Invoke();
        }

        public void OpenMainMenu()
        {
            _manager.PlayButtonClick();
            Time.timeScale = 0f;

            _mainMenuCanvasGroup.alpha = 1;
            _mainMenuCanvasGroup.interactable = true;
            _mainMenuCanvasGroup.blocksRaycasts = true;

            _pauseCanvasGroup.alpha = 0;
            _pauseCanvasGroup.interactable = false;
            _pauseCanvasGroup.blocksRaycasts = false;

            _continueCanvasGroup.alpha = 1;
            _continueCanvasGroup.interactable = true;
            _continueCanvasGroup.blocksRaycasts = true;
        }

        public void CloseMainMenu()
        {
            _manager.PlayButtonClick();
            Time.timeScale = 1f;

            _pauseCanvasGroup.alpha = 1;
            _pauseCanvasGroup.interactable = true;
            _pauseCanvasGroup.blocksRaycasts = true;

            _continueCanvasGroup.alpha = 0;
            _continueCanvasGroup.interactable = false;
            _continueCanvasGroup.blocksRaycasts = false;

            _mainMenuCanvasGroup.alpha = 0;
            _mainMenuCanvasGroup.interactable = false;
            _mainMenuCanvasGroup.blocksRaycasts = false;
        }

        public void Restart()
        {
            _manager.PlayButtonClick();
            Time.timeScale = 0f;
            SceneManager.LoadScene(_currentScene);
        }

        public void MuteSound()
        {
            _manager.StopMainMusic();

            _mutedSoundCanvasGroup.alpha = 1;
            _mutedSoundCanvasGroup.interactable = true;
            _mutedSoundCanvasGroup.blocksRaycasts = true;

            _unmutedSoundCanvasGroup.alpha = 0;
            _unmutedSoundCanvasGroup.interactable = false;
            _unmutedSoundCanvasGroup.blocksRaycasts = false;
        }

        public void UnmuteSound()
        {
            _manager.PlayMainMusic();
            _unmutedSoundCanvasGroup.alpha = 1;
            _unmutedSoundCanvasGroup.interactable = true;
            _unmutedSoundCanvasGroup.blocksRaycasts = true;

            _mutedSoundCanvasGroup.alpha = 0;
            _mutedSoundCanvasGroup.interactable = false;
            _mutedSoundCanvasGroup.blocksRaycasts = false;
        }

        public void OpenHomeScene()
        {
            _manager.PlayButtonClick();
            MainScene.Load();
        }

        public void OpenShopScene()
        {
            _manager.PlayButtonClick();
            ShopScene.Load();
        }

        public void OpenHangarScene()
        {
            _manager.PlayButtonClick();
            HangarScene.Load();
        }

        public void ShowVictoryScreen()
        {
            _manager.PlayButtonClick();
            _levelFinishedCanvas.SetActive(true);
        }

        private void ShowWinnerScreen()
        {
            YG2.saves.TrainingLevelPassed = true;
            Time.timeScale = 0f;
            _levelFinishedCanvas.SetActive(true);
            _manager.PlayLevelFinished();
            UpdateGoalStatus();
            YG2.SaveProgress();
        }

        private void ShowDefeatedScreen()
        {
            Time.timeScale = 0f;
            _levelFailedCanvas.SetActive(true);
            _manager.PlayLevelFailed();
        }

        private void IsShootButtonInteractable(bool isInteractable)
        {
            _playerShootButton.interactable = isInteractable;
        }

        private void IsSkipTurnButtonInteractable(bool isInteractable)
        {
            _playerSkipTurnButton.interactable = isInteractable;
        }

        private IEnumerator FadeRoutine()
        {
            yield return Fade(0f, 1f, _fadeDuration);

            yield return new WaitForSeconds(_visibleDuration);

            yield return Fade(1f, 0f, _fadeDuration);
        }

        private IEnumerator Fade(float startValue, float targetValue, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float alpha = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
                SetAlpha(alpha);
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            SetAlpha(targetValue);
        }

        private void SetAlpha(float alpha)
        {
            _airdropNotifierCanvasGroup.alpha = alpha;
        }

        private void IsInventoryInteractabe(bool value)
        {
            _inventoryCanvasGroup.interactable = value;
        }

        private void UpdateTurnCounterText(int turnCount)
        {
            _counterText.text = turnCount.ToString();
        }

        private void UpdateGoalStatus()
        {
            _turnCount = _turnState.TurnCount;

            if (_turnCount <= 10)
            {
                _textGoal1.SetActive(true);
                _textGoal2.SetActive(true);
                _textGoal3.SetActive(true);
                _levelRewardAmount = 2000;
                _pointsRewardAmount = 100;
            }

            if (_turnCount <= 20 && _turnCount > 10)
            {
                _textGoal1.SetActive(true);
                _textGoal2.SetActive(true);
                _levelRewardAmount = 1000;
                _pointsRewardAmount = 50;
            }

            if (_turnCount <= 40 && _turnCount > 20)
            {
                _textGoal1.SetActive(true);
                _levelRewardAmount = 500;
                _pointsRewardAmount = 10;
            }

            _moneyRewardText.text = $"{_levelRewardAmount}";
            _pointsRewardText.text = $"{_pointsRewardAmount}";
            PlayerRewardReceived?.Invoke(_levelRewardAmount);
            PlayerPointsReceived?.Invoke(_pointsRewardAmount);
            YG2.SaveProgress();
        }

        private void OnSpawned()
        {
            StartCoroutine(FadeRoutine());
        }
    }
}
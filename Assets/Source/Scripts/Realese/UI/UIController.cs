using IJunior.TypedScenes;
using TanksArmageddon;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Agava.WebUtility;
using System.Collections;

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
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _textGoal1;
    [SerializeField] private GameObject _textGoal2;
    [SerializeField] private GameObject _textGoal3;
    [SerializeField] private TurnManager _turnManager;
    [SerializeField] private TMP_Text _counterText;
    [SerializeField] private TMP_Text _moneyRewardText;
    [SerializeField] private TMP_Text _pointsRewardText;
    [SerializeField] private float _fadeDuration = 1.0f;
    [SerializeField] private float _visibleDuration = 1.0f;

    private int _turnCount;
    private int _levelRewardAmount;
    private int _pointsRewardAmount;

    public event Action PlayerShootButtonPressed;
    public static event Action EnemyDefeated;
    public static event Action<int> PlayerRewardReceived;
    public static event Action<int> PlayerPointsReceived;
    public static event Action SoundTurnedOff;
    public static event Action SoundTurnedOn;
    public static event Action ButtonClicked;
    public static event Action FinishedCanvasShown;
    public static event Action FailedCanvasShown;
    public static event Action SkipTurnButtonPressed;

    private void Start()
    {
        Time.timeScale = 1;

        if (Device.IsMobile == true)
        {
            Debug.Log("Platform: mobile!");
            _playerMovementCanvas.SetActive(true);
        }
        else
        {
            Debug.Log("Platform: PC!");
            _playerMovementCanvas.SetActive(false);
        }
    }

    private void OnEnable()
    {
        AirdropSpawner.Spawned += OnSpawned;
        TurnManager.AllEnemiesDead += ShowWinnerScreen;
        TurnManager.CanPlayerShoot += IsShootButtonInteractable;
        TurnManager.CanPlayerShoot += IsSkipTurnButtonInteractable;
        TurnManager.CanPlayerShoot += IsInventoryInteractabe;
        TurnManager.CompletedTurns += UpdateTurnCounterText;
        _player.Defeated += ShowDefeatedScreen;
    }

    private void OnDisable()
    {
        AirdropSpawner.Spawned -= OnSpawned;
        TurnManager.AllEnemiesDead -= ShowWinnerScreen;
        TurnManager.CanPlayerShoot -= IsShootButtonInteractable;
        TurnManager.CanPlayerShoot -= IsSkipTurnButtonInteractable;
        TurnManager.CanPlayerShoot -= IsInventoryInteractabe;
        TurnManager.CompletedTurns -= UpdateTurnCounterText;
        _player.Defeated -= ShowDefeatedScreen;
    }

    private void OnSpawned()
    {
        StartCoroutine(FadeRoutine());
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
        _turnCount = _turnManager.TurnCount;

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

        _moneyRewardText.text = $"НАГРАДА: {_levelRewardAmount}";
        _pointsRewardText.text = $"НАГРАДА: {_pointsRewardAmount}";
        PlayerRewardReceived?.Invoke(_levelRewardAmount);
        PlayerPointsReceived?.Invoke(_pointsRewardAmount);
    }

    private void ShowWinnerScreen()
    {
        EnemyDefeated?.Invoke();
        Time.timeScale = 0f;
        _levelFinishedCanvas.SetActive(true);
        FinishedCanvasShown?.Invoke();
        UpdateGoalStatus();
    }

    private void ShowDefeatedScreen()
    {
        Time.timeScale = 0f;
        _levelFailedCanvas.SetActive(true);
        FailedCanvasShown?.Invoke();
    }

    private void IsShootButtonInteractable(bool isInteractable)
    {
        _playerShootButton.interactable = isInteractable;
    }

    private void IsSkipTurnButtonInteractable(bool isInteractable)
    {
        _playerSkipTurnButton.interactable = isInteractable;
    }

    public void ShootButtonPressed()
    {
        if (!_playerShootButton.interactable)
            return;

        _playerShootButton.interactable = false;
        ButtonClicked?.Invoke();
        PlayerShootButtonPressed?.Invoke();
    }

    public void SkipTurnButton()
    {
        if (!_playerSkipTurnButton.interactable)
            return;

        _playerSkipTurnButton.interactable = false;
        ButtonClicked?.Invoke();
        SkipTurnButtonPressed?.Invoke();
    }

    public void OpenMainMenu()
    {
        ButtonClicked?.Invoke();
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
        ButtonClicked?.Invoke();
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
        ButtonClicked?.Invoke();
        Time.timeScale = 0f;
        TrainingScene.Load();
    }

    public void MuteSound()
    {
        //ButtonClicked?.Invoke();
        SoundTurnedOff?.Invoke();

        Debug.Log("Sound muted.");
        _mutedSoundCanvasGroup.alpha = 1;
        _mutedSoundCanvasGroup.interactable = true;
        _mutedSoundCanvasGroup.blocksRaycasts = true;

        _unmutedSoundCanvasGroup.alpha = 0;
        _unmutedSoundCanvasGroup.interactable = false;
        _unmutedSoundCanvasGroup.blocksRaycasts = false;
    }

    public void UnmuteSound()
    {
        //ButtonClicked?.Invoke();
        SoundTurnedOn?.Invoke();
        Debug.Log("Sound unmuted.");
        _unmutedSoundCanvasGroup.alpha = 1;
        _unmutedSoundCanvasGroup.interactable = true;
        _unmutedSoundCanvasGroup.blocksRaycasts = true;

        _mutedSoundCanvasGroup.alpha = 0;
        _mutedSoundCanvasGroup.interactable = false;
        _mutedSoundCanvasGroup.blocksRaycasts = false;
    }

    public void OpenHomeScene()
    {
        ButtonClicked?.Invoke();
        Debug.Log("Load Main Scene.");
        MainScene.Load();
    }

    public void OpenShopScene()
    {
        ButtonClicked?.Invoke();
        Debug.Log("Load Shop Scene.");
        ShopScene.Load();
    }

    public void OpenHangarScene()
    {
        ButtonClicked?.Invoke();
        Debug.Log("Load Hangar Scene.");
        HangarScene.Load();
    }

    public void ShowVictoryScreen()
    {
        ButtonClicked?.Invoke();
        _levelFinishedCanvas.SetActive(true);
    }
}

using IJunior.TypedScenes;
using TanksArmageddon;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _pauseCanvasGroup;
    [SerializeField] private CanvasGroup _continueCanvasGroup;
    [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup _unmutedSoundCanvasGroup;
    [SerializeField] private CanvasGroup _mutedSoundCanvasGroup;
    [SerializeField] private CanvasGroup _inventoryCanvasGroup;
    [SerializeField] private GameObject _levelFinishedCanvas;
    [SerializeField] private GameObject _levelFailedCanvas;
    [SerializeField] private Button _playerShootButton;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Player _player;
    [SerializeField] private GameObject _textGoal1;
    [SerializeField] private GameObject _textGoal2;
    [SerializeField] private GameObject _textGoal3;
    [SerializeField] private TurnManager _turnManager;

    private int _turnCount;

    public event Action PlayerShootButtonPressed;
    public static event Action EnemyDefeated;
    public static event Action SoundTurnedOff;
    public static event Action SoundTurnedOn;
    public static event Action ButtonClicked;
    public static event Action FinishedCanvasShown;
    public static event Action FailedCanvasShown;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        TurnManager.AllEnemiesDead += ShowWinnerScreen;
        TurnManager.CanPlayerShoot += IsShootButtonInteractable;
        TurnManager.CanPlayerShoot += IsInventoryInteractabe;
        _player.Defeated += ShowDefeatedScreen;
    }

    private void OnDisable()
    {
        TurnManager.AllEnemiesDead -= ShowWinnerScreen;
        TurnManager.CanPlayerShoot -= IsShootButtonInteractable;
        TurnManager.CanPlayerShoot -= IsInventoryInteractabe;
        _player.Defeated -= ShowDefeatedScreen;
    }

    private void IsInventoryInteractabe(bool value)
    {
        _inventoryCanvasGroup.interactable = value;
    }

    private void UpdateGoalStatus()
    {
        _turnCount = _turnManager.TurnCount;

        if (_turnCount <= 5)
        {
            _textGoal1.SetActive(true);
            _textGoal2.SetActive(true);
            _textGoal3.SetActive(true);
        }

        if (_turnCount <= 10 && _turnCount > 5)
        {
            _textGoal1.SetActive(true);
            _textGoal2.SetActive(true);
        }

        if (_turnCount <= 40 && _turnCount > 10)
        {
            _textGoal1.SetActive(true);
        }
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

    public void ShootButtonPressed()
    {
        if (!_playerShootButton.interactable)
            return;

        _playerShootButton.interactable = false;
        ButtonClicked?.Invoke();
        PlayerShootButtonPressed?.Invoke();
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
        ButtonClicked?.Invoke();
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
        ButtonClicked?.Invoke();
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

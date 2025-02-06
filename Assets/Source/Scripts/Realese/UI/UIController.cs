using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup _pauseCanvasGroup;
    [SerializeField] private CanvasGroup _continueCanvasGroup;
    [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup _unmutedSoundCanvasGroup;
    [SerializeField] private CanvasGroup _mutedSoundCanvasGroup;
    [SerializeField] private GameObject _levelFinishedCanvas;
    //[SerializeField] private Enemy _enemy;

    public event Action ButtonPressed;

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void OnEnable()
    {
        //_enemy.Defeated += ShowWinnerScreen;
    }

    private void OnDisable()
    {
        //_enemy.Defeated -= ShowWinnerScreen;
    }

    private void ShowWinnerScreen()
    {
        _levelFinishedCanvas.SetActive(true);
    }

    public void ShootButtonPressed()
    {
        ButtonPressed?.Invoke();
    }

    public void OpenMainMenu()
    {
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
        Time.timeScale = 0f;
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1f;
    }

    public void MuteSound()
    {
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
        Debug.Log("Load Home Scene.");
        //SceneManager.LoadScene("HomeScene");
    }

    public void OpenShopScene()
    {
        Debug.Log("Load Shop Scene.");
        //SceneManager.LoadScene("ShopScene");
    }

    public void OpenHangarScene()
    {
        Debug.Log("Load Hangar Scene.");
        //SceneManager.LoadScene("HangarScene");
    }
}

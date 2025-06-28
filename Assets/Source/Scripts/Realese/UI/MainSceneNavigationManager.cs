using System;
using IJunior.TypedScenes;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class MainSceneNavigationManager : MonoBehaviour
{
    [SerializeField] private GameObject _authView;
    [SerializeField] private GameObject _levelsBlockSprite;

    public static event Action ButtonClicked;

    private void Start()
    {
        Time.timeScale = 1f;
        _authView.SetActive(false);

        if (YG2.saves.TrainingLevelPassed)
        {
            _levelsBlockSprite.SetActive(false);
        }
        else
        {
            _levelsBlockSprite.SetActive(true);
        }

#if !UNITY_EDITOR && UNITY_WEBGL
        YG2.InterstitialAdvShow();
#endif
    }

    private void OnEnable()
    {
        YG2.onGetSDKData += TryOpenLeaderboard;
    }

    private void OnDisable()
    {
        YG2.onGetSDKData -= TryOpenLeaderboard;
    }

    public void AcceptPressed()
    {
        ButtonClicked?.Invoke();
        YG2.OpenAuthDialog();
    }

    public void DeclinePressed()
    {
        ButtonClicked?.Invoke();
        _authView.SetActive(false);
    }

    public void LeaderboardButtonPressed()
    {
        ButtonClicked?.Invoke();
        TryOpenLeaderboard();
    }

    private void TryOpenLeaderboard()
    {
        if (YG2.player.auth)
        {
            _authView.SetActive(false);
            LeaderboardScene.Load();
        }
        else
        {
            _authView.SetActive(true);
        }
    }
}
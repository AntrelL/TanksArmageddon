using System;
using System.Collections.Generic;
using IJunior.TypedScenes;
using UnityEngine;
using YG;

public class MainSceneNavigationManager : MonoBehaviour
{
    private readonly Dictionary<string, bool> _levelsStatus;

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

    public void LoadShopScene()
    {
        ButtonClicked?.Invoke();
        ShopScene.Load();
    }

    public void LoadHangarScene()
    {
        ButtonClicked?.Invoke();
        HangarScene.Load();
    }

    public void LeaderboardButtonPressed()
    {
        ButtonClicked?.Invoke();
        TryOpenLeaderboard();
    }

    public void LoadTrainingLevel()
    {
        ButtonClicked?.Invoke();
        TrainingScene.Load();
    }

    public void LoadLevel1()
    {
        ButtonClicked?.Invoke();
        Level1.Load();
    }

    public void LoadLevel2()
    {
        ButtonClicked?.Invoke();
        Level2.Load();
    }

    public void LoadLevel3()
    {
        ButtonClicked?.Invoke();
        Level3.Load();
    }

    public void LoadLevel4()
    {
        ButtonClicked?.Invoke();
        Level4.Load();
    }

    public void LoadLevel5()
    {
        ButtonClicked?.Invoke();
        Level5.Load();
    }

    public void LoadLevel6()
    {
        ButtonClicked?.Invoke();
        Level6.Load();
    }

    public void LoadLevel7()
    {
        ButtonClicked?.Invoke();
        Level7.Load();
    }

    public void LoadLevel8()
    {
        ButtonClicked?.Invoke();
        Level8.Load();
    }

    public void LoadLevel9()
    {
        ButtonClicked?.Invoke();
        Level9.Load();
    }

    public void LoadLevel10()
    {
        ButtonClicked?.Invoke();
        Level10.Load();
    }

    private void TryOpenLeaderboard()
    {
        if (YG2.player.auth == true)
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
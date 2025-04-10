using IJunior.TypedScenes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using YG;

public class MainSceneNavigationManager : MonoBehaviour
{
    [SerializeField] private GameObject _authView;
    [SerializeField] private AdService _adService;
    [SerializeField] private GameObject _levelsBlockSprite;


    private Dictionary<string, bool> _levelsStatus;

    public static event Action ButtonClicked;

    private void Start()
    {
        Time.timeScale = 1f;

        if (YG2.saves.trainingLevelPassed)
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
        Debug.Log("AcceptPressed");
        YG2.OpenAuthDialog();
    }

    public void DeclinePressed()
    {
        ButtonClicked?.Invoke();
        Debug.Log("DeclinePressed");
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
        Debug.Log("LeaderboardButtonPressed");
        TryOpenLeaderboard();
    }

    private void TryOpenLeaderboard()
    {
        if (YG2.player.auth == true)
        {
            Debug.Log("TryOpenLeaderboard, player.auth = true");
            _authView.SetActive(false);
            LeaderboardScene.Load();
        }
        else
        {
            Debug.Log("TryOpenLeaderboard, player.auth = false");
            _authView.SetActive(true);
        }
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
        //Level2.Load();
    }

    /*public void LoadLevel3() 
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
    }*/
}

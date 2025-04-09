using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdService
{
    public static event Action CollectVideoAdReward;
    public static event Action ShowInteractiveElements;

    //public void ShowVideoAd() => Agava.YandexGames.VideoAd.Show(OnOpenAdCallback, OnRewardedCallback, OnCloseAdCallback);
   // public void ShowInterstitialAd() => Agava.YandexGames.InterstitialAd.Show(OnOpenAdCallback, OnCloseInterstitialAdCallback);

    private void OnOpenAdCallback()
    {
        Time.timeScale = 0;
        AudioListener.volume = 0f;
    }

    private void OnCloseAdCallback()
    {
        Time.timeScale = 1;
        AudioListener.volume = 1f;
        ShowInteractiveElements?.Invoke();
    }

    private void OnRewardedCallback()
    {
        CollectVideoAdReward?.Invoke();
    }

    private void OnCloseInterstitialAdCallback(bool result)
    {
        Time.timeScale = 1;
        AudioListener.volume = 1f;
        ShowInteractiveElements?.Invoke();
    }
}

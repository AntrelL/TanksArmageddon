using IJunior.TypedScenes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneNavigationManager : MonoBehaviour
{
    public static event Action ButtonClicked;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void LoadShopScene()
    {
        ButtonClicked?.Invoke();
        Debug.Log("Load ShopScene");
        ShopScene.Load();
    }

    public void LoadHangarScene()
    {
        ButtonClicked?.Invoke();
        Debug.Log("Load HangarScene");
        HangarScene.Load();
    }

    public void LoadZeroLevel()
    {
        ButtonClicked?.Invoke();
        Debug.Log("Load ZeroLevel");
        TrainingScene.Load();
    }
}

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

    public void LoadTrainingLevel()
    {
        ButtonClicked?.Invoke();
        Debug.Log("Load TrainingLevel");
        TrainingScene.Load();
    }

    public void LoadLevel1()
    {
        ButtonClicked?.Invoke();
        Debug.Log("Load Level1");
        Level1.Load();
    }
}

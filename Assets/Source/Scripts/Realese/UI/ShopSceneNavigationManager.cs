using IJunior.TypedScenes;
using System;
using UnityEngine;
using YG;

public class ShopSceneNavigationManager : MonoBehaviour
{
    public static event Action ButtonClicked;

    public void LoadMainScene()
    {
        YG2.SaveProgress();
        ButtonClicked?.Invoke();
        Debug.Log("Load MainScene");
        MainScene.Load();
    }
}

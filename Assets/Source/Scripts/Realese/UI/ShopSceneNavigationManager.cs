using IJunior.TypedScenes;
using System;
using UnityEngine;

public class ShopSceneNavigationManager : MonoBehaviour
{
    public static event Action ButtonClicked;

    public void LoadMainScene()
    {
        ButtonClicked?.Invoke();
        Debug.Log("Load MainScene");
        MainScene.Load();
    }
}

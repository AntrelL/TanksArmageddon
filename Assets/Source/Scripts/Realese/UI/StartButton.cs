using System;
using IJunior.TypedScenes;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public static event Action MainSceneOpened;

    public void OpenMainScene()
    {
        MainSceneOpened?.Invoke();
        MainScene.Load();
    }
}
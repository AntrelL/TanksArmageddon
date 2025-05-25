using IJunior.TypedScenes;
using System;
using System.Collections;
using System.Collections.Generic;
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

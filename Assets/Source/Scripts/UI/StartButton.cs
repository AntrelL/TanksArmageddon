using IJunior.TypedScenes;
using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public static event Action MainSceneOpened;

    public void OpenMainScene()
    {
        MainSceneOpened?.Invoke();
        MainScene.Load();
    }
}

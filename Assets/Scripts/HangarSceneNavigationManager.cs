using IJunior.TypedScenes;
using System;
using UnityEngine;

public class HangarSceneNavigationManager : MonoBehaviour
{
    public static event Action ButtonClicked;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void LoadHomeScene()
    {
        ButtonClicked?.Invoke();
        Debug.Log("Load HomeScene");
        MainScene.Load();
    }
}

using IJunior.TypedScenes;
using System;
using UnityEngine;
using YG;

public class HangarSceneNavigationManager : MonoBehaviour
{
    public static event Action ButtonClicked;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void LoadHomeScene()
    {
        YG2.SaveProgress();
        ButtonClicked?.Invoke();
        Debug.Log("Load HomeScene");
        MainScene.Load();
    }
}

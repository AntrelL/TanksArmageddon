using System;
using IJunior.TypedScenes;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private AudioManager _manager;

    private void Awake()
    {
        _manager = FindObjectOfType<AudioManager>();
    }

    public void OpenMainScene()
    {
        _manager.PlayButtonClick();
        MainScene.Load();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using UnityEngine.UI;
using UnityEditor;
using System;
using IJunior.TypedScenes;

public class InitYG : MonoBehaviour
{
    
#if !UNITY_EDITOR && UNITY_WEBGL
    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize(OnInitialized);
    }

    private void OnInitialized()
    {
        Debug.Log("YG_SDK initialized");
        InitScene.Load();
    }
#endif
    
}

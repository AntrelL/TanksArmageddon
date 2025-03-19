using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using UnityEngine.UI;
using UnityEditor;
using System;

public class InitYG : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    public static event Action YGSDKinitialized;

#if !UNITY_EDITOR && UNITY_WEBGL
    private void Awake()
    {
        _startButton.interactable = false;
        YandexGamesSdk.CallbackLogging = true;
    }

    private IEnumerator Start()
    {
        yield return YandexGamesSdk.Initialize(OnInitialized);
    }

    private void OnInitialized()
    {
        _startButton.interactable = true;
        YGSDKinitialized.?Invoke();
        Debug.Log("YG_SDK initialized");
    }
#endif
}

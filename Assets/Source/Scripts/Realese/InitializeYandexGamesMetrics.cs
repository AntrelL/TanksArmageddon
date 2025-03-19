using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;

public class InitializeYandexGamesMetrics : MonoBehaviour
{
#if !UNITY_EDITOR && UNITY_WEBGL
    private void Start()
    {
        YandexGamesSdk.GameReady();
    }

#endif
}

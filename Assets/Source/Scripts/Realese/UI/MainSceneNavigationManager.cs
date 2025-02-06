using IJunior.TypedScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneNavigationManager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void LoadShopScene()
    {
        Debug.Log("Load ShopScene");
        ShopScene.Load();
    }

    public void LoadHangarScene()
    {
        Debug.Log("Load HangarScene");
        HangarScene.Load();
    }

    public void LoadZeroLevel()
    {
        Debug.Log("Load ZeroLevel");
        TrainingScene.Load();
    }
}

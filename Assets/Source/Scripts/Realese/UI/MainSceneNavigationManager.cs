using IJunior.TypedScenes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneNavigationManager : MonoBehaviour
{
    public static event Action ButtonClicked;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void LoadShopScene()
    {
        ButtonClicked?.Invoke();
        ShopScene.Load();
    }

    public void LoadHangarScene()
    {
        ButtonClicked?.Invoke();
        HangarScene.Load();
    }

    public void LoadTrainingLevel()
    {
        ButtonClicked?.Invoke();
        TrainingScene.Load();
    }

    /*public void LoadLevel1()
    {
        ButtonClicked?.Invoke();
        Level1.Load();
    }

    public void LoadLevel2()
    {
        ButtonClicked?.Invoke();
        Level2.Load();
    }

    public void LoadLevel3() 
    {
        ButtonClicked?.Invoke();
        Level3.Load();
    }

    public void LoadLevel4() 
    {
        ButtonClicked?.Invoke();
        Level4.Load();
    }

    public void LoadLevel5()
    {
        ButtonClicked?.Invoke();
        Level5.Load();
    }

    public void LoadLevel6()
    {
        ButtonClicked?.Invoke();
        Level6.Load();
    }

    public void LoadLevel7()
    {
        ButtonClicked?.Invoke();
        Level7.Load();
    }

    public void LoadLevel8()
    {
        ButtonClicked?.Invoke();
        Level8.Load();
    }

    public void LoadLevel9()
    {
        ButtonClicked?.Invoke();
        Level9.Load();
    }

    public void LoadLevel10()
    {
        ButtonClicked?.Invoke();
        Level10.Load();
    }*/
}

using IJunior.TypedScenes;
using System;
using UnityEngine;
using YG;

public class ShopSceneNavigationManager : MonoBehaviour
{
    [SerializeField] private Canvas _helpViewCanvas;

    public static event Action ButtonClicked;
    public static event Action TextShowing;

    public void HelpButtonPressed()
    {
        ButtonClicked?.Invoke();
        _helpViewCanvas.gameObject.SetActive(true);
        TextShowing?.Invoke();
    }

    public void OkayButtonPressed()
    {
        ButtonClicked?.Invoke();
        _helpViewCanvas.gameObject.SetActive(false);
    }

    public void LoadMainScene()
    {
        YG2.SaveProgress();
        ButtonClicked?.Invoke();
        Debug.Log("Load MainScene");
        MainScene.Load();
    }
}

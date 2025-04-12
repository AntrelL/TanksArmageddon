using IJunior.TypedScenes;
using System;
using UnityEngine;
using YG;

public class HangarSceneNavigationManager : MonoBehaviour
{
    [SerializeField] private Canvas _helpViewCanvas;

    public static event Action ButtonClicked;
    public static event Action TextShowing;

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
}

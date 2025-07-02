using System;
using IJunior.TypedScenes;
using UnityEngine;
using YG;

public class ShopSceneNavigationManager : MonoBehaviour
{
    [SerializeField] private Canvas _helpViewCanvas;
    
    private AudioManager _manager;

    public  event Action TextShowing;

    private void Awake()
    {
        _manager = FindObjectOfType<AudioManager>();
    }
    
    public void HelpButtonPressed()
    {
        _manager.PlayButtonClick();
        _helpViewCanvas.gameObject.SetActive(true);
        TextShowing?.Invoke();
    }

    public void OkayButtonPressed()
    {
        _manager.PlayButtonClick();
        _helpViewCanvas.gameObject.SetActive(false);
    }

    public void LoadMainScene()
    {
        YG2.SaveProgress();
        _manager.PlayButtonClick();
        MainScene.Load();
    }
}
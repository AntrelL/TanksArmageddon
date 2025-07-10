using System;
using IJunior.TypedScenes;
using UnityEngine;
using YG;

namespace Source.Scripts.Release.Stuff
{
    public class HangarSceneNavigationManager : MonoBehaviour
    {
        [SerializeField] private Canvas _helpViewCanvas;
    
        private AudioManager _manager;
    
        public event Action TextShowing;

        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
        }
    
        private void Start()
        {
            Time.timeScale = 1f;
        }

        public void LoadHomeScene()
        {
            YG2.SaveProgress();
            _manager.PlayButtonClick();
            MainScene.Load();
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
    }
}
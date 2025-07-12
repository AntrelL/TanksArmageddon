using System;
using IJunior.TypedScenes;
using Source.Scripts.Release.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Scripts.Release.UI.ControllerParts
{
    public class SceneNavigator : MonoBehaviour
    {
        private string _currentScene;
        private AudioManager _audioManager;

        public void Init(AudioManager audioManager)
        {
            _audioManager = audioManager;
            _currentScene = SceneManager.GetActiveScene().name;
        }

        public void Restart()
        {
            _audioManager.PlayButtonClick();
            Time.timeScale = 0f;
            SceneManager.LoadScene(_currentScene);
        }

        public void OpenHomeScene() =>
            LoadTypedSceneWithClick(MainScene.Load);

        public void OpenShopScene() =>
            LoadTypedSceneWithClick(ShopScene.Load);

        public void OpenHangarScene() =>
            LoadTypedSceneWithClick(HangarScene.Load);

        private void LoadTypedSceneWithClick(Action<LoadSceneMode> loadSceneMethod)
        {
            _audioManager.PlayButtonClick();
            loadSceneMethod?.Invoke(LoadSceneMode.Single);
        }
    }
}

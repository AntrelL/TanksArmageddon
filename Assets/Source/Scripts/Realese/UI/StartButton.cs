using IJunior.TypedScenes;
using Source.Scripts.Realese.Stuff;
using UnityEngine;

namespace Source.Scripts.Realese.UI
{
    public class StartButton : MonoBehaviour
    {
        private AudioManager _manager;

        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
        }

        public void OpenMainScene()
        {
            _manager.PlayButtonClick();
            MainScene.Load();
        }
    }
}
using NaughtyAttributes;
using Source.Scripts.Release.Stuff;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Scripts.Release.UI
{
    public class SceneLoader : MonoBehaviour
    {
        [Scene]
        [SerializeField] private string _sceneName;

        private AudioManager _manager;

        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
        }

        public void LoadScene()
        {
            _manager.PlayButtonClick();
            SceneManager.LoadScene(_sceneName);
        }
    }
}
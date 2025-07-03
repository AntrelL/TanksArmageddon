using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

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
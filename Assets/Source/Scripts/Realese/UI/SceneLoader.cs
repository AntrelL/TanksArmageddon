using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
    {
        [Scene]
        [SerializeField] private string _sceneName;
        
        public static event Action ButtonClicked;
        
        public void LoadScene()
        {
            ButtonClicked?.Invoke();
            SceneManager.LoadScene(_sceneName);
        }
    }
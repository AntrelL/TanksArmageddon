using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSlider : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletsContainer;
    [SerializeField] private int _bulletCount = 10;
    [SerializeField] private string _sceneToLoad = "Test"; // Название сцены для загрузки

    private void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(_sceneToLoad);
        gameLevel.allowSceneActivation = false;

        for (int i = 0; i < _bulletCount; i++)
        {
            Instantiate(_bulletPrefab, _bulletsContainer);
        }

        while (!gameLevel.isDone)
        {
            float progress = Mathf.Clamp01(gameLevel.progress / 0.9f);
            int filledBullets = Mathf.FloorToInt(progress * _bulletCount);

            for (int i = 0; i < _bulletsContainer.childCount; i++)
            {
                _bulletsContainer.GetChild(i).gameObject.SetActive(i < filledBullets);
            }

            if (gameLevel.progress >= 0.9f)
            {
                gameLevel.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}

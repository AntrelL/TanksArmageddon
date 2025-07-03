using IJunior.TypedScenes;
using Source.Scripts.Realese.Stuff;
using UnityEngine;
using YG;

namespace Source.Scripts.Realese.UI
{
    public class MainSceneNavigationManager : MonoBehaviour
    {
        [SerializeField] private GameObject _authView;
        [SerializeField] private GameObject _levelsBlockSprite;

        private AudioManager _manager;

        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
        }
    
        private void Start()
        {
            Time.timeScale = 1f;
            _authView.SetActive(false);

            if (YG2.saves.TrainingLevelPassed)
            {
                _levelsBlockSprite.SetActive(false);
            }
            else
            {
                _levelsBlockSprite.SetActive(true);
            }

#if !UNITY_EDITOR && UNITY_WEBGL
        YG2.InterstitialAdvShow();
#endif
        }

        private void OnEnable()
        {
            YG2.onGetSDKData += TryOpenLeaderboard;
        }

        private void OnDisable()
        {
            YG2.onGetSDKData -= TryOpenLeaderboard;
        }

        private void TryOpenLeaderboard()
        {
            if (YG2.player.auth)
            {
                _authView.SetActive(false);
                LeaderboardScene.Load();
            }
            else
            {
                _authView.SetActive(true);
            }
        }
    
        public void AcceptPressed()
        {
            _manager.PlayButtonClick();
            YG2.OpenAuthDialog();
        }

        public void DeclinePressed()
        {
            _manager.PlayButtonClick();
            _authView.SetActive(false);
        }

        public void LeaderboardButtonPressed()
        {
            _manager.PlayButtonClick();
            TryOpenLeaderboard();
        }
    }
}
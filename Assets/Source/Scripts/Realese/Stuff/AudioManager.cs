using UnityEngine;

namespace Source.Scripts.Realese.Stuff
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _backgroundMusicSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Audio Clips")]
        [SerializeField] private AudioClip _mainBackgroundMusic;
        [SerializeField] private AudioClip _levelFailedSound;
        [SerializeField] private AudioClip _levelFinishedSound;
        [SerializeField] private AudioClip _projectileShootedSound;
        [SerializeField] private AudioClip _tankHittedSound;
        [SerializeField] private AudioClip _buttonClickSound;

        private bool _isMusicOn = true;

        public static AudioManager Instance => _instance;

        public bool IsMusicOn => _isMusicOn;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            PlayMainMusic();
        }

        public void PlayMainMusic()
        {
            if (_backgroundMusicSource == null || _mainBackgroundMusic == null)
                return;
            _backgroundMusicSource.clip = _mainBackgroundMusic;
            _backgroundMusicSource.loop = true;
            _backgroundMusicSource.Play();
            _isMusicOn = true;
        }

        public void StopMainMusic()
        {
            if (_backgroundMusicSource == null)
                return;

            _backgroundMusicSource.Stop();
            _isMusicOn = false;
        }

        private void OnInBackgroundChange(bool inBackground)
        {
            AudioListener.pause = inBackground;
            AudioListener.volume = inBackground ? 0f : 1f;
        }

        public void PlayLevelFailed()
        {
            if (_isMusicOn)
            {
                if (_sfxSource == null || _levelFailedSound == null)
                    return;

                _sfxSource.volume = 0.5f;
                _sfxSource.PlayOneShot(_levelFailedSound);
            }
        }

        public void PlayLevelFinished()
        {
            if (_isMusicOn)
            {
                if (_sfxSource == null || _levelFinishedSound == null)
                    return;

                _sfxSource.volume = 0.5f;
                _sfxSource.PlayOneShot(_levelFinishedSound);
            }
        }

        public void PlayProjectileShoot()
        {
            if (_isMusicOn)
            {
                if (_sfxSource == null || _projectileShootedSound == null)
                    return;

                _sfxSource.volume = 1f;
                _sfxSource.PlayOneShot(_projectileShootedSound);
            }
        }

        public void PlayTankHit()
        {
            if (_isMusicOn)
            {
                if (_sfxSource == null || _tankHittedSound == null)
                    return;

                _sfxSource.volume = 1f;
                _sfxSource.PlayOneShot(_tankHittedSound);
            }
        }

        public void PlayButtonClick()
        {
            if (_isMusicOn)
            {
                if (_sfxSource == null || _buttonClickSound == null)
                    return;

                _sfxSource.volume = 1f;
                _sfxSource.PlayOneShot(_buttonClickSound);
            }
        }
    }
}
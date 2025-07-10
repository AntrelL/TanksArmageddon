using UnityEngine;

namespace Source.Scripts.Release.Stuff
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

        public bool IsMusicOn { get; private set; } = true;

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
            IsMusicOn = true;
        }

        public void StopMainMusic()
        {
            if (_backgroundMusicSource == null)
                return;

            _backgroundMusicSource.Stop();
            IsMusicOn = false;
        }

        public void PlayLevelFailed()
        {
            if (IsMusicOn)
            {
                if (_sfxSource == null || _levelFailedSound == null)
                    return;

                _sfxSource.volume = 0.5f;
                _sfxSource.PlayOneShot(_levelFailedSound);
            }
        }

        public void PlayLevelFinished()
        {
            if (IsMusicOn)
            {
                if (_sfxSource == null || _levelFinishedSound == null)
                    return;

                _sfxSource.volume = 0.5f;
                _sfxSource.PlayOneShot(_levelFinishedSound);
            }
        }

        public void PlayProjectileShoot()
        {
            if (IsMusicOn)
            {
                if (_sfxSource == null || _projectileShootedSound == null)
                    return;

                _sfxSource.volume = 1f;
                _sfxSource.PlayOneShot(_projectileShootedSound);
            }
        }

        public void PlayTankHit()
        {
            if (IsMusicOn)
            {
                if (_sfxSource == null || _tankHittedSound == null)
                    return;

                _sfxSource.volume = 1f;
                _sfxSource.PlayOneShot(_tankHittedSound);
            }
        }

        public void PlayButtonClick()
        {
            if (IsMusicOn)
            {
                if (_sfxSource == null || _buttonClickSound == null)
                    return;

                _sfxSource.volume = 1f;
                _sfxSource.PlayOneShot(_buttonClickSound);
            }
        }
    }
}
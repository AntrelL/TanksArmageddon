using Agava.WebUtility;
using System.Collections;
using System.Collections.Generic;
using TanksArmageddon;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

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

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
        Shoot.PlayerFired += PlayProjectileShoot;
        UIController.SoundTurnedOff += StopMainMusic;
        UIController.SoundTurnedOn += PlayMainMusic;
        StartButton.MainSceneOpened += PlayButtonClick;
        Shop.CardClicked += PlayButtonClick;
        MainSceneNavigationManager.ButtonClicked += PlayButtonClick;
        ShopSceneNavigationManager.ButtonClicked += PlayButtonClick;
        UIController.ButtonClicked += PlayButtonClick;
        UIController.FinishedCanvasShown += PlayLevelFinished;
        UIController.FailedCanvasShown += PlayLevelFailed;
        Player.PlayerHit += PlayTankHit;
        Enemy.EnemyHitted += PlayTankHit;
        DefaultProjectile.GroundHit += PlayTankHit;
        DefaultProjectile.EdgeOfMapHit += PlayButtonClick;
        EnemyBullet.GroundHit += PlayTankHit;
        EnemyBullet.EdgeOfMapHit += PlayButtonClick;
        ProjectileShooter2D.EnemyShooted += PlayProjectileShoot;
        HangarSceneNavigationManager.ButtonClicked += PlayButtonClick;
        Hangar.ButtonClicked += PlayButtonClick;
        TutorialManager.ButtonClicked += PlayButtonClick;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
        Shoot.PlayerFired -= PlayProjectileShoot;
        UIController.SoundTurnedOff -= StopMainMusic;
        UIController.SoundTurnedOn -= PlayMainMusic;
        StartButton.MainSceneOpened -= PlayButtonClick;
        Shop.CardClicked -= PlayButtonClick;
        MainSceneNavigationManager.ButtonClicked -= PlayButtonClick;
        ShopSceneNavigationManager.ButtonClicked -= PlayButtonClick;
        UIController.ButtonClicked -= PlayButtonClick;
        UIController.FinishedCanvasShown -= PlayLevelFinished;
        UIController.FailedCanvasShown -= PlayLevelFailed;
        Player.PlayerHit -= PlayTankHit;
        Enemy.EnemyHitted -= PlayTankHit;
        DefaultProjectile.GroundHit -= PlayTankHit;
        DefaultProjectile.EdgeOfMapHit -= PlayButtonClick;
        EnemyBullet.GroundHit -= PlayTankHit;
        EnemyBullet.EdgeOfMapHit -= PlayButtonClick;
        ProjectileShooter2D.EnemyShooted -= PlayProjectileShoot;
        HangarSceneNavigationManager.ButtonClicked -= PlayButtonClick;
        Hangar.ButtonClicked -= PlayButtonClick;
        TutorialManager.ButtonClicked -= PlayButtonClick;
    }

    private void PlayMainMusic()
    {
        if (_backgroundMusicSource == null || _mainBackgroundMusic == null)
            return;
        _backgroundMusicSource.clip = _mainBackgroundMusic;
        _backgroundMusicSource.loop = true;
        _backgroundMusicSource.Play();
        _isMusicOn = true;
    }

    private void StopMainMusic()
    {
        if (_backgroundMusicSource == null) return;
        _backgroundMusicSource.Stop();
        _isMusicOn = false;
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        Debug.Log("!UNFOCUSED!");
        AudioListener.pause = inBackground;
        AudioListener.volume = inBackground ? 0f : 1f;
    }

    private void PlayLevelFailed()
    {
        if (_isMusicOn)
        {
            if (_sfxSource == null || _levelFailedSound == null) return;
            _sfxSource.volume = 0.5f;
            _sfxSource.PlayOneShot(_levelFailedSound);
        }
    }

    private void PlayLevelFinished()
    {
        if (_isMusicOn)
        {
            if (_sfxSource == null || _levelFinishedSound == null) return;
            _sfxSource.volume = 0.5f;
            _sfxSource.PlayOneShot(_levelFinishedSound);
        }
    }

    private void PlayProjectileShoot()
    {
        if (_isMusicOn)
        {
            if (_sfxSource == null || _projectileShootedSound == null) return;
            _sfxSource.volume = 1f;
            _sfxSource.PlayOneShot(_projectileShootedSound);
        }
    }

    private void PlayTankHit()
    {
        if (_isMusicOn)
        {
            if (_sfxSource == null || _tankHittedSound == null) return;
            _sfxSource.volume = 1f;
            _sfxSource.PlayOneShot(_tankHittedSound);
        }
    }

    private void PlayButtonClick()
    {
        if (_isMusicOn)
        {
            if (_sfxSource == null || _buttonClickSound == null) return;
            _sfxSource.volume = 1f;
            _sfxSource.PlayOneShot(_buttonClickSound);
        }
    }
}

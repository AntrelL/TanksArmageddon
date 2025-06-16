using UnityEngine;

public class MusicStatusUpdater : MonoBehaviour
{
    [SerializeField] private UIController _UIcontroller;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();

        if (_audioManager.IsMusicOn == true)
        {
            _UIcontroller.UnmuteSound();
        }
        else
        {
            _UIcontroller.MuteSound();
        }
    }
}
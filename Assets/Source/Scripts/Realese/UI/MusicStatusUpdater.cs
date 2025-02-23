using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStatusUpdater : MonoBehaviour
{
    private AudioManager _audioManager;
    [SerializeField] private UIController _UIcontroller;

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

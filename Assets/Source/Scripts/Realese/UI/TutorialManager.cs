using Agava.WebUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _tutorialTips;
    [SerializeField] private GameObject _tutorialBlockUICanvas;
    [SerializeField] private TypewriterEffect _typewriter;
    private int _currentIndex = 0;

    public static event Action<bool> TutorialEnded;
    public static event Action ButtonClicked;

    private void Start()
    {
        foreach (var tip in _tutorialTips)
        {
            var button = tip.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => OnOkayButtonClicked());
        }
    }

    private void OnEnable()
    {
        CameraController.ShowTips += SetTipsStatus;
    }

    private void OnDisable()
    {
        CameraController.ShowTips -= SetTipsStatus;
    }

    private void SetTipsStatus()
    {
        TutorialEnded?.Invoke(false);
        _tutorialBlockUICanvas.SetActive(true);

        for (int i = 0; i < _tutorialTips.Length; i++)
        {
            TMP_Text currentTipText = _tutorialTips[i].GetComponentInChildren<TMP_Text>();

            if (i == 3)
            {
                if (Device.IsMobile == true)
                {
                    currentTipText.text = "Для движения используй кнопки в левом нижнем углу.\r\nДля прицеливания - слайдер справа.\r\nДля стрельбы - кнопку в правом нижнем углу.";
                }
                else
                {
                    currentTipText.text = "Для движения используй клавиши A/D.\r\nДля прицеливания - слайдер справа.\r\nДля стрельбы - кнопку в правом нижнем углу.";
                }
            }

            if (i == 0)
            {
                _tutorialTips[i].SetActive(true);
                _typewriter.GetText(currentTipText);
            }
            else
            {
                _tutorialTips[i].SetActive(false);
            }
        }
    }

    public void OnOkayButtonClicked()
    {
        ButtonClicked?.Invoke();
        TutorialEnded?.Invoke(false);
        _tutorialTips[_currentIndex].SetActive(false);
        _currentIndex++;

        if (_currentIndex < _tutorialTips.Length)
        {
            _tutorialTips[_currentIndex].SetActive(true);
            TMP_Text currentTipText = _tutorialTips[_currentIndex].GetComponentInChildren<TMP_Text>();
            _typewriter.GetText(currentTipText);
        }
        else
        {
            _tutorialBlockUICanvas.SetActive(false);
            TutorialEnded?.Invoke(true);
        }
    }
}

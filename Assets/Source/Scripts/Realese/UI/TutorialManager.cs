using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _tutorialTips;
    [SerializeField] private GameObject _tutorialBlockUICanvas;
    [SerializeField] private TypewriterEffect _typewriter;
    [SerializeField] private int _mobileOrPCTipIndex;

    private int _currentIndex = 0;
    private string _currentLanguage = "ru";

    public static event Action<bool> TutorialEnded;
    public static event Action ButtonClicked;

    private void Start()
    {
        /*
        _currentLanguage = YandexGamesSdk.Environment.i18n.lang;

        foreach (var tip in _tutorialTips)
        {
            var button = tip.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => OnOkayButtonClicked());
        }*/
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

            if (_currentLanguage == "ru")
            {
                if (i == _mobileOrPCTipIndex)
                {
                    /* if (Device.IsMobile == true)
                     {
                         Debug.Log("Platform: mobile!");
                         currentTipText.text = "Для движения используй кнопки в левом нижнем углу.\r\nДля прицеливания - слайдер справа.\r\nДля стрельбы - кнопку в правом нижнем углу.";
                     }
                     else
                     {
                         Debug.Log("Platform: PC!");
                         currentTipText.text = "Для движения используй клавиши A/D.\r\nДля прицеливания - слайдер справа.\r\nДля стрельбы - кнопку в правом нижнем углу.";
                     }*/
                }
            }

            if (_currentLanguage == "en")
            {
                if (i == _mobileOrPCTipIndex)
                {
                    /*if (Device.IsMobile == true)
                    {
                        Debug.Log("Platform: mobile!");
                        currentTipText.text = "Use the buttons in the lower left corner to move.\r\nTo aim, use the slider on the right.\r\nTo fire, press the button in the lower right corner.";
                    }
                    else
                    {
                        Debug.Log("Platform: PC!");
                        currentTipText.text = "Use the A/D keys to move.\r\nTo aim, use the slider on the right.\r\nTo fire, press the button in the lower right corner.";
                    }*/
                }
            }

            /*if (_currentLanguage == "tr")
            {
                if (i == _mobileOrPCTipIndex)
                {
                    if (Device.IsMobile == true)
                    {
                        Debug.Log("Platform: mobile!");
                        currentTipText.text = "Hareket etmek için sol alt köşedeki düğmeleri kullanın.\r\nNişan almak için sağdaki kaydırıcıyı kullanın.\r\nAteş etmek için sağ alt köşedeki düğmeye basın.";
                    }
                    else
                    {
                        Debug.Log("Platform: PC!");
                        currentTipText.text = "Taşımak için A/D tuşlarını kullanın.\r\nNişan almak için sağdaki kaydırıcıyı kullanın.\r\nAteş etmek için sağ alt köşedeki düğmeye basın.";
                    }
                }
            }*/

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

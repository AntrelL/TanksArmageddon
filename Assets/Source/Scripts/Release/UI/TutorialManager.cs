using System;
using Source.Scripts.Release.Stuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Source.Scripts.Release.UI
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _tutorialTips;
        [SerializeField] private GameObject _tutorialBlockUICanvas;
        [SerializeField] private TypewriterEffect _typewriter;
        [SerializeField] private int _mobileOrPCTipIndex;
        [SerializeField] private CameraController _cameraController;

        private int _currentIndex = 0;
        private string _currentLanguage = "ru";
        private AudioManager _manager;

        public event Action<bool> TutorialEnded;
    
        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
        }

        private void Start()
        {
            _currentLanguage = YG2.envir.language;

            foreach (var tip in _tutorialTips)
            {
                var button = tip.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => OnOkayButtonClicked());
            }
        }

        private void OnEnable()
        {
            _cameraController.ShowTips += SetTipsStatus;
        }

        private void OnDisable()
        {
            _cameraController.ShowTips -= SetTipsStatus;
        }

        public void OnOkayButtonClicked()
        {
            _manager.PlayButtonClick();
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

        private void SetTipsStatus()
        {
            TutorialEnded?.Invoke(false);
            _tutorialBlockUICanvas.SetActive(true);

            for (int i = 0; i < _tutorialTips.Length; i++)
            {
                TMP_Text currentTipText = _tutorialTips[i].GetComponentInChildren<TMP_Text>();

                if (i == _mobileOrPCTipIndex)
                {
                    currentTipText.text = GetPlatformSpecificText(_currentLanguage, YG2.envir.isMobile);
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

        private string GetPlatformSpecificText(string language, bool isMobile)
        {
            switch (language)
            {
                case "ru":
                    return isMobile
                        ? "Для движения используй кнопки в левом нижнем углу.\r\nДля прицеливания - слайдер справа.\r\nДля стрельбы - кнопку в правом нижнем углу."
                        : "Для движения используй клавиши A/D.\r\nДля прицеливания - слайдер справа.\r\nДля стрельбы - кнопку в правом нижнем углу.";

                case "en":
                    return isMobile
                        ? "Use the buttons in the lower left corner to move.\r\nTo aim, use the slider on the right.\r\nTo fire, press the button in the lower right corner."
                        : "Use the A/D keys to move.\r\nTo aim, use the slider on the right.\r\nTo fire, press the button in the lower right corner.";

                case "tr":
                    return isMobile
                        ? "Hareket etmek için sol alt köşedeki düğmeleri kullanın.\r\nNişan almak için sağdaki kaydırıcıyı kullanın.\r\nAteş etmek için sağ alt köşedeki düğmeye basın."
                        : "Taşımak için A/D tuşlarını kullanın.\r\nNişan almak için sağdaki kaydırıcıyı kullanın.\r\nAteş etmek için sağ alt köşedeki düğmeye basın.";

                default:
                    return isMobile
                        ? "Use the buttons in the lower left corner to move.\r\nTo aim, use the slider on the right.\r\nTo fire, press the button in the lower right corner."
                        : "Use the A/D keys to move.\r\nTo aim, use the slider on the right.\r\nTo fire, press the button in the lower right corner.";
            }
        }
    }
}
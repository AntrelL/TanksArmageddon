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
    private int currentIndex = 0;

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
            if (i == 0)
            {
                _tutorialTips[i].SetActive(true);
                TMP_Text currentTipText = _tutorialTips[i].GetComponentInChildren<TMP_Text>();
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
        _tutorialTips[currentIndex].SetActive(false);
        currentIndex++;

        if (currentIndex < _tutorialTips.Length)
        {
            _tutorialTips[currentIndex].SetActive(true);
            TMP_Text currentTipText = _tutorialTips[currentIndex].GetComponentInChildren<TMP_Text>();
            _typewriter.GetText(currentTipText);
        }
        else
        {
            _tutorialBlockUICanvas.SetActive(false);
            TutorialEnded?.Invoke(true);
        }
    }
}

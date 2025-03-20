using Agava.YandexGames;
using IJunior.TypedScenes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainSceneNavigationManager : MonoBehaviour
{
    [SerializeField] private GameObject _authView;


    private Dictionary<string, bool> _levelsStatus;

    public static event Action ButtonClicked;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    /*private void GetLevelsStatus()
    {
        foreach (string level in _levelsStatus.Keys)
        {
            if (level == "TrainingScene")
            {
                _checkboxLevel1.SetActive(true);
            }

            if (level == "Level1")
            {
                _checkboxLevel1.SetActive(true);
            }
        }
    }*/

    /*private void SetLevelStatus(string name, bool value)
    {
        if (_levelsStatus.ContainsKey(name) == false)
        {
            _levelsStatus.Add(name, value);
        }
        else
        {
            return;
        }
    }*/

    public void AcceptPressed()
    {
        ButtonClicked?.Invoke();

        if (PlayerAccount.IsAuthorized == false)
        {
            PlayerAccount.Authorize();
            PlayerAccount.RequestPersonalProfileDataPermission();
            _authView.SetActive(false);
        }

        if (PlayerAccount.IsAuthorized)
        {
            PlayerAccount.RequestPersonalProfileDataPermission();
            LeaderboardScene.Load();
        }
    }

    public void DeclinePressed()
    {
        ButtonClicked?.Invoke();
        _authView.SetActive(false);
    }

    public void LoadShopScene()
    {
        ButtonClicked?.Invoke();
        ShopScene.Load();
    }

    public void LoadHangarScene()
    {
        ButtonClicked?.Invoke();
        HangarScene.Load();
    }

    public void LeaderboardButtonPressed()
    {
        ButtonClicked?.Invoke();
        TryOpenLeaderboard();
    }

    private void TryOpenLeaderboard()
    {
        if (PlayerAccount.IsAuthorized)
        {
            PlayerAccount.RequestPersonalProfileDataPermission();
            LeaderboardScene.Load();
        }

        if (PlayerAccount.IsAuthorized == false)
        {
            _authView.SetActive(true);
        }
    }

    public void LoadTrainingLevel()
    {
        ButtonClicked?.Invoke();
        TrainingScene.Load();
    }

    /*public void LoadLevel1()
    {
        ButtonClicked?.Invoke();
        Level1.Load();
    }

    public void LoadLevel2()
    {
        ButtonClicked?.Invoke();
        Level2.Load();
    }

    public void LoadLevel3() 
    {
        ButtonClicked?.Invoke();
        Level3.Load();
    }

    public void LoadLevel4() 
    {
        ButtonClicked?.Invoke();
        Level4.Load();
    }

    public void LoadLevel5()
    {
        ButtonClicked?.Invoke();
        Level5.Load();
    }

    public void LoadLevel6()
    {
        ButtonClicked?.Invoke();
        Level6.Load();
    }

    public void LoadLevel7()
    {
        ButtonClicked?.Invoke();
        Level7.Load();
    }

    public void LoadLevel8()
    {
        ButtonClicked?.Invoke();
        Level8.Load();
    }

    public void LoadLevel9()
    {
        ButtonClicked?.Invoke();
        Level9.Load();
    }

    public void LoadLevel10()
    {
        ButtonClicked?.Invoke();
        Level10.Load();
    }*/
}

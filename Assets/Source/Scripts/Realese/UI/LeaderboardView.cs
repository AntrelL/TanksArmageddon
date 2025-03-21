using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Agava.YandexGames;

public class LeaderboardView : MonoBehaviour
{
    [SerializeField] private TMP_Text[] _ranks;
    [SerializeField] private TMP_Text[] _leaderNames;
    [SerializeField] private TMP_Text[] _scoreList;
    [SerializeField] private string _leaderboardName = "TanksArmageddonLBTest";

    private const string EnglishAnonymousName = "Anonymous";
    private const string RussianAnonymousName = "Аноним";
    private const string TurkishAnonymousName = "Anonim";

    private int _playerScore;

    private void Start()
    {
        GetPlayerScore();
        SetLeaderboardScore();
        OpenYandexLeaderboard();
    }

    public void OpenYandexLeaderboard()
    {
        Leaderboard.GetEntries(_leaderboardName, (result) =>
        {
            int leadersNumber = result.entries.Length >= _leaderNames.Length ? _leaderNames.Length : result.entries.Length;

            for (int i = 0; i < leadersNumber; i++)
            {
                string name = result.entries[i].player.publicName;

                if (string.IsNullOrEmpty(name))
                {
                    string currentLanguage = YandexGamesSdk.Environment.i18n.lang;
                    Debug.Log("currentLanguageCode: " + currentLanguage);

                    switch (currentLanguage)
                    {
                        case "ru":
                            name = RussianAnonymousName;
                            break;
                        case "en":
                            name = EnglishAnonymousName;
                            break;
                        case "tr":
                            name = TurkishAnonymousName;
                            break;
                        default:
                            name = EnglishAnonymousName;
                            break;
                    }
                }

                _leaderNames[i].text = name;
                _scoreList[i].text = result.entries[i].formattedScore;
                _ranks[i].text = result.entries[i].rank.ToString();
            }
        });
    }

    public void SetLeaderboardScore()
    {
        if (YandexGamesSdk.IsInitialized)
        {
            Leaderboard.GetPlayerEntry(_leaderboardName, OnSuccessCallback);
        }
    }

    private void GetPlayerScore()
    {
        _playerScore = GameManager.Instance.GetPlayerPoints();
    }

    private void OnSuccessCallback(LeaderboardEntryResponse result)
    {
        GetPlayerScore();

        if (result == null || _playerScore > result.score)
        {
            Leaderboard.SetScore(_leaderboardName, _playerScore);
        }
    }
}

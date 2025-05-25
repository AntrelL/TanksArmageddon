using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class LeaderboardUpdater : MonoBehaviour
{
    [SerializeField] private string _leaderboardName;

    private void Start()
    {
        YG2.SetLeaderboard(_leaderboardName, YG2.saves.playerPoints);
    }
}

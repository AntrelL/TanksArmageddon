using UnityEngine;
using YG;

namespace Source.Scripts.Realese.Stuff
{
    public class LeaderboardUpdater : MonoBehaviour
    {
        [SerializeField] private string _leaderboardName;

        private void Start()
        {
            YG2.SetLeaderboard(_leaderboardName, YG2.saves.PlayerPoints);
        }
    }
}
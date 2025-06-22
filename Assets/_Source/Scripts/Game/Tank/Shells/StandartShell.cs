using UnityEngine;

namespace TanksArmageddon
{
    public class StandartShell : Shell
    {
        public override void ApplyEffect(Collision2D collision)
        {
            Debug.Log("Просто попали в цель!");
        }
    }
}
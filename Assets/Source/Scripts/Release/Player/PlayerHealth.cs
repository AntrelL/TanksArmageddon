using Source.Scripts.Release.HitProcessing;
using Source.Scripts.Release.Stuff;

namespace Source.Scripts.Release.Player
{
    public class PlayerHealth : Health
    {
        protected override int GetMaxHealth() => PlayerDataHandler.Instance.GetPlayerMaxHealth();
    }
}
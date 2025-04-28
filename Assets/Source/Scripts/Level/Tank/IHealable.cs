namespace TanksArmageddon
{
    public interface IHealable
    {
        void Heal(int amount);
        int CurrentHealth { get; }
        int MaxHealth { get; }
    }
}

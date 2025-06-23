namespace YG
{
    public partial class SavesYG
    {
        public int PlayerPoints = 0;
        public int PlayerBalance = 5000;
        public int PlayerHealth = 1000;
        
        public bool TrainingLevelPassed = false;
        
        public int[] WeaponCardCounts = new int[5];
        
        public ClearWeaponData[] ClearWeaponsData;
    }
}
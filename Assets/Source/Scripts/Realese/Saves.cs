namespace YG
{
    public partial class SavesYG
    {
        public int playerPoints = 0;
        public int playerBalance = 5000;
        public int playerHealth = 1000;
        
        public bool trainingLevelPassed = false;
        
        public int[] weaponCardCounts = new int[5];
        
        public ClearWeaponData[] clearWeaponsData;
    }
}
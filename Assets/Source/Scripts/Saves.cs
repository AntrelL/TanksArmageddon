using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        public int playerPoints = 0;
        public int playerBalance = 5000;
        public int playerHealth = 1000;
    }

    public partial class SavesYG
    {
        public bool trainingLevelPassed = false;
    }

    public partial class SavesYG
    {
        public int[] weaponCardCounts = new int[5];
    }

    public partial class SavesYG
    {
        public ClearWeaponData[] clearWeaponsData;

        //Weapon01
        //currentDamage = 100;
        //upgradeLevel = 0;

        //Weapon02
        //currentDamage = 110;
        //upgradeLevel = 1;

        //Weapon03
        //currentDamage = 200;
        //upgradeLevel = 0;

        //Weapon04
        //currentDamage = 500;
        //upgradeLevel = 0;

        //Weapon05
        //currentDamage = 300;
        //upgradeLevel = 0;
    }
}


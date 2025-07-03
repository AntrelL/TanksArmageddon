using System;

namespace Source.Scripts.Realese.Projectiles
{
    [Serializable]
    public class ClearWeaponData
    {
        public int BaseDamage;
        public int UpgradeLevel;
        public int CurrentDamage;

        public ClearWeaponData(WeaponData weaponData)
        {
            BaseDamage = weaponData.BaseDamage;
            UpgradeLevel = weaponData.UpgradeLevel;
            CurrentDamage = weaponData.CurrentDamage;
        }
    }
}
using UnityEngine;

public class WeaponUpgradeSystem : MonoBehaviour
{
    /*private WeaponData _weaponData;
    private int _currentCards;

    private int[] _cardsForUpgrades = { 10, 20, 30, 50, 100 }; // Карточки для каждого уровня
    private float[] _damageMultipliers = { 1.1f, 1.2f, 1.3f, 1.5f, 2.0f }; // Множители урона

    public void SetWeaponData(WeaponData weaponData)
    {
        _weaponData = weaponData;
    }

    public void SetCards(int cardCount)
    {
        _currentCards = cardCount;
    }

    public void UpgradeWeapon()
    {
        if (_weaponData == null)
        {
            Debug.LogError("Weapon data not set.");
            return;
        }

        int currentLevel = _weaponData.upgradeLevel;

        if (currentLevel < _cardsForUpgrades.Length)
        {
            int cardsRequired = _cardsForUpgrades[currentLevel];

            if (_currentCards >= cardsRequired)
            {
                _currentCards -= cardsRequired;
                _weaponData.upgradeLevel++;

                _weaponData.baseDamage = Mathf.RoundToInt(_weaponData.currentDamage * _damageMultipliers[currentLevel]);

                Debug.Log($"Weapon upgraded to level {_weaponData.upgradeLevel} with new damage {_weaponData.baseDamage}");
            }
            else
            {
                Debug.Log("Not enough cards to upgrade the weapon.");
            }
        }
        else
        {
            Debug.Log("Weapon is already at max level.");
        }
    }

    public int GetCurrentCardsAmount()
    {
        return _currentCards;
    }

    public int GetCardsRequiredForNextLevel(int currentLevel)
    {
        if (currentLevel < _cardsForUpgrades.Length)
        {
            return _cardsForUpgrades[currentLevel];
        }
        else
        {
            return -1;
        }
    }

    public int MaxUpgradeLevel()
    {
        return _cardsForUpgrades.Length;
    }*/
}

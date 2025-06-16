using System;
using TMPro;
using UnityEngine;
using YG;

public class Hangar : MonoBehaviour
{
    [SerializeField] private GameObject[] _upgradeIndicators;
    [SerializeField] private TextMeshProUGUI[] _weaponLevelTexts;
    [SerializeField] private TextMeshProUGUI[] _weaponCardTexts;
    [SerializeField] private TextMeshProUGUI[] _weaponDamageTexts;

    private readonly float[] _damageMultipliers = {1.1f, 1.2f, 1.3f, 1.5f, 2.0f};
    private readonly int[] _requiredCardsForNextLevel = {10, 20, 30, 50, 100};

    private void Start()
    {
        Time.timeScale = 1.0f;
        UpdateWeaponLevelTexts();
        UpdateWeaponDamageTexts();
        UpdateUpgradeIndicators();
        UpdateCardInfoUI();
    }

    public static event Action ButtonClicked;

    private void UpdateUpgradeIndicators()
    {
        for (var i = 0; i < _upgradeIndicators.Length; i++)
        {
            var weaponData = YG2.saves.clearWeaponsData[i];
            var currentCardCount = YG2.saves.weaponCardCounts[i];

            var currentLevel = weaponData.UpgradeLevel;

            if (currentLevel < _requiredCardsForNextLevel.Length &&
                currentCardCount >= _requiredCardsForNextLevel[currentLevel])
                _upgradeIndicators[i].SetActive(true);
            else
                _upgradeIndicators[i].SetActive(false);
        }
    }

    private void UpdateCardInfoUI()
    {
        for (var i = 0; i < _weaponCardTexts.Length; i++)
        {
            var currentCardCount = YG2.saves.weaponCardCounts[i];
            var currentLevel = YG2.saves.clearWeaponsData[i].UpgradeLevel;

            if (currentLevel < _requiredCardsForNextLevel.Length)
            {
                var cardsNeededForNextLevel = _requiredCardsForNextLevel[currentLevel];
                _weaponCardTexts[i].text = $"{currentCardCount}/{cardsNeededForNextLevel}";
            }
            else
            {
                _weaponCardTexts[i].text = "";
            }
        }
    }

    private void UpdateWeaponDamageText(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < _weaponDamageTexts.Length)
            _weaponDamageTexts[weaponIndex].text = $"{YG2.saves.clearWeaponsData[weaponIndex].CurrentDamage}";
    }

    private void UpdateWeaponDamageTexts()
    {
        for (var i = 0; i < _weaponDamageTexts.Length; i++) UpdateWeaponDamageText(i);
    }

    private void UpdateWeaponLevelText(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < _weaponLevelTexts.Length)
        {
            var level = YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel;
            _weaponLevelTexts[weaponIndex].text = $"{level}";
        }
    }

    private void UpdateWeaponLevelTexts()
    {
        for (var i = 0; i < _weaponLevelTexts.Length; i++) UpdateWeaponLevelText(i);
    }

    public void SelectAndUpgradeWeapon(int weaponIndex)
    {
        ButtonClicked?.Invoke();

        var currentCardCount = YG2.saves.weaponCardCounts[weaponIndex];
        var currentLevel = YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel;

        if (YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel < _requiredCardsForNextLevel.Length)
        {
            var cardsNeeded = _requiredCardsForNextLevel[YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel];

            if (currentCardCount >= cardsNeeded)
            {
                YG2.saves.weaponCardCounts[weaponIndex] = currentCardCount - cardsNeeded;
                YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel = currentLevel + 1;

                YG2.saves.clearWeaponsData[weaponIndex].CurrentDamage = Mathf.RoundToInt(
                    YG2.saves.clearWeaponsData[weaponIndex].BaseDamage * _damageMultipliers[currentLevel]);

                YG2.SaveProgress();
                UpdateWeaponLevelText(weaponIndex);
                UpdateWeaponDamageText(weaponIndex);
                UpdateUpgradeIndicators();
                UpdateCardInfoUI();
            }
            else
            {
                Debug.LogError($"Not enough cards for weapon {weaponIndex + 1}. " +
                               $"You have {currentCardCount}, but need {cardsNeeded}.");
            }
        }
        else
        {
            Debug.LogError($"Weapon {weaponIndex + 1} is already at max level.");
            YG2.SaveProgress();
        }
    }
}
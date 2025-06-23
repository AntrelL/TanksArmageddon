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

    private readonly float[] _damageMultipliers = { 1.1f, 1.2f, 1.3f, 1.5f, 2.0f };
    private readonly int[] _requiredCardsForNextLevel = { 10, 20, 30, 50, 100 };

    public static event Action ButtonClicked;

    private void Start()
    {
        Time.timeScale = 1.0f;
        UpdateWeaponLevelTexts();
        UpdateWeaponDamageTexts();
        UpdateUpgradeIndicators();
        UpdateCardInfoUI();
    }

    private void UpdateUpgradeIndicators()
    {
        for (int i = 0; i < _upgradeIndicators.Length; i++)
        {
            ClearWeaponData weaponData = YG2.saves.ClearWeaponsData[i];
            int currentCardCount = YG2.saves.WeaponCardCounts[i];

            int currentLevel = weaponData.UpgradeLevel;

            if (currentLevel < _requiredCardsForNextLevel.Length &&
                currentCardCount >= _requiredCardsForNextLevel[currentLevel])
            {
                _upgradeIndicators[i].SetActive(true);
            }
            else
            {
                _upgradeIndicators[i].SetActive(false);
            }
        }
    }

    private void UpdateCardInfoUI()
    {
        for (int i = 0; i < _weaponCardTexts.Length; i++)
        {
            int currentCardCount = YG2.saves.WeaponCardCounts[i];
            int currentLevel = YG2.saves.ClearWeaponsData[i].UpgradeLevel;

            if (currentLevel < _requiredCardsForNextLevel.Length)
            {
                int cardsNeededForNextLevel = _requiredCardsForNextLevel[currentLevel];
                _weaponCardTexts[i].text = $"{currentCardCount}/{cardsNeededForNextLevel}";
            }
            else
            {
                _weaponCardTexts[i].text = string.Empty;
            }
        }
    }

    private void UpdateWeaponDamageText(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < _weaponDamageTexts.Length)
        {
            _weaponDamageTexts[weaponIndex].text = $"{YG2.saves.ClearWeaponsData[weaponIndex].CurrentDamage}";
        }
    }

    private void UpdateWeaponDamageTexts()
    {
        for (int i = 0; i < _weaponDamageTexts.Length; i++)
        {
            UpdateWeaponDamageText(i);
        }
    }

    private void UpdateWeaponLevelText(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < _weaponLevelTexts.Length)
        {
            int level = YG2.saves.ClearWeaponsData[weaponIndex].UpgradeLevel;
            _weaponLevelTexts[weaponIndex].text = $"{level}";
        }
    }

    private void UpdateWeaponLevelTexts()
    {
        for (int i = 0; i < _weaponLevelTexts.Length; i++)
        {
            UpdateWeaponLevelText(i);
        }
    }

    public void SelectAndUpgradeWeapon(int weaponIndex)
    {
        ButtonClicked?.Invoke();

        int currentCardCount = YG2.saves.WeaponCardCounts[weaponIndex];
        int currentLevel = YG2.saves.ClearWeaponsData[weaponIndex].UpgradeLevel;

        if (YG2.saves.ClearWeaponsData[weaponIndex].UpgradeLevel < _requiredCardsForNextLevel.Length)
        {
            int cardsNeeded = _requiredCardsForNextLevel[YG2.saves.ClearWeaponsData[weaponIndex].UpgradeLevel];

            if (currentCardCount >= cardsNeeded)
            {
                YG2.saves.WeaponCardCounts[weaponIndex] = currentCardCount - cardsNeeded;
                YG2.saves.ClearWeaponsData[weaponIndex].UpgradeLevel = currentLevel + 1;

                YG2.saves.ClearWeaponsData[weaponIndex].CurrentDamage = Mathf.RoundToInt(YG2.saves.ClearWeaponsData[weaponIndex].BaseDamage * _damageMultipliers[currentLevel]);

                YG2.SaveProgress();
                UpdateWeaponLevelText(weaponIndex);
                UpdateWeaponDamageText(weaponIndex);
                UpdateUpgradeIndicators();
                UpdateCardInfoUI();
            }
        }
        else
        {
            YG2.SaveProgress();
        }
    }
}
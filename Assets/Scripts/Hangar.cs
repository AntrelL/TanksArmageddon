using UnityEngine;
using TMPro;
using System;
using YG;

public class Hangar : MonoBehaviour
{
    [SerializeField] private GameObject[] _upgradeIndicators;
    [SerializeField] private TextMeshProUGUI[] _weaponLevelTexts;
    [SerializeField] private TextMeshProUGUI[] _weaponCardTexts;
    [SerializeField] private TextMeshProUGUI[] _weaponDamageTexts;

    private float[] _damageMultipliers = { 1.1f, 1.2f, 1.3f, 1.5f, 2.0f };
    private int[] _requiredCardsForNextLevel = { 10, 20, 30, 50, 100 };

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
            //ClearWeaponData weaponData = GameManager.Instance.GetWeaponData(i);
            ClearWeaponData weaponData = YG2.saves.clearWeaponsData[i];
            //int currentCardCount = GameManager.Instance.GetCardCount(i);
            int currentCardCount = YG2.saves.weaponCardCounts[i];

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
            //ClearWeaponData weaponData = YG2.saves.clearWeaponsData[i];
            //int currentCardCount = GameManager.Instance.GetCardCount(i);
            int currentCardCount = YG2.saves.weaponCardCounts[i];
            int currentLevel = YG2.saves.clearWeaponsData[i].UpgradeLevel;

            if (currentLevel < _requiredCardsForNextLevel.Length)
            {
                int cardsNeededForNextLevel = _requiredCardsForNextLevel[currentLevel];
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
        {
            //ClearWeaponData weaponData = YG2.saves.clearWeaponsData[weaponIndex];
            _weaponDamageTexts[weaponIndex].text = $"{YG2.saves.clearWeaponsData[weaponIndex].CurrentDamage}";
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
            //ClearWeaponData weaponData = GameManager.Instance.GetWeaponData(weaponIndex);
            //ClearWeaponData weaponData = YG2.saves.clearWeaponsData[weaponIndex];

            //if (weaponData != null)
            //{
            //_weaponLevelTexts[weaponIndex].text = $"{weaponData.UpgradeLevel}";
            // }
            int level = YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel;
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
        //ClearWeaponData selectedWeaponData = YG2.saves.clearWeaponsData[weaponIndex];

        /*if (selectedWeaponData == null)
        {
            Debug.LogError($"Weapon data not found for the given weapon index: {weaponIndex}");
            return;
        }*/

        //int currentCardCount = GameManager.Instance.GetCardCount(weaponIndex);
        int currentCardCount = YG2.saves.weaponCardCounts[weaponIndex];
        Debug.Log("currentCardCount: " + currentCardCount);
        //int currentLevel = selectedWeaponData.UpgradeLevel;
        int currentLevel = YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel;

        if (YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel < _requiredCardsForNextLevel.Length)
        {
            int cardsNeeded = _requiredCardsForNextLevel[YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel];

            if (currentCardCount >= cardsNeeded)
            {
                //GameManager.Instance.SetCardCount(weaponIndex, currentCardCount - cardsNeeded);
                YG2.saves.weaponCardCounts[weaponIndex] = currentCardCount - cardsNeeded;
                YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel++;

                YG2.saves.clearWeaponsData[weaponIndex].CurrentDamage = Mathf.RoundToInt(YG2.saves.clearWeaponsData[weaponIndex].BaseDamage * _damageMultipliers[currentLevel]);
                Debug.Log($"Weapon {weaponIndex + 1} upgraded to level {YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel}!");

                //YG2.saves.clearWeaponsData[weaponIndex].CurrentDamage = YG2.saves.clearWeaponsData[weaponIndex].CurrentDamage;
                //YG2.saves.clearWeaponsData[weaponIndex].UpgradeLevel = currentLevel;
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
            //_weaponCardTexts[weaponIndex].gameObject.SetActive(false);
            Debug.LogError($"Weapon {weaponIndex + 1} is already at max level.");
            YG2.SaveProgress();
        }
    }
}

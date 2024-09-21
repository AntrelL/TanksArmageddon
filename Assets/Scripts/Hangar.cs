using UnityEngine;
using TMPro;

public class Hangar : MonoBehaviour
{
    [SerializeField] private GameObject[] _upgradeIndicators; // Иконки, которые показывают возможность улучшения
    [SerializeField] private TextMeshProUGUI[] _weaponLevelTexts;
    [SerializeField] private TextMeshProUGUI[] _weaponCardTexts; // Массив TextMeshPro для отображения количества карточек
    private int[] _requiredCardsForNextLevel = { 10, 20, 30, 50 }; // Требования по карточкам для уровней

    private void Start()
    {
        UpdateWeaponLevelTexts();
        UpdateUpgradeIndicators();
        UpdateCardInfoUI();
    }

    // Метод для обновления иконок улучшения
    private void UpdateUpgradeIndicators()
    {
        for (int i = 0; i < _upgradeIndicators.Length; i++)
        {
            WeaponData weaponData = GameManager.Instance.GetWeaponData(i);
            int currentCardCount = GameManager.Instance.GetCardCount(i);
            int currentLevel = weaponData.upgradeLevel;

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

    // Метод для обновления информации о карточках в UI
    private void UpdateCardInfoUI()
    {
        for (int i = 0; i < _weaponCardTexts.Length; i++)
        {
            WeaponData weaponData = GameManager.Instance.GetWeaponData(i);
            int currentCardCount = GameManager.Instance.GetCardCount(i);
            int currentLevel = weaponData.upgradeLevel;

            if (currentLevel < _requiredCardsForNextLevel.Length)
            {
                int cardsNeededForNextLevel = _requiredCardsForNextLevel[currentLevel];
                _weaponCardTexts[i].text = $"{currentCardCount}/{cardsNeededForNextLevel}";
            }
            else
            {
                _weaponCardTexts[i].text = $"Max Level"; // Если оружие на максимальном уровне
            }
        }
    }

    private void UpdateWeaponLevelText(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < _weaponLevelTexts.Length)
        {
            WeaponData weaponData = GameManager.Instance.GetWeaponData(weaponIndex);
            if (weaponData != null)
            {
                _weaponLevelTexts[weaponIndex].text = $"Lvl.{weaponData.upgradeLevel}";
            }
        }
    }

    private void UpdateWeaponLevelTexts()
    {
        for (int i = 0; i < _weaponLevelTexts.Length; i++)
        {
            UpdateWeaponLevelText(i);
        }
    }

    // Метод для выбора и улучшения оружия при нажатии на кнопку
    public void SelectAndUpgradeWeapon(int weaponIndex)
    {
        WeaponData selectedWeaponData = GameManager.Instance.GetWeaponData(weaponIndex);

        if (selectedWeaponData == null)
        {
            Debug.LogError($"Weapon data not found for the given weapon index: {weaponIndex}");
            return;
        }

        int currentCardCount = GameManager.Instance.GetCardCount(weaponIndex);
        int currentLevel = selectedWeaponData.upgradeLevel;

        if (currentLevel < _requiredCardsForNextLevel.Length)
        {
            int cardsNeeded = _requiredCardsForNextLevel[currentLevel];

            if (currentCardCount >= cardsNeeded)
            {
                GameManager.Instance.SetCardCount(weaponIndex, currentCardCount - cardsNeeded);
                selectedWeaponData.upgradeLevel++;
                Debug.Log($"Weapon {weaponIndex + 1} upgraded to level {selectedWeaponData.upgradeLevel}!");

                // Обновляем индикаторы и текстовую информацию
                UpdateWeaponLevelText(weaponIndex);
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
        }
    }
}

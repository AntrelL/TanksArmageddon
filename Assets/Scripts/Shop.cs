using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerBalanceText; // Текст для отображения баланса игрока
    [SerializeField] private TextMeshProUGUI[] _cardCostTexts; // Массив TextMeshPro элементов для стоимости карточек каждого оружия

    private void Start()
    {
        // Инициализируем текст баланса при старте
        UpdatePlayerBalanceUI();
    }

    // Метод для покупки карточек определенного оружия
    public void BuyWeaponCard(int weaponIndex)
    {
        // Получаем стоимость карточек для данного оружия из TextMeshPro
        int cardCost = GetCardCost(weaponIndex);

        if (GameManager.Instance.TrySpendMoney(cardCost)) // Проверяем, хватает ли денег
        {
            // Увеличиваем количество карточек для этого оружия
            int currentCardCount = GameManager.Instance.GetCardCount(weaponIndex);
            GameManager.Instance.SetCardCount(weaponIndex, currentCardCount + 1);

            Debug.Log($"Card bought for weapon {weaponIndex + 1}. You now have {currentCardCount + 1} cards.");

            // Обновляем баланс в UI
            UpdatePlayerBalanceUI();
        }
        else
        {
            Debug.LogError("Not enough money to buy the card!");
        }
    }

    // Метод для получения стоимости карточек из TextMeshPro
    private int GetCardCost(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < _cardCostTexts.Length)
        {
            // Попытка конвертировать текстовое значение в число
            if (int.TryParse(_cardCostTexts[weaponIndex].text, out int cardCost))
            {
                return cardCost;
            }
            else
            {
                Debug.LogError("Invalid card cost in TextMeshPro for weapon index " + weaponIndex);
                return 0; // Если не удалось считать число, возвращаем 0
            }
        }
        else
        {
            Debug.LogError("Weapon index out of range for card costs.");
            return 0;
        }
    }

    // Метод для обновления UI баланса игрока
    private void UpdatePlayerBalanceUI()
    {
        _playerBalanceText.text = $"{GameManager.Instance.GetPlayerBalance()}";
    }
}

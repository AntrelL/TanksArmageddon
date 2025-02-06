using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerBalanceText;
    [SerializeField] private TextMeshProUGUI[] _cardCostTexts;

    private void Start()
    {
        Time.timeScale = 1f;
        UpdatePlayerBalanceUI();
    }

    public void BuyWeaponCard(int weaponIndex)
    {
        int cardCost = GetCardCost(weaponIndex);

        if (GameManager.Instance.TrySpendMoney(cardCost))
        {
            int currentCardCount = GameManager.Instance.GetCardCount(weaponIndex);
            GameManager.Instance.SetCardCount(weaponIndex, currentCardCount + 1);

            Debug.Log($"Card bought for weapon {weaponIndex + 1}. You now have {currentCardCount + 1} cards.");

            UpdatePlayerBalanceUI();
        }
        else
        {
            Debug.LogError("Not enough money to buy the card!");
        }
    }

    private int GetCardCost(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < _cardCostTexts.Length)
        {
            if (int.TryParse(_cardCostTexts[weaponIndex].text, out int cardCost))
            {
                return cardCost;
            }
            else
            {
                Debug.LogError("Invalid card cost in TextMeshPro for weapon index " + weaponIndex);

                return 0;
            }
        }
        else
        {
            Debug.LogError("Weapon index out of range for card costs.");
            return 0;
        }
    }

    private void UpdatePlayerBalanceUI()
    {
        _playerBalanceText.text = $"{GameManager.Instance.GetPlayerBalance()}";
    }
}

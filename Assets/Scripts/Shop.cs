using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerBalanceText; // ����� ��� ����������� ������� ������
    [SerializeField] private TextMeshProUGUI[] _cardCostTexts; // ������ TextMeshPro ��������� ��� ��������� �������� ������� ������

    private void Start()
    {
        // �������������� ����� ������� ��� ������
        UpdatePlayerBalanceUI();
    }

    // ����� ��� ������� �������� ������������� ������
    public void BuyWeaponCard(int weaponIndex)
    {
        // �������� ��������� �������� ��� ������� ������ �� TextMeshPro
        int cardCost = GetCardCost(weaponIndex);

        if (GameManager.Instance.TrySpendMoney(cardCost)) // ���������, ������� �� �����
        {
            // ����������� ���������� �������� ��� ����� ������
            int currentCardCount = GameManager.Instance.GetCardCount(weaponIndex);
            GameManager.Instance.SetCardCount(weaponIndex, currentCardCount + 1);

            Debug.Log($"Card bought for weapon {weaponIndex + 1}. You now have {currentCardCount + 1} cards.");

            // ��������� ������ � UI
            UpdatePlayerBalanceUI();
        }
        else
        {
            Debug.LogError("Not enough money to buy the card!");
        }
    }

    // ����� ��� ��������� ��������� �������� �� TextMeshPro
    private int GetCardCost(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < _cardCostTexts.Length)
        {
            // ������� �������������� ��������� �������� � �����
            if (int.TryParse(_cardCostTexts[weaponIndex].text, out int cardCost))
            {
                return cardCost;
            }
            else
            {
                Debug.LogError("Invalid card cost in TextMeshPro for weapon index " + weaponIndex);
                return 0; // ���� �� ������� ������� �����, ���������� 0
            }
        }
        else
        {
            Debug.LogError("Weapon index out of range for card costs.");
            return 0;
        }
    }

    // ����� ��� ���������� UI ������� ������
    private void UpdatePlayerBalanceUI()
    {
        _playerBalanceText.text = $"{GameManager.Instance.GetPlayerBalance()}";
    }
}
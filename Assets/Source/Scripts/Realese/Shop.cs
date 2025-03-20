using System;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerBalanceText;
    [SerializeField] private TextMeshProUGUI[] _cardCostTexts;
    [SerializeField] private TMP_Text _purchasedCardsInfo;

    private string _currentLanguage = "ru";

    public static event Action CardClicked;

    private void Start()
    {
        Time.timeScale = 1f;
#if !UNITY_EDITOR && UNITY_WEBGL
        _currentLanguage = Agava.YandexGames.YandexGamesSdk.Environment.i18n.lang;
#endif
        UpdatePlayerBalanceUI();
    }

    public void BuyWeaponCard(int weaponIndex)
    {
        int cardCost = GetCardCost(weaponIndex);

        if (GameManager.Instance.TrySpendMoney(cardCost))
        {
            int currentCardCount = GameManager.Instance.GetCardCount(weaponIndex);
            GameManager.Instance.SetCardCount(weaponIndex, currentCardCount + 1);
            CardClicked?.Invoke();

            if (_currentLanguage == "ru")
            {
                _purchasedCardsInfo.text = $"Куплена карточка для {weaponIndex + 1} снаряда. \r\nКоличество доступных карточек для улучшения: {currentCardCount + 1}";
            }

            if (_currentLanguage == "en")
            {
                _purchasedCardsInfo.text = $"Purchased card for {weaponIndex + 1} projectile. \r\nAmount of available improvement cards: {currentCardCount + 1}";
            }

            if (_currentLanguage == "tr")
            {
                _purchasedCardsInfo.text = $"{weaponIndex + 1} mermisi için kart satın alındı. \r\nGeliştirilebilecek kart sayısı: {currentCardCount + 1}";
            }

            UpdatePlayerBalanceUI();
        }
        else
        {
            _purchasedCardsInfo.text = "Недостаточно ядер.";
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

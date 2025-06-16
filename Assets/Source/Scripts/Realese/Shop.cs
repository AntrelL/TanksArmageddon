using System;
using TMPro;
using UnityEngine;
using YG;

public class Shop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerBalanceText;
    [SerializeField] private TextMeshProUGUI[] _cardCostTexts;
    [SerializeField] private TMP_Text _purchasedCardsInfo;

    private string _currentLanguage = "ru";

    private void Start()
    {
        Time.timeScale = 1f;
        _currentLanguage = YG2.envir.language;
        UpdatePlayerBalanceUI();
    }

    public static event Action CardClicked;

    public void AdReward()
    {
        var ad = "ad";
#if !UNITY_EDITOR && UNITY_WEBGL
        YG2.RewardedAdvShow(ad, () =>
        {
            GameManager.Instance.SetPlayerBalance(1000);
            UpdatePlayerBalanceUI();
        });
#endif
    }

    public void BuyCard(int index)
    {
        var cardCost = GetCardCost(index);

        if (GameManager.Instance.TrySpendMoney(cardCost))
        {
            if (index == 5)
            {
                GameManager.Instance.IncreasePlayerHealth();

                var currentPlayerHealth = GameManager.Instance.GetPlayerHealth();

                if (_currentLanguage == "ru")
                    _purchasedCardsInfo.text =
                        $"Здоровье игрока увеличено на 10. \r\nТекущее здоровье игрока: {currentPlayerHealth}";

                if (_currentLanguage == "en")
                    _purchasedCardsInfo.text =
                        $"Player's health has been increased by 10. \r\nPlayer's current health: {currentPlayerHealth}";

                if (_currentLanguage == "tr")
                    _purchasedCardsInfo.text =
                        $"Oyuncunun sağlığı 10 arttı. \r\nOyuncunun mevcut sağlığı: {currentPlayerHealth}";

                UpdatePlayerBalanceUI();
                CardClicked?.Invoke();

                return;
            }

            var currentCardCount = YG2.saves.weaponCardCounts[index];

            YG2.saves.weaponCardCounts[index] = currentCardCount + 1;
            CardClicked?.Invoke();

            if (_currentLanguage == "ru")
                _purchasedCardsInfo.text =
                    $"Куплена карточка для {index + 1} снаряда. \r\nКоличество доступных карточек для улучшения: {currentCardCount + 1}";

            if (_currentLanguage == "en")
                _purchasedCardsInfo.text =
                    $"Purchased card for {index + 1} projectile. \r\nAmount of available improvement cards: {currentCardCount + 1}";

            if (_currentLanguage == "tr")
                _purchasedCardsInfo.text =
                    $"{index + 1} mermisi için kart satın alındı. \r\nGeliştirilebilecek kart sayısı: {currentCardCount + 1}";

            UpdatePlayerBalanceUI();
        }
        else
        {
            NotEnoughCoresWarning();
        }
    }

    private void NotEnoughCoresWarning()
    {
        if (_currentLanguage == "ru") _purchasedCardsInfo.text = "Недостаточно ядер.";

        if (_currentLanguage == "en") _purchasedCardsInfo.text = "Not enough cores.";

        if (_currentLanguage == "tr") _purchasedCardsInfo.text = "Yeterli çekirdek yok.";
    }

    private int GetCardCost(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < _cardCostTexts.Length)
        {
            if (int.TryParse(_cardCostTexts[weaponIndex].text, out var cardCost))
                return cardCost;
            return 0;
        }

        return 0;
    }

    private void UpdatePlayerBalanceUI()
    {
        _playerBalanceText.text = $"{GameManager.Instance.GetPlayerBalance()}";
    }
}
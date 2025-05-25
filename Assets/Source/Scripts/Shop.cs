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

    public static event Action CardClicked;

    private void Start()
    {
        Time.timeScale = 1f;
        _currentLanguage = YG2.envir.language;
        UpdatePlayerBalanceUI();
    }

    public void AdReward()
    {

        string hyiPoimiShop = "hyiShop";
#if !UNITY_EDITOR && UNITY_WEBGL
        YG2.RewardedAdvShow(hyiPoimiShop, () =>
        {
            GameManager.Instance.SetPlayerBalance(1000);
            UpdatePlayerBalanceUI();
        });
#endif
    }

    public void BuyCard(int index)
    {
        int cardCost = GetCardCost(index);

        if (GameManager.Instance.TrySpendMoney(cardCost))
        {
            if (index == 5)
            {
                GameManager.Instance.IncreasePlayerHealth();

                int currentPlayerHealth = GameManager.Instance.GetPlayerHealth();

                if (_currentLanguage == "ru")
                {
                    _purchasedCardsInfo.text = $"Здоровье игрока увеличено на 10. \r\nТекущее здоровье игрока: {currentPlayerHealth}";
                }

                if (_currentLanguage == "en")
                {
                    _purchasedCardsInfo.text = $"Player's health has been increased by 10. \r\nPlayer's current health: {currentPlayerHealth}";
                }

                if (_currentLanguage == "tr")
                {
                    _purchasedCardsInfo.text = $"Oyuncunun sağlığı 10 arttı. \r\nOyuncunun mevcut sağlığı: {currentPlayerHealth}";
                }

                //YG2.SaveProgress();
                UpdatePlayerBalanceUI();
                CardClicked?.Invoke();

                return;
            }

            //int currentCardCount = GameManager.Instance.GetCardCount(index);
            int currentCardCount = YG2.saves.weaponCardCounts[index];

            //GameManager.Instance.SetCardCount(index, currentCardCount + 1);
            YG2.saves.weaponCardCounts[index] = currentCardCount + 1;
            CardClicked?.Invoke();

            if (_currentLanguage == "ru")
            {
                _purchasedCardsInfo.text = $"Куплена карточка для {index + 1} снаряда. \r\nКоличество доступных карточек для улучшения: {currentCardCount + 1}";
            }

            if (_currentLanguage == "en")
            {
                _purchasedCardsInfo.text = $"Purchased card for {index + 1} projectile. \r\nAmount of available improvement cards: {currentCardCount + 1}";
            }

            if (_currentLanguage == "tr")
            {
                _purchasedCardsInfo.text = $"{index + 1} mermisi için kart satın alındı. \r\nGeliştirilebilecek kart sayısı: {currentCardCount + 1}";
            }

            //YG2.SaveProgress();
            UpdatePlayerBalanceUI();
        }
        else
        {
            //YG2.SaveProgress();
            NotEnoughCoresWarning();
        }
    }

    private void NotEnoughCoresWarning()
    {
        if (_currentLanguage == "ru")
        {
            _purchasedCardsInfo.text = "Недостаточно ядер.";
        }

        if (_currentLanguage == "en")
        {
            _purchasedCardsInfo.text = "Not enough cores.";
        }

        if (_currentLanguage == "tr")
        {
            _purchasedCardsInfo.text = "Yeterli çekirdek yok.";
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

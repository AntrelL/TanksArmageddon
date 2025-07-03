using TMPro;
using UnityEngine;
using YG;

namespace Source.Scripts.Realese.Stuff
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerBalanceText;
        [SerializeField] private TextMeshProUGUI[] _cardCostTexts;
        [SerializeField] private TMP_Text _purchasedCardsInfo;

        private string _currentLanguage = "ru";
        private AudioManager _manager;

        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
        }
    
        private void Start()
        {
            Time.timeScale = 1f;
            _currentLanguage = YG2.envir.language;
            UpdatePlayerBalanceUI();
        }

        public void AdReward()
        {
            string ad = "ad";
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
            int cardCost = GetCardCost(index);

            if (PlayerDataHandler.Instance.TrySpendMoney(cardCost))
            {
                if (index == 5)
                {
                    PlayerDataHandler.Instance.IncreasePlayerHealth();

                    int currentPlayerHealth = PlayerDataHandler.Instance.GetPlayerHealth();
                    UpdateHealthUpgrade(currentPlayerHealth);

                    UpdatePlayerBalanceUI();
                    _manager.PlayButtonClick();

                    return;
                }

                int currentCardCount = YG2.saves.WeaponCardCounts[index];
                YG2.saves.WeaponCardCounts[index] = currentCardCount + 1;
                _manager.PlayButtonClick();

                UpdateWeaponUpgrade(index, currentCardCount + 1);

                UpdatePlayerBalanceUI();
            }
            else
            {
                NotEnoughCoresWarning();
            }
        }

        private void NotEnoughCoresWarning()
        {
            switch (_currentLanguage)
            {
                case "ru":
                    _purchasedCardsInfo.text = "Недостаточно ядер.";
                    break;
            
                case "en":
                    _purchasedCardsInfo.text = "Not enough cores.";
                    break;
            
                case "tr":
                    _purchasedCardsInfo.text = "Yeterli çekirdek yok.";
                    break;
            
                default:
                    _purchasedCardsInfo.text = "Not enough cores.";
                    break;
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
            }
        
            return 0;
        }

        private void UpdatePlayerBalanceUI()
        {
            _playerBalanceText.text = $"{PlayerDataHandler.Instance.GetPlayerBalance()}";
        }

        private void UpdateHealthUpgrade(int currentPlayerHealth)
        {
            switch (_currentLanguage)
            {
                case "ru":
                    _purchasedCardsInfo.text = $"Здоровье игрока увеличено на 10. \r\nТекущее здоровье игрока: {currentPlayerHealth}";
                    break;
            
                case "en":
                    _purchasedCardsInfo.text = $"Player's health has been increased by 10. \r\nPlayer's current health: {currentPlayerHealth}";
                    break;
            
                case "tr":
                    _purchasedCardsInfo.text = $"Oyuncunun sağlığı 10 arttı. \r\nOyuncunun mevcut sağlığı: {currentPlayerHealth}";
                    break;
            
                default:
                    _purchasedCardsInfo.text = $"Player's health has been increased by 10. \r\nPlayer's current health: {currentPlayerHealth}";
                    break;
            }
        }

        private void UpdateWeaponUpgrade(int index, int cardCount)
        {
            switch (_currentLanguage)
            {
                case "ru":
                    _purchasedCardsInfo.text = $"Куплена карточка для {index + 1} снаряда. \r\nКоличество доступных карточек для улучшения: {cardCount}";
                    break;
            
                case "en":
                    _purchasedCardsInfo.text = $"Purchased card for {index + 1} projectile. \r\nAmount of available improvement cards: {cardCount}";
                    break;
            
                case "tr":
                    _purchasedCardsInfo.text = $"{index + 1} mermisi için kart satın alındı. \r\nGeliştirilebilecek kart sayısı: {cardCount}";
                    break;
            
                default:
                    _purchasedCardsInfo.text = $"Purchased card for {index + 1} projectile. \r\nAmount of available improvement cards: {cardCount}";
                    break;
            }
        }
    }
}
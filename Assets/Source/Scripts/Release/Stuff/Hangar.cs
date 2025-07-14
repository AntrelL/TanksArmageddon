using Source.Scripts.Release.Projectiles;
using TMPro;
using UnityEngine;
using YG;

namespace Source.Scripts.Release.Stuff
{
    public class Hangar : MonoBehaviour
    {
        private readonly float[] _damageMultipliers = { 1.1f, 1.2f, 1.3f, 1.5f, 2.0f };
        private readonly int[] _requiredCardsForNextLevel = { 10, 20, 30, 50, 100 };

        [SerializeField] private GameObject[] _upgradeIndicators;
        [SerializeField] private TextMeshProUGUI[] _weaponLevelTexts;
        [SerializeField] private TextMeshProUGUI[] _weaponCardTexts;
        [SerializeField] private TextMeshProUGUI[] _weaponDamageTexts;

        private AudioManager _manager;

        private void Awake()
        {
            _manager = FindObjectOfType<AudioManager>();
        }

        private void Start()
        {
            Time.timeScale = 1.0f;
            UpdateWeaponLevelTexts();
            UpdateWeaponDamageTexts();
            UpdateUpgradeIndicators();
            UpdateCardInfoUI();
        }

        public void OnUpgradeWeaponButtonClicked(int weaponIndex) => 
            TryUpgradeWeapon(weaponIndex);

        private bool TryUpgradeWeapon(int weaponIndex)
        {
            _manager.PlayButtonClick();

            var saves = YG2.saves;
            var weaponData = saves.ClearWeaponsData[weaponIndex];
            int currentLevel = weaponData.UpgradeLevel;

            if (currentLevel >= _requiredCardsForNextLevel.Length)
            {
                YG2.SaveProgress();
                return false;
            }

            int currentCardCount = saves.WeaponCardCounts[weaponIndex];
            int cardsNeeded = _requiredCardsForNextLevel[currentLevel];

            if (currentCardCount < cardsNeeded)
                return false;
            
            saves.WeaponCardCounts[weaponIndex] = currentCardCount - cardsNeeded;
            weaponData.UpgradeLevel = currentLevel + 1;
            weaponData.CurrentDamage = Mathf.RoundToInt(
                weaponData.BaseDamage * _damageMultipliers[currentLevel]);

            YG2.SaveProgress();
            
            UpdateWeaponLevelText(weaponIndex);
            UpdateWeaponDamageText(weaponIndex);
            UpdateUpgradeIndicators();
            UpdateCardInfoUI();

            return true;
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
                _weaponDamageTexts[weaponIndex].text = 
                    $"{YG2.saves.ClearWeaponsData[weaponIndex].CurrentDamage}";
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
    }
}

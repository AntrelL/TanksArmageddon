using UnityEngine;
using YG;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WeaponData[] _weaponDataList = new WeaponData[5];

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        UIController.PlayerRewardReceived += SetPlayerBalance;
        UIController.PlayerPointsReceived += SetPlayerPoints;
    }

    private void OnDisable()
    {
        UIController.PlayerRewardReceived -= SetPlayerBalance;
        UIController.PlayerPointsReceived -= SetPlayerPoints;
    }

    public void IncreasePlayerHealth()
    {
        YG2.saves.playerHealth += 10;
    }

    public int GetPlayerHealth()
    {
        return YG2.saves.playerHealth;
    }

    private void SetPlayerPoints(int value)
    {
        YG2.saves.playerPoints += value;
    }

    public int GetPlayerPoints()
    {
        return YG2.saves.playerPoints;
    }

    public int GetPlayerMaxHealth()
    {
        return YG2.saves.playerHealth;
    }

    public int GetCardCount(int weaponIndex)
    {
        return YG2.saves.weaponCardCounts[weaponIndex];
    }

    public void SetCardCount(int weaponIndex, int cardCount)
    {
        YG2.saves.weaponCardCounts[weaponIndex] = cardCount;
    }

    public ClearWeaponData GetWeaponData(int weaponIndex)
    {
        return YG2.saves.clearWeaponsData[weaponIndex];
    }

    public int GetPlayerBalance()
    {
        return YG2.saves.playerBalance;
    }

    public void SetPlayerBalance(int amount)
    {
        YG2.saves.playerBalance += amount;
        YG2.SaveProgress();
    }

    public bool TrySpendMoney(int amount)
    {
        if (YG2.saves.playerBalance >= amount)
        {
            YG2.saves.playerBalance -= amount;

            return true;
        }

        return false;
    }
}
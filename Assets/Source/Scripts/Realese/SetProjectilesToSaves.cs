using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SetProjectilesToSaves : MonoBehaviour
{
    [SerializeField] private WeaponData[] weapons;

    private void Start()
    {
        if (YG2.saves.clearWeaponsData == null)
        {
            YG2.saves.clearWeaponsData = Array.ConvertAll(weapons, weapon => new ClearWeaponData(weapon));
            YG2.SaveProgress();
            Debug.Log("YG2.saves.clearWeaponsData == null, " + YG2.saves.clearWeaponsData);
        }
        else
        {
            Debug.Log("YG2.saves.clearWeaponsData != null, " + YG2.saves.clearWeaponsData);
            Debug.Log("Data YG2.saves.clearWeaponsData[0]" + YG2.saves.clearWeaponsData[0]);
        }

    }
}

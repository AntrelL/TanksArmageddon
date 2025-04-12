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
        if (YG2.saves.clearWeaponsData is null)
        {
            YG2.saves.clearWeaponsData = Array.ConvertAll(weapons, weapon => new ClearWeaponData(weapon));
        }

        YG2.SaveProgress();
    }
}

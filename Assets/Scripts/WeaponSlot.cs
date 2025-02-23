using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI currentDamage;
    public Image highlight;

    private bool _isSelected = false;

    public void SetWeaponData(WeaponData weapon)
    {
        icon.sprite = weapon.icon;
        currentDamage.text = weapon.currentDamage.ToString();
        UpdateHighlight();
    }

    public void SelectWeapon()
    {
        if (_isSelected)
        {
            UpdateHighlight();

        }
    }

    public void Select()
    {
        _isSelected = true;
        UpdateHighlight();
    }

    public void Deselect()
    {
        _isSelected = false;
        UpdateHighlight();
    }

    private void UpdateHighlight()
    {
        if (highlight != null)
        {
            highlight.enabled = _isSelected;
        }
    }
}

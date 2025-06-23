using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI CurrentDamage;
    public Image Highlight;

    [SerializeField] private TMP_Text _ammoCountText;

    private bool _isSelected = false;
    
    public void SetWeaponData(ClearWeaponData weapon)
    {
        CurrentDamage.text = weapon.CurrentDamage.ToString();
        UpdateHighlight();
    }

    public void SelectWeapon()
    {
        if (_isSelected)
        {
            UpdateHighlight();
        }
    }

    public void UpdateAmmoCount(int ammoCount)
    {
        if (_ammoCountText == null) 
            return;

        if (ammoCount > 0)
        {
            _ammoCountText.text = ammoCount.ToString();
            _ammoCountText.gameObject.SetActive(true);
        }
        else
        {
            _ammoCountText.gameObject.SetActive(false);
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
        if (Highlight != null)
        {
            Highlight.enabled = _isSelected;
        }
    }
}
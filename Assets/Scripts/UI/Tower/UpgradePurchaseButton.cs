using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.ComponentModel;
using System.Reflection;

public class UpgradePurchaseButton : PurchaseButton
{
    [SerializeField, Tooltip("Text component to show off the description.")]
    private TMP_Text descriptionText;
    
    private int _cost;

    private int _upgradeIndex;

    private UpgradeScript _upgradeScript;
    
    private UpgradeShopSetupScript _upgradeShopSetupScript;

    [SerializeField, Tooltip("Text component to show off that you bought the upgrade.")]
    private GameObject boughtText;
    
    private static readonly Dictionary<Upgrade.UpgradeType, string> attackNames = new()
    {
        { Upgrade.UpgradeType.Range, "Range" },
        { Upgrade.UpgradeType.FireRate, "Fire Rate" },
    };
    
    public override void Purchase()
    {
        if (_upgradeScript == null) return;
        
        if (GameManager.instance != null)
            GameManager.instance.ChargePlayer(_cost);
            
        _upgradeScript.ApplyUpgrade(_upgradeIndex);

        SetUpBoughtButton(true);
        
        if (_upgradeShopSetupScript != null)
            _upgradeShopSetupScript.UpdateText(_upgradeScript);
    }

    public static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }
    
    public void SetUpgrade(UpgradeScript pScript, Upgrade pUpgrade, int pUpgradeIndex, UpgradeShopSetupScript pUSSS)
    {
        SetUpBoughtButton(pUpgrade.upgradeBought);
        
        if (pUpgrade.upgradeBought) return;
        
        _upgradeIndex = pUpgradeIndex;
        _upgradeScript = pScript;
        
        nameText.SetText(GetEnumDescription(pUpgrade.type));
        string valueText = "";
        switch (pUpgrade.type)
        {
            case Upgrade.UpgradeType.Range:
                valueText = $"+{pUpgrade.addedValue} Range";
                break;
            case Upgrade.UpgradeType.FireRate:
                valueText = $"-{pUpgrade.addedValue} Second";
                if (pUpgrade.addedValue > 1) valueText += "s";
                break;
        }
        
        descriptionText.SetText(valueText);
        
        _cost = pUpgrade.cost;
        OnMoneyChanged(GameManager.instance.Money);
        
        costText.SetText(_cost.ToString());

        _upgradeShopSetupScript = pUSSS;
    }

    private void SetUpBoughtButton(bool bought)
    {
        boughtText.SetActive(bought);
        nameText.gameObject.SetActive(!bought);
        descriptionText.gameObject.SetActive(!bought);
        costText.gameObject.SetActive(!bought);

        button.interactable = !bought;
        if (button.interactable && GameManager.instance != null)
            OnMoneyChanged(GameManager.instance.Money);
    }

    protected override void OnMoneyChanged(int pMoney)
    {
        lastCheckedMoney = _cost <= pMoney;
        button.interactable = lastCheckedMoney && lastCheckedState;
    }
}
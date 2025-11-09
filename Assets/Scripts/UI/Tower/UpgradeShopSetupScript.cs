using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeShopSetupScript : MonoBehaviour
{
    [Header("Shop Settings")] 
    [SerializeField, Tooltip("Set the icon that updates to the corrosponding tower.")]
    private Image icon = null;
    [SerializeField, Tooltip("Set the text that updates the damage.")]
    private TMP_Text damage = null;
    [SerializeField, Tooltip("Set the text that updates the range.")]
    private TMP_Text range = null;
    [SerializeField, Tooltip("Set the text that updates the attack interval.")]
    private TMP_Text attackInterval = null;
    [SerializeField, Tooltip("Set the text that updates the attack type.")]
    private TMP_Text attackType = null;
    [SerializeField, Tooltip("Set the upgrade buttons.")]
    private UpgradePurchaseButton[] upgradeButtons = null;
    
    // Map type to readable string
    private static readonly Dictionary<Type, string> attackNames = new()
    {
        { typeof(SingleFireTowerAttack), "Single Fire" },
        { typeof(AOEAttack), "Area Of Effect" },
        { typeof(DebuffAttack), "Debuff" }
    };
    
    public void SetupShop(UpgradeScript pUS, Sprite pTowerIcon)
    {
        damage.SetText(pUS.TA.CurrentAttackDamage.ToString());
        range.SetText(pUS.TA.CurrentAttackRange.ToString());
        attackInterval.SetText(pUS.TA.CurrentAttackRate.ToString());
        
        if (attackNames.TryGetValue(pUS.TA.GetType(), out string attackTypeDisplay))
            attackType.SetText(attackTypeDisplay);
        
        if (pTowerIcon != null)
            icon.sprite = pTowerIcon;

        if (upgradeButtons != null && upgradeButtons.Length > 0)
        {
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                if (pUS.Upgrades.Length <= i) break;
                
                upgradeButtons[i].SetUpgrade(pUS, pUS.Upgrades[i], i, this);
            }
        }
    }

    public void UpdateText(UpgradeScript pUS)
    {
        if (pUS == null && pUS.TA == null) return;
        
        damage.SetText(pUS.TA.CurrentAttackDamage.ToString());
        range.SetText(pUS.TA.CurrentAttackRange.ToString());
        attackInterval.SetText(pUS.TA.CurrentAttackRate.ToString());
    }
}

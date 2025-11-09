using System;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public struct Upgrade
{
    public enum UpgradeType
    {
        [Description("Range")]
        Range,
        [Description("Fire Rate")]
        FireRate
    }

    [Tooltip("Set the upgrade type.")]
    public UpgradeType type;
    [Tooltip("Set the upgrade amount. This amount will be added ON TOP of the original value."), Min(0)]
    public float addedValue;
    [Tooltip("Set the amount that the upgrade is going to cost."), Min(0)]
    public int cost;

    [NonEditable]
    public bool upgradeBought;

    [Tooltip("The renderer of the object for the upgrade.")]
    public Renderer[] rend;
    [Tooltip("The new color that the selected renderer will render after buying the upgrade.")]
    public Color upgradedColor;
}

public class UpgradeScript : MonoBehaviour
{
    // Allowing inspector to change the upgrades.
    [Header("Upgrade Settings")]
    [SerializeField, Tooltip("Set the upgrade amount.")] 
    private Upgrade[] _upgrades;

    public Upgrade[] Upgrades
    {
        private set => _upgrades = value;
        get => _upgrades;
    }

    public TowerAttack TA { private set; get; }
    
    public RangeRenderer RR { private set; get; }

    private void Awake()
    {
        TA = GetComponent<TowerAttack>();
        RR = GetComponent<RangeRenderer>();
    }

    public void ApplyUpgrade(int upgradeId)
    {
        // Caching for easier readability.
        Upgrade upgrade = _upgrades[upgradeId];
        TA.ApplyUpgrade(upgrade.type, upgrade.addedValue);

        // Making sure the upgrades in the instance knows it has been bought.
        Upgrades[upgradeId].upgradeBought = true;
        
        if (upgrade.rend != null)
        {
            foreach (Renderer rend in upgrade.rend)
                rend.material.color = upgrade.upgradedColor;
        }
        
        RR.UpdateCircle(true);
    }
}
using UnityEngine;

[System.Serializable]
public class RangeUpgrades
{
    public float rangeUpgrade;
    public int cost;
    public string description;
}

[System.Serializable]
public class FireRateUpgrades
{
    public float fireRateUpgrade;
    public int cost;
}

[CreateAssetMenu(fileName = "Tower", menuName = "TowerDefenseAssets/Tower", order = 1)]
public class TowerScriptableObjects : ScriptableObject
{
    [Header("Tower Settings")]
    [Tooltip("Set the Tower GameObject.")]
    public GameObject tower = null;
    [Tooltip("Set how much this tower cost."), Min(0)]
    public int price = 100;
    [Tooltip("Set how the tower fires.")]
    public FireMode fireMode = FireMode.single;
    [Tooltip("Set how fast the tower shoots in seconds."), Min(0.1f)]
    public float fireRate = 3;
    [Tooltip("Set how much range this tower has."), Min(0.5f)]
    public float range = 3;
    [Tooltip("Set which bullet the tower fires (if applicable).")]
    public GameObject bullet = null;

    // [Header("Upgrade Settings")]
    [Tooltip("Each element defines how much range will be added to the current range when bought.")]
    public RangeUpgrades[] rangeUpgrades;
    [Tooltip("Each element defines how much faster (in seconds) the tower will fire when bought.")]
    public FireRateUpgrades[] fireRateUpgrades = null;
}